using Iron.Domain.Entities;
using Iron.Domain.Repositories;
using Iron.Domain.ValueObjects;
using Iron.Infra.Repositories;
using Iron.Infrastructure.Persistence.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
namespace Iron.Infra.Tests.Repositories;

[Trait("Category", "Integration")]
public class UserRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly IUserRepository _userRepository;
    private readonly ApplicationDbContext _context;
    private readonly User _validUser;

    public UserRepositoryTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new ApplicationDbContext(options);
        _context.Database.EnsureCreated();
        _userRepository = new UserRepository(_context);

        _validUser = User.Create(
            "João", "Silva",
            Email.Create("joao@gmail.com"),
            "hash",
            PhoneNumber.Create("14996431278"));
    }

    [Fact]
    public async Task AddAsync_GivenValidUser_ShouldPersistUser()
    {
        //Arrange
        var user = _validUser;

        //Act
        await _userRepository.AddAsync(user);
        await _context.SaveChangesAsync();

        var found = await _userRepository.GetByIdAsync(user.Id);
        Assert.NotNull(found);
        Assert.Equal("joao@gmail.com", found.Email.Value);
    }

    [Fact]
    public async Task AddAsync_GivenValidUser_ShouldPersistValueObjectsCorrectly()
    {
        // Arrange
        var user = _validUser;

        // Act
        await _userRepository.AddAsync(user);
        await _context.SaveChangesAsync();

        //Envazia o user da memoria, do cache do EF
        //Isso faz a gente ter uma ideia se os VO foram persistidos e convertidos corretamente
        _context.ChangeTracker.Clear();

        var found = await _userRepository.GetByIdAsync(user.Id);

        // Assert
        Assert.NotNull(found);
        Assert.Equal("joao@gmail.com", found.Email.Value);
        Assert.Equal("14996431278", found.PhoneNumber.Value);
    }

    [Fact]
    public async Task AddAsync_GivenValidUser_ShouldGenerateId()
    {
        Assert.Equal(0L, _validUser.Id);
        await _userRepository.AddAsync(_validUser);
        await _context.SaveChangesAsync();
        Assert.NotEqual(0L, _validUser.Id);
    }

    [Fact]
    public async Task AddAsync_GivenDuplicateEmail_ShouldThrowDbUpdateException()
    {
        var user1 = User.Create("Joao", "Teodoro", Email.Create("joao@gmail.com"),
            "hash", PhoneNumber.Create("14995652365"));

        var user2 = User.Create("Teodoro", "Joao", Email.Create("joao@gmail.com"),
            "hash", PhoneNumber.Create("14996431278"));

        // Arrange: o primeiro usuário já está salvo
        await _userRepository.AddAsync(user1);
        await _context.SaveChangesAsync();

        // Act: adiciona o segundo com email duplicado
        await _userRepository.AddAsync(user2);

        // Assert: o índice único faz o save estourar
        await Assert.ThrowsAsync<DbUpdateException>(() => _context.SaveChangesAsync());
    }

    [Fact]
    public async Task GetByIdAsync_GivenExistId_ShouldReturnExistUser()
    {
        await _userRepository.AddAsync(_validUser);
        await _context.SaveChangesAsync();
        var expectedId = _validUser.Id;

        _context.ChangeTracker.Clear();
        var found = await _userRepository.GetByIdAsync(expectedId);

        Assert.NotNull(found);
        Assert.Equal(expectedId, found.Id);
        Assert.Equal(_validUser.FirstName, found.FirstName);
        Assert.Equal(_validUser.LastName, found.LastName);
        Assert.Equal(_validUser.Email.Value, found.Email.Value);
        Assert.Equal(_validUser.PhoneNumber.Value, found.PhoneNumber.Value);
    }

    [Fact]
    public async Task GetByIdAsync_GivenNonExistentId_ShouldReturnNull()
    {
        await _userRepository.AddAsync(_validUser);
        await _context.SaveChangesAsync();

        var found = await _userRepository.GetByIdAsync(_validUser.Id + 1L);

        Assert.Null(found);
    }

    [Fact]
    public async Task Update_GivenValidUser_ShouldPersistChanges()
    {
        await _userRepository.AddAsync(_validUser);
        await _context.SaveChangesAsync();
        var id = _validUser.Id;

        _validUser.ChangeEmail(Email.Create("joao2@gmail.com"));
        _validUser.UpdateProfile("Joao2", "Teodoro2", PhoneNumber.Create("14996563652"));
        _userRepository.Update(_validUser);
        await _context.SaveChangesAsync();

        _context.ChangeTracker.Clear();

        var found = await _userRepository.GetByIdAsync(id);
        Assert.NotNull(found);
        Assert.Equal("joao2@gmail.com", found.Email.Value);
        Assert.Equal("14996563652", found.PhoneNumber.Value);
        Assert.Equal("Joao2", found.FirstName);
        Assert.Equal("Teodoro2", found.LastName);
    }

    [Fact]
    public async Task Update_GivenDuplicateEmail_ShouldThrowDbUpdateException()
    {
        const string sharedEmail = "joao@gmail.com";

        var existingUser = User.Create("Joao", "Teodoro", Email.Create(sharedEmail),
            "hash", PhoneNumber.Create("14996431278"));
        var newUser = User.Create("Marcos", "Teodoro", Email.Create("marcos@gmail.com"),
            "hash", PhoneNumber.Create("14995636525"));

        await _userRepository.AddAsync(existingUser);
        await _userRepository.AddAsync(newUser);
        await _context.SaveChangesAsync();

        newUser.ChangeEmail(Email.Create(sharedEmail));
        _userRepository.Update(newUser);

        await Assert.ThrowsAsync<DbUpdateException>(() => _context.SaveChangesAsync());
    }

    [Fact]
    public async Task Remove_GivenExistingUser_ShouldRemoveUser()
    {
        await _userRepository.AddAsync(_validUser);
        await _context.SaveChangesAsync();
        var id = _validUser.Id;

        _userRepository.Remove(_validUser);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var deletedUser = await _userRepository.GetByIdAsync(id);
        Assert.Null(deletedUser);
    }

    [Fact]
    public async Task ExistsByEmailAsync_GivenExistingEmail_ShouldReturnTrue()
    {
        await _userRepository.AddAsync(_validUser);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();
        var email = _validUser.Email;

        var result = await _userRepository.ExistsByEmailAsync(email);
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsByEmailAsync_GivenNonExistentEmail_ShouldReturnFalse()
    {
        await _userRepository.AddAsync(_validUser);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var email = "joao2@gmail.com";

        var result = await _userRepository.ExistsByEmailAsync(Email.Create(email));
        Assert.False(result);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}
