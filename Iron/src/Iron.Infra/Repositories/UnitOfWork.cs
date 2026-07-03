using Iron.Domain.Repositories;
using Iron.Infrastructure.Persistence.Context;

namespace Iron.Infra.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
