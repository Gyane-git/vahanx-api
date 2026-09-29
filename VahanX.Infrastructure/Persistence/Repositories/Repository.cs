using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VahanX.Application.Common.Interfaces;
using VahanX.Domain.Common;
using VahanX.Infrastructure.Persistence;

namespace VahanX.Infrastructure.Persistence.Repositories;

/// <summary>
/// Generic repository implementation.
/// </summary>
public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly VahanXDbContext Context;
    protected readonly DbSet<T> DbSet;

    public Repository(VahanXDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<T> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking().AsQueryable();

        if (filter != null)
            query = query.Where(filter);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<IQueryable<T>> QueryAsync(bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsQueryable();
        if (asNoTracking)
            query = query.AsNoTracking();
        return await Task.FromResult(query);
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        Context.Entry(entity).State = EntityState.Modified;
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbSet.Remove(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(predicate, cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsQueryable();
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }
}
