namespace Iron.Aplication.DTOs.Auth.Response;

public record RefreshTokenResponse(
    string AccessToken,
    string RefreshToken
);
