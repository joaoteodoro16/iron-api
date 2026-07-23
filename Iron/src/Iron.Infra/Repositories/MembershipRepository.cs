using Iron.Domain.Entities;
using Iron.Domain.Repositories;
using Iron.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Iron.Infra.Repositories;

public class MembershipRepository : RepositoryBase<Membership>, IMembershipRepository
{
    public MembershipRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<ICollection<Membership>> GetByUserId(long userId)
    {
        return await DbSet
        .Where(m => m.UserId == userId && m.IsActive)
        .Include(m => m.Gym)
        .Include(m => m.Role)
        .ToListAsync();
    }


}
