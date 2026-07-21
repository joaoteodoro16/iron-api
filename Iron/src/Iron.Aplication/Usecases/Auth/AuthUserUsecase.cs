using FluentValidation;
using Iron.Aplication.Common;
using Iron.Aplication.DTOs.Auth.Request;
using Iron.Aplication.DTOs.Auth.Response;
using Iron.Aplication.Services;
using Iron.Aplication.Validators;
using Iron.Domain.Common;
using Iron.Domain.Entities;
using Iron.Domain.Repositories;
using Iron.Domain.ValueObjects;

namespace Iron.Aplication.Usecases.Auth;

public class AuthUserUsecase
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly IValidator<AuthUserRequest> _validator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AuthUserUsecase(ITokenService tokenService, IUserRepository userRepository, IValidator<AuthUserRequest> validator, IPasswordHasher passwordHasher,
        IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
        _validator = validator;
        _passwordHasher = passwordHasher;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }

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

        var token = _tokenService.GenerateToken(user);
        var (refreshToken, refreshExpiresAt) = _tokenService.GenerateRefreshToken();

        await _refreshTokenRepository.AddAsync(RefreshToken.Create(user.Id, refreshToken, refreshExpiresAt));
        await _unitOfWork.SaveChangesAsync();

        var dto = new AuthUserResponse(user.Id, user.FirstName, user.LastName, user.Email.Value, user.EmailConfirmed, user.IsPlatformAdmin, token, refreshToken);
        return Result.Ok<AuthUserResponse>(dto, AuthMessages.UserAuthenticated);
    }
}
