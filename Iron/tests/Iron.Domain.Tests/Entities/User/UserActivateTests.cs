namespace Iron.Domain.Tests.Entities.User;

public class UserActivateTests
{
    [Fact]
    public void Activate_WhenCalled_ShouldUpdateIsActivePropertyForTrueValue()
    {
        var user = UserTestData.GetValidUser();
        user.Activate();
        Assert.True(user.IsActive);
        Assert.NotNull(user.UpdatedAt);
    }
}
