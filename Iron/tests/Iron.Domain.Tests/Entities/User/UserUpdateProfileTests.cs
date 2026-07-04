using Iron.Domain.ValueObjects;

namespace Iron.Domain.Tests.Entities.User;

public class UserUpdateProfileTests
{
    [Fact]
    public void UpdateProfile_GivenValidData_ShouldUpdateProperties()
    {
        var user = UserTestData.GetValidUser();

        var updatedData = GetUpdatedProfileData();

        user.UpdateProfile(updatedData.UpdatedFirstName, updatedData.UpdatedLastName, phoneNumber: updatedData.UpdatedPhoneNumber);

        Assert.Equal(updatedData.UpdatedFirstName, user.FirstName);
        Assert.Equal(updatedData.UpdatedLastName, user.LastName);
        Assert.Equal(updatedData.UpdatedPhoneNumber.Value, user.PhoneNumber.Value);
        Assert.NotNull(user.UpdatedAt);
    }

    [Fact]
    public void UpdateProfile_GivenEmptyFirstName_ShouldThrowArgumentException()
    {
        var user = UserTestData.GetValidUser();

        var updatedData = GetUpdatedProfileData();
        updatedData.UpdatedFirstName = string.Empty;

        Assert.Throws<ArgumentException>(() => user.UpdateProfile(updatedData.UpdatedFirstName, updatedData.UpdatedLastName, updatedData.UpdatedPhoneNumber));
    }

    [Fact]
    public void UpdateProfile_GivenEmptyLastName_ShouldThrowArgumentException()
    {
        var user = UserTestData.GetValidUser();

        var updatedData = GetUpdatedProfileData();
        updatedData.UpdatedLastName = string.Empty;

        Assert.Throws<ArgumentException>(() => user.UpdateProfile(updatedData.UpdatedFirstName, updatedData.UpdatedLastName, updatedData.UpdatedPhoneNumber));
    }

    [Fact]
    public void UpdateProfile_GivenNullPhoneNumber_ShouldThrowArgumentNullException()
    {
        var user = UserTestData.GetValidUser();

        var updatedData = GetUpdatedProfileData();
        updatedData.UpdatedPhoneNumber = null!;

        Assert.Throws<ArgumentNullException>(() => user.UpdateProfile(updatedData.UpdatedFirstName, updatedData.UpdatedLastName, updatedData.UpdatedPhoneNumber));
    }

    private static (string UpdatedFirstName, string UpdatedLastName, PhoneNumber UpdatedPhoneNumber) GetUpdatedProfileData()
    {
        return ("Marcio", "Silva", PhoneNumber.Create("14999999999"));
    }
}
