

using ITSMDS.Core.Application.DTOs;
using ITSMDS.Domain.Entities;

namespace ITSMDS.Core.Application.Abstractions;

public interface IRoleRepository
{
    IQueryable<Role> GetListRoleAsync(CancellationToken ct =default);

    ValueTask CreateRoleAsync(Role role, CancellationToken ct);



}
