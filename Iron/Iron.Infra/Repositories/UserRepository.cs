using Iron.Domain.Entities;
using Iron.Domain.Repositories;
using Iron.Domain.ValueObjects;
using Iron.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Iron.Infra.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistsByEmailAsync(Email email)
    {
        return await DbSet.AnyAsync(u => u.Email == email);
    }
}
