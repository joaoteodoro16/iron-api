using FluentValidation;
using Iron.Aplication.Common;
using Iron.Aplication.DTOs.Auth.Request;
using Iron.Aplication.DTOs.Auth.Response;
using Iron.Aplication.Services;
using Iron.Aplication.Validators;
using Iron.Domain.Common;
using Iron.Domain.Repositories;
using Iron.Domain.ValueObjects;

namespace Iron.Aplication.Usecases.Auth;

public class AuthUserUsecase(ITokenService tokenService, IUserRepository userRepository, IValidator<AuthUserRequest> validator, IPasswordHasher passwordHasher,
IRefreshTokenRepository refreshTokenRepository)
{
    private readonly ITokenService _tokenService = tokenService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IValidator<AuthUserRequest> _validator = validator;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    public async Task<Result<AuthUserResponse>> ExecuteAsync(AuthUserRequest request)
    {
        var validatorResult = await _validator.ValidateAsync(request);
        if (!validatorResult.IsValid)
            return Result.Fail<AuthUserResponse>(validatorResult.Errors.Select(e => e.ErrorMessage).ToList(), ErrorType.Validation, AuthMessages.InvalidFields);

        var emailResult = Result.Try(() => Email.Create(request.Email));
        if (emailResult.IsFailure)
            return Result.Fail<AuthUserResponse>(AuthMessages.InvalidCredentials, ErrorType.Unauthorized);

        var user = await _userRepository.GetByEmailAsync(emailResult.Value);

        if (user == null)
            return Result.Fail<AuthUserResponse>(AuthMessages.InvalidCredentials, ErrorType.Unauthorized);

        var validPassword = _passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!validPassword)
            return Result.Fail<AuthUserResponse>(AuthMessages.InvalidCredentials, ErrorType.Unauthorized);

        var tokenResult = Result.Try(() => _tokenService.GenerateToken(user));
        if (tokenResult.IsFailure)
            return Result.Fail<AuthUserResponse>(AuthMessages.UnexpectedError, ErrorType.Failure);



        var dto = new AuthUserResponse(user.Id, user.FirstName, user.LastName, user.Email.Value, user.EmailConfirmed, user.IsPlatformAdmin, tokenResult.Value);
        return Result.Ok<AuthUserResponse>(dto, AuthMessages.UserAuthenticated);
    }

}
