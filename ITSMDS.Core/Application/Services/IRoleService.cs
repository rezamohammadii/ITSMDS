

using ITSMDS.Core.Application.DTOs;

namespace ITSMDS.Core.Application.Services;

public interface IRoleService
{
    Task<PageResultDto<RoleDto>> GetAllRoleAsync(int pageNumber, int pageSize, string searchTrem , CancellationToken ct);

    Task<bool> CreateRoleAsync(RoleDtoIn roleInput, CancellationToken ct);
}
