namespace Iron.Domain.Tests.Entities.User;

public class UserGrantPlatformAdminTests
{
    [Fact]
    public void GrantPlatformAdmin_WhenCalled_ShouldUpdateIsPlatformAdminWithTrueValue()
    {
        var user = UserTestData.GetValidUser();
        user.GrantPlatformAdmin();
        Assert.True(user.IsPlatformAdmin);
        Assert.NotNull(user.UpdatedAt);
    }
}
