using Iron.Domain.Entities;
using Iron.Domain.Repositories;
using Iron.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Iron.Infra.Repositories;

public abstract class RepositoryBase<TEntity> : IRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly ApplicationDbContext Context;

    protected DbSet<TEntity> DbSet => Context.Set<TEntity>();

    protected RepositoryBase(ApplicationDbContext context)
    {
        Context = context;
    }

    public virtual async Task<TEntity?> GetByIdAsync(long id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
    }

    public virtual void Update(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public virtual void Remove(TEntity entity)
    {
        DbSet.Remove(entity);
    }
}
