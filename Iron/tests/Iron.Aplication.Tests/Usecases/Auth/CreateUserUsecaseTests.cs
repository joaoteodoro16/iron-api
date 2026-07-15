using FluentValidation;
using Iron.Aplication.DTOs;
using Iron.Aplication.Usecases.Auth;
using Iron.Domain.Repositories;
using Iron.Domain.Security;
using NSubstitute;

namespace Iron.Aplication.Tests.Usecases.Auth;

public class CreateUserUsecaseTests
{
    private readonly CreateUserUsecase _sut;

    public CreateUserUsecaseTests()
    {
        var userRepository = Substitute.For<IUserRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var passwordHasher = Substitute.For<IPasswordHasher>();
        var validator = Substitute.For<IValidator<CreateUserRequest>>();

        _sut = new CreateUserUsecase(userRepository, unitOfWork, passwordHasher, validator);
    }

    [Fact]
    public async Task ExecuteAsync_GivenCreateUserRequest_ShouldReturnOkResultAndCreateUser()
    {

    }
}
