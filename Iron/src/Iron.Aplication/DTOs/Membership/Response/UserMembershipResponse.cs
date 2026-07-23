namespace Iron.Aplication.DTOs.Membership.Response;

public record UserMembershipResponse(
    long GymId,
    string GymName,
    string RoleName
);
