using Iron.Domain.Entities;
using Iron.Domain.ValueObjects;

namespace Iron.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<bool> ExistsByEmailAsync(Email email);
}
