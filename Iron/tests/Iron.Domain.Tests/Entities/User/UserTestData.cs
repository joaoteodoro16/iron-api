using Iron.Domain.ValueObjects;

namespace Iron.Domain.Tests.Entities.User;

internal static class UserTestData
{
    internal static (
        string FirstName,
        string LastName,
        Email Email,
        string PasswordHash,
        PhoneNumber PhoneNumber,
        bool IsPlatformAdmin,
        bool EmailConfirmed) GetValidParameters()
    {
        return (
            "João",
            "Teodoro",
            Email.Create("joao@gmail.com"),
            "123123",
            PhoneNumber.Create("14996431278"),
            false,
            false
        );
    }

    internal static Iron.Domain.Entities.User GetValidUser()
    {
        var userData = GetValidParameters();
        return Iron.Domain.Entities.User.Create(userData.FirstName, userData.LastName, userData.Email, userData.PasswordHash, userData.PhoneNumber, userData.IsPlatformAdmin);
    }
}
