using FluentValidation;
using Iron.Aplication.DTOs;
using Iron.Domain.Common;
using Iron.Domain.Entities;
using Iron.Domain.Repositories;
using Iron.Domain.Security;
using Iron.Domain.ValueObjects;

namespace Iron.Aplication.Usecases.Auth;

public class CreateUserUsecase(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IValidator<CreateUserRequest> validator)
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IValidator<CreateUserRequest> _validator = validator;

    public async Task<Result<long>> ExecuteAsync(CreateUserRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result.Fail<long>(validationResult.Errors.Select(e => e.ErrorMessage).ToList(), ErrorType.Validation, "Existem campos inválidos");

        var emailResult = Result.Try(() => Email.Create(request.Email));
        if (emailResult.IsFailure)
            return Result.Fail<long>(emailResult.Error, emailResult.ErrorType, "Email inválido");

        var phoneResult = Result.Try(() => PhoneNumber.Create(request.PhoneNumber));
        if (phoneResult.IsFailure)
            return Result.Fail<long>(phoneResult.Error, phoneResult.ErrorType, "Número de telefone inválido");

        var emailInUse = await _userRepository.ExistsByEmailAsync(emailResult.Value);
        if (emailInUse)
            return Result.Fail<long>("Já existe um usuário cadastrado com esse email.", ErrorType.Conflict);

        var passwordHash = _passwordHasher.Hash(request.Password);

        var userResult = Result.Try(() => User.Create(
            request.FirstName,
            request.LastName,
            emailResult.Value,
            passwordHash,
            phoneResult.Value,
            request.isPlatformAdmin
        ));

        if (userResult.IsFailure)
            return Result.Fail<long>(userResult.Error, userResult.ErrorType, "Não foi possível criar o usuário");

        await _userRepository.AddAsync(userResult.Value);
        await _unitOfWork.SaveChangesAsync();

        return Result.Ok(userResult.Value.Id, "Usuário criado com sucesso.");
    }
}
