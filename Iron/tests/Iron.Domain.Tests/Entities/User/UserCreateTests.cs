using AwesomeAssertions;
using Xunit.Abstractions;

namespace Iron.Domain.Tests.Entities.User;

public class UserCreateTests(ITestOutputHelper testOutputHelper)
{
    //for when I can have more than one option for the same test
    // [Theory]
    // [InlineData("Carro1")]
    // [InlineData("Carro2")]
    // public void Create_GivenAllParameters_ThenShouldSetThePropertiesCorrectly(string expectedCarName);

    //For generante randon values, use Bogus/Faker
    //Example: pivate readonly Faker  _faker = new("pt_BR);
    //var expectedCarName = _fakes.Vehicle.Model();

    [Fact]
    public void Create_GivenValidParameters_ShouldCreateUser()
    {
        //Given all parameters
        var expectedData = UserTestData.GetValidParameters();

        // Act
        var user = Domain.Entities.User.Create(expectedData.FirstName, expectedData.LastName, expectedData.Email, expectedData.PasswordHash, expectedData.PhoneNumber, expectedData.IsPlatformAdmin);

        // Assert
        Assert.Equal(expectedData.FirstName, user.FirstName);
        Assert.Equal(expectedData.LastName, user.LastName);
        Assert.Equal(expectedData.Email.Value, user.Email.Value);
        Assert.Equal(expectedData.PasswordHash, user.PasswordHash);
        Assert.Equal(expectedData.PhoneNumber.Value, user.PhoneNumber.Value);
        Assert.Equal(expectedData.IsPlatformAdmin, user.IsPlatformAdmin);
        Assert.True(user.IsActive);
        Assert.Null(user.LastLoginAt);
        Assert.Empty(user.Memberships);
        Assert.Empty(user.RefreshTokens);
        Assert.False(user.EmailConfirmed);
    }

    [Fact]
    [Trait("Approach", "FluentAssertions")]
    public void FluentAssertionsCreate_GivenValidParameters_ShouldCreateUser()
    {
        //Given all parameters
        var expectedData = UserTestData.GetValidParameters();

        // Act
        var user = Domain.Entities.User.Create(expectedData.FirstName, expectedData.LastName, expectedData.Email, expectedData.PasswordHash, expectedData.PhoneNumber, expectedData.IsPlatformAdmin);

        // Assert
        user.FirstName.Should().Be(expectedData.FirstName, "Should be equal");
        user.LastName.Should().Be(expectedData.LastName);
        user.Email.Value.Should().Be(user.Email.Value);
        user.PasswordHash.Should().Be(expectedData.PasswordHash);
        user.PhoneNumber.Should().Be(expectedData.PhoneNumber);
        user.IsPlatformAdmin.Should().BeFalse();
        user.IsActive.Should().BeTrue();
        user.LastLoginAt.Should().BeNull("Should be Null");
        user.Memberships.Should().BeEmpty();
        user.RefreshTokens.Should().BeEmpty();
        user.EmailConfirmed.Should().BeFalse();
    }

    [Fact]
    public void Create_GivenEmptyFirstName_ShouldThrowArgumentException()
    {
        var expectedData = UserTestData.GetValidParameters();
        expectedData.FirstName = string.Empty;
        testOutputHelper.WriteLine(expectedData.FirstName);

        Assert.Throws<ArgumentException>(() => Domain.Entities.User.Create(expectedData.FirstName, expectedData.LastName, expectedData.Email,
        expectedData.PasswordHash, expectedData.PhoneNumber, expectedData.IsPlatformAdmin));
    }

    [Fact]
    public void Create_GivenEmptyLastName_ShouldThrowArgumentException()
    {
        var expectedData = UserTestData.GetValidParameters();
        expectedData.LastName = string.Empty;

        Assert.Throws<ArgumentException>(() => Domain.Entities.User.Create(expectedData.FirstName, expectedData.LastName, expectedData.Email,
        expectedData.PasswordHash, expectedData.PhoneNumber, expectedData.IsPlatformAdmin));
    }

    [Fact]
    public void Create_GivenEmptyPasswordHash_ShouldThrowArgumentException()
    {
        var expectedData = UserTestData.GetValidParameters();
        expectedData.PasswordHash = string.Empty;

        Assert.Throws<ArgumentException>(() => Domain.Entities.User.Create(expectedData.FirstName, expectedData.LastName, expectedData.Email,
        expectedData.PasswordHash, expectedData.PhoneNumber, expectedData.IsPlatformAdmin));
    }

    [Fact]
    public void Create_GivenNullEmail_ShouldThrowArgumentNullException()
    {
        var expectedData = UserTestData.GetValidParameters();
        expectedData.Email = null!;

        Assert.Throws<ArgumentNullException>(() => Domain.Entities.User.Create(expectedData.FirstName, expectedData.LastName, expectedData.Email,
        expectedData.PasswordHash, expectedData.PhoneNumber, expectedData.IsPlatformAdmin));
    }

    [Fact]
    public void Create_GivenNullPhoneNumber_ShouldThrowArgumentNullException()
    {
        var expectedData = UserTestData.GetValidParameters();
        expectedData.PhoneNumber = null!;

        Assert.Throws<ArgumentNullException>(() => Domain.Entities.User.Create(expectedData.FirstName, expectedData.LastName, expectedData.Email,
        expectedData.PasswordHash, expectedData.PhoneNumber, expectedData.IsPlatformAdmin));
    }
}
