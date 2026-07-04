using Iron.Domain.ValueObjects;

namespace Iron.Domain.Tests.Entities.User;

public class UserChangeEmailTests
{
    [Fact]
    public void ChangeEmail_GivenValidEmail_ShouldUpdateEmailProperty()
    {
        var user = UserTestData.GetValidUser();
        var updatedEmail = Email.Create("joao.teodoro@gmail.com");
        user.ChangeEmail(updatedEmail);

        Assert.Equal(updatedEmail.Value, user.Email.Value);
        Assert.NotNull(user.UpdatedAt);
    }

    [Fact]
    public void ChangeEmail_GivenNullEmail_ShouldThrowArgumentNullException()
    {
        var user = UserTestData.GetValidUser();
        Email updatedEmail = null!;

        Assert.Throws<ArgumentNullException>(() => user.ChangeEmail(updatedEmail));
    }
}
