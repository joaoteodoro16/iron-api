using FluentValidation;
using FluentValidation.Results;
using Iron.Aplication.DTOs;
using Iron.Aplication.Usecases.Auth;
using Iron.Domain.Common;
using Iron.Domain.Entities;
using Iron.Domain.Repositories;
using Iron.Aplication.Services;
using Iron.Domain.ValueObjects;
using NSubstitute;

namespace Iron.Aplication.Tests.Usecases.Auth;

public class CreateUserUsecaseTests
{
    //Os mocks precisam ser CAMPOS: o construtor monta o _sut com eles,
    //mas cada teste precisa alcançá-los pra configurar stubs e verificar chamadas.
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
    private readonly IValidator<CreateUserRequest> _validator = Substitute.For<IValidator<CreateUserRequest>>();

    private readonly CreateUserUsecase _sut;

    public CreateUserUsecaseTests()
    {
        _sut = new CreateUserUsecase(_userRepository, _unitOfWork, _passwordHasher, _validator);
    }

    private static CreateUserRequest ValidRequest() =>
        new("Joao", "Teodoro", "joao@gmail.com", "password123", "14996365254");

    [Fact]
    public async Task ExecuteAsync_GivenValidRequest_ShouldCreateUserAndReturnOk()
    {
        var request = ValidRequest();
        _validator.ValidateAsync(request).Returns(new ValidationResult());
        _userRepository.ExistsByEmailAsync(Arg.Any<Email>()).Returns(false);
        _passwordHasher.Hash(request.Password).Returns("hashed-password");

        var result = await _sut.ExecuteAsync(request);

        Assert.True(result.IsSuccess);

        await _userRepository.Received(1).AddAsync(Arg.Is<User>(u =>
            u.FirstName == "Joao" &&
            u.LastName == "Teodoro" &&
            u.Email.Value == "joao@gmail.com" &&
            u.PhoneNumber.Value == "14996365254" &&
            u.PasswordHash == "hashed-password"));

        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task ExecuteAsync_GivenInvalidRequest_ShouldReturnValidationFailure()
    {
        // Arrange
        var request = ValidRequest();
        var failures = new List<ValidationFailure> { new("Email", "Email é obrigatório") };
        _validator.ValidateAsync(request).Returns(new ValidationResult(failures));

        // Act
        var result = await _sut.ExecuteAsync(request);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.ErrorType);

        // Assert 
        await _userRepository.DidNotReceive().AddAsync(Arg.Any<User>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task ExecuteAsync_GivenInvalidEmail_ShouldReturnValidationFailure()
    {
        var request = ValidRequest() with { Email = "email-invalido" };
        _validator.ValidateAsync(request).Returns(new ValidationResult());

        var result = await _sut.ExecuteAsync(request);
        Assert.True(result.IsFailure);
        await _userRepository.DidNotReceive().ExistsByEmailAsync(Arg.Any<Email>());
        await _userRepository.DidNotReceive().AddAsync(Arg.Any<User>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task ExecuteAsync_GivenInvalidPhoneNumber_ShouldReturnValidationFailure()
    {
        var request = ValidRequest() with { PhoneNumber = "invalid-phone-number" };
        _validator.ValidateAsync(request).Returns(new ValidationResult());

        var result = await _sut.ExecuteAsync(request);

        Assert.True(result.IsFailure);
        await _userRepository.DidNotReceive().AddAsync(Arg.Any<User>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task ExecuteAsync_GivenEmailAlreadyUse_ShouldConflictFailure()
    {
        var request = ValidRequest();
        _validator.ValidateAsync(request).Returns(new ValidationResult());
        _userRepository.ExistsByEmailAsync(Arg.Any<Email>()).Returns(true);

        var result = await _sut.ExecuteAsync(request);
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Conflict, result.ErrorType);
        await _userRepository.Received(1).ExistsByEmailAsync(Arg.Any<Email>());
        await _userRepository.DidNotReceive().AddAsync(Arg.Any<User>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserCreationFails_ShouldReturnFailure()
    {
        var request = ValidRequest() with { FirstName = "" };
        _validator.ValidateAsync(request).Returns(new ValidationResult());
        _userRepository.ExistsByEmailAsync(Arg.Any<Email>()).Returns(false);
        _passwordHasher.Hash(request.Password).Returns("password-hashed");

        var result = await _sut.ExecuteAsync(request);
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.ErrorType);

        await _userRepository.DidNotReceive().AddAsync(Arg.Any<User>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync();
    }
}
