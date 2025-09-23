using ITSMDS.Domain.DTOs;

namespace ITSMDS.Application.Services;

public interface IRoleService
{
    Task<PageResultDto<RoleDto>> GetAllRoleAsync(int pageNumber, int pageSize, string searchTrem, CancellationToken ct);

    Task<(bool, string)> CreateRoleAsync(RoleDtoIn roleInput, CancellationToken ct);
    Task<(bool, string)> DeleteRoleAsync(int roleId, CancellationToken ct);
    Task<bool> AssignRoleToUserAsync(string personalCode , int roleId, CancellationToken ct);
}
