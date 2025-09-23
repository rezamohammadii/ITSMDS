using ITSMDS.Domain.DTOs;
using ITSMDS.Domain.Entities;

namespace ITSMDS.Application.Abstractions;

public interface IRoleRepository
{
    IQueryable<Role> GetRoleQueryAsync(CancellationToken ct = default);

    ValueTask CreateRoleAsync(Role role, CancellationToken ct);

    ValueTask<Role> GetRoleByRoleIdAsync(int roleId  , CancellationToken ct = default);

    ValueTask<bool> CheckRoleNameExsistAsync(string roleName, CancellationToken ct = default);
}
