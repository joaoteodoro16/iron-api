namespace Iron.Domain.Tests.Entities.User;

public class UserRecordLoginTests
{
    [Fact]
    public void RecordLogin_WhenCalled_ShouldUpdateLastLoginAt()
    {
        var user = UserTestData.GetValidUser();
        user.RecordLogin();
        Assert.NotNull(user.LastLoginAt);
    }
}
