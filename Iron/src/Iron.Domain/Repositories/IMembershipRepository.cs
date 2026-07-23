using Iron.Domain.Entities;

namespace Iron.Domain.Repositories;

public interface IMembershipRepository : IRepository<Membership>
{
    Task<ICollection<Membership>> GetByUserId(long UserId);
}
