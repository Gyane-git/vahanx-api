using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Admin;
using VahanX.Domain.Entities;

namespace VahanX.Application.Services;

public interface IPermissionManagementService
{
    Task<List<PermissionDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PermissionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}

/// <summary>
/// Permissions are system-defined and immutable; the API exposes read-only listing.
/// </summary>
public class PermissionManagementService : IPermissionManagementService
{
    private readonly IRepository<Permission> _permissions;

    public PermissionManagementService(IRepository<Permission> permissions)
    {
        _permissions = permissions;
    }

    public async Task<List<PermissionDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var query = await _permissions.QueryAsync(true, cancellationToken);
        return await query
            .OrderBy(p => p.Module).ThenBy(p => p.Name)
            .Select(p => new PermissionDto { Id = p.Id, Name = p.Name, Description = p.Description, Module = p.Module, IsSystemDefined = p.IsSystemDefined })
            .ToListAsync(cancellationToken);
    }

    public async Task<PermissionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var p = await _permissions.GetByIdAsync(id, cancellationToken);
        return p is null ? null : new PermissionDto { Id = p.Id, Name = p.Name, Description = p.Description, Module = p.Module, IsSystemDefined = p.IsSystemDefined };
    }
}
