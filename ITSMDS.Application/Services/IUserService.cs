using ITSMDS.Domain.DTOs;

namespace ITSMDS.Application.Services;

public interface IUserService
{
    #region User
    Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken ct = default);
    Task<UserResponse> GetUserAsync(int pCode, CancellationToken ct = default);
    Task<List<UserResponse>> GetAllAsync(CancellationToken ct = default);
    Task<UserResponse> UpdateAsync(UpdateUserRequest request, CancellationToken ct = default);
    Task<bool> DeleteUserAsync(int pCode, CancellationToken ct = default);
    #endregion

    #region Permission

    Task<List<PermissionDto>> GetPermissionListAsync(CancellationToken ct = default);
    #endregion

}
