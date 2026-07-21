using Iron.Domain.Entities;
using Iron.Domain.Repositories;
using Iron.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Iron.Infra.Repositories;

public class RefreshTokenRepository : RepositoryBase<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await DbSet.FirstOrDefaultAsync(u => u.Token == token);
    }

}
