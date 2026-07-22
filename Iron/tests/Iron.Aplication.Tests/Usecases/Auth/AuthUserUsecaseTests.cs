using FluentValidation;
using FluentValidation.Results;
using Iron.Aplication.DTOs.Auth.Request;
using Iron.Aplication.Services;
using Iron.Aplication.Usecases.Auth;
using Iron.Domain.Entities;
using Iron.Domain.Repositories;
using Iron.Domain.ValueObjects;
using NSubstitute;

namespace Iron.Aplication.Tests.Usecases.Auth;

public class AuthUserUsecaseTests
{
    private readonly IValidator<AuthUserRequest> _validator = Substitute.For<IValidator<AuthUserRequest>>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
    private readonly ITokenService _tokenService = Substitute.For<ITokenService>();
    private readonly IRefreshTokenRepository _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private readonly AuthUserUsecase _sut;

    public AuthUserUsecaseTests()
    {
        _sut = new AuthUserUsecase(_tokenService, _userRepository, _validator, _passwordHasher, _refreshTokenRepository, _unitOfWork);
    }

    private static AuthUserRequest ValidRequest() =>
        new("joao@gmail.com", "12345678");

    //No login o User vem DO BANCO, então ele já tem Id. Como o Id é atribuído pelo EF
    //(setter protegido), o teste simula isso via reflection — senão o Id fica 0 e o
    //RefreshToken.Create rejeita (ele exige userId > 0).
    private static User ValidUser(long id = 1)
    {
        var user = User.Create(
            "Joao", "Teodoro",
            Email.Create("joao@gmail.com"),
            "hashed-password",
            PhoneNumber.Create("14996365254"));

        typeof(BaseEntity).GetProperty(nameof(BaseEntity.Id))!.SetValue(user, id);

        return user;
    }

    [Fact]
    public async Task ExecuteAsync_GivenValidCredentials_ShouldAuthenticateUser()
    {
        var request = ValidRequest();
        var user = ValidUser();
        var refreshExpiresAt = DateTime.UtcNow.AddDays(5);

        _validator.ValidateAsync(request).Returns(new ValidationResult());
        _userRepository.GetByEmailAsync(Arg.Any<Email>()).Returns(user);
        _passwordHasher.Verify(request.Password, user.PasswordHash).Returns(true);
        _tokenService.GenerateToken(user).Returns("access-token");
        _tokenService.GenerateRefreshToken().Returns(("refresh-token", refreshExpiresAt));

        //Act
        var result = await _sut.ExecuteAsync(request);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("access-token", result.Value.AccessToken);
        Assert.Equal("refresh-token", result.Value.refreshToken);
        Assert.Equal(user.Email.Value, result.Value.Email);

        await _refreshTokenRepository.Received(1).AddAsync(Arg.Is<RefreshToken>(rt =>
            rt.UserId == user.Id &&
            rt.Token == "refresh-token" &&
            rt.ExpiresAt == refreshExpiresAt));

        await _unitOfWork.Received(1).SaveChangesAsync();
    }
}
