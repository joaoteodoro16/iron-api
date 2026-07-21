namespace Iron.Aplication.DTOs.Auth.Response;

public record AuthUserResponse(
    long Id,
    string FirstName,
    string LastName,
    string Email,
    bool EmailConfirmed,
    bool IsPlatformAdmin,
    string AccessToken
);
