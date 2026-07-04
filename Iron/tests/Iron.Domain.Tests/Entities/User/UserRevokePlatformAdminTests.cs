namespace Iron.Domain.Tests.Entities.User;

public class UserRevokePlatformAdminTests
{
    [Fact]
    public void RevokePlatformAdmin_WhenCalled_ShouldUpdateIsPlatformAdminWithFalseValue()
    {
        var user = UserTestData.GetValidUser();
        user.RevokePlatformAdmin();
        Assert.False(user.IsPlatformAdmin);
        Assert.NotNull(user.UpdatedAt);
    }
}
