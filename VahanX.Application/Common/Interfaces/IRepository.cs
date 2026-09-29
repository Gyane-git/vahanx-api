using System.Linq.Expressions;
using VahanX.Domain.Common;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Generic repository interface for data access.
/// </summary>
public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<T> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default);
    Task<IQueryable<T>> QueryAsync(bool asNoTracking = true, CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
}
