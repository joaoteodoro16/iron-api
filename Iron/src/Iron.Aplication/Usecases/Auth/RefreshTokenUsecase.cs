using Iron.Aplication.Common;
using Iron.Aplication.DTOs.Auth.Response;
using Iron.Aplication.Services;
using Iron.Domain.Common;
using Iron.Domain.Entities;
using Iron.Domain.Repositories;

namespace Iron.Aplication.Usecases.Auth;

public class RefreshTokenUsecase(ITokenService tokenService, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork)
{
    public async Task<Result<RefreshTokenResponse>> ExecuteAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return Result.Fail<RefreshTokenResponse>(AuthMessages.Unauthenticated, ErrorType.Unauthorized, message: AuthMessages.Unauthenticated);

        var storedToken = await refreshTokenRepository.GetByTokenAsync(token);
        if (storedToken is null || !storedToken.IsActive)
            return Result.Fail<RefreshTokenResponse>(AuthMessages.Unauthenticated, ErrorType.Unauthorized, message: AuthMessages.Unauthenticated);


        var user = await userRepository.GetByIdAsync(storedToken.UserId);
        if (user is null)
            return Result.Fail<RefreshTokenResponse>(AuthMessages.Unauthenticated, ErrorType.Unauthorized, message: AuthMessages.Unauthenticated);


        storedToken.Revoke();
        refreshTokenRepository.Update(storedToken);

        var accessToken = tokenService.GenerateToken(user);
        var (refreshToken, refreshExpiresAt) = tokenService.GenerateRefreshToken();

        await refreshTokenRepository.AddAsync(RefreshToken.Create(user.Id, refreshToken, refreshExpiresAt));

        await unitOfWork.SaveChangesAsync();

        var response = new RefreshTokenResponse(accessToken, refreshToken);
        return Result.Ok(response, AuthMessages.UserAuthenticated);
    }
}
