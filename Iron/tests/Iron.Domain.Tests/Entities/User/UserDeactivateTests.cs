namespace Iron.Domain.Tests.Entities.User;

public class UserDeactivateTests
{
    [Fact]
    public void Deactivate_WhenCalled_ShouldUpdateIsActivePropertyForFalseValue()
    {
        var user = UserTestData.GetValidUser();
        user.Deactivate();
        Assert.False(user.IsActive);
        Assert.NotNull(user.UpdatedAt);
    }
}
