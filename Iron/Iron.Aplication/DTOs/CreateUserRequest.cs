namespace Iron.Aplication.DTOs;

public record CreateUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string PhoneNumber,
    bool isPlatformAdmin = false
);
