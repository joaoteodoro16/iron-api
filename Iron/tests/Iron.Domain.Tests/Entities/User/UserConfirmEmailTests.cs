namespace Iron.Domain.Tests.Entities.User;

public class UserConfirmEmailTests
{
    [Fact]
    public void ConfirmEmail_WhenCalled_ShouldConfirmEmail()
    {
        var user = UserTestData.GetValidUser();
        user.ConfirmEmail();
        Assert.True(user.EmailConfirmed);
        Assert.NotNull(user.UpdatedAt);
    }

    [Fact]
    public void ChangePassword_GivenValidPasswordHash_ShouldUpdatePasswordHashProperty()
    {
        var user = UserTestData.GetValidUser();
        var updatedPasswordHash = "1234567";
        user.ChangePassword(updatedPasswordHash);
        Assert.Equal(updatedPasswordHash, user.PasswordHash);
        Assert.NotNull(user.UpdatedAt);
    }
}
