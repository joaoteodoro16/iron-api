namespace Iron.Aplication.DTOs.Auth.Request;

public record AuthUserRequest(
    string Email,
    string Password
);
