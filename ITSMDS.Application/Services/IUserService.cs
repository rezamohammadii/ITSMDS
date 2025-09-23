using ITSMDS.Domain.DTOs;

namespace ITSMDS.Application.Services;

public interface IUserService
{
    #region User
    Task<(bool, string)> CreateAsync(CreateUserRequest request, CancellationToken ct = default);
    Task<UserResponse> GetUserByPersonalCodeAsync(int pCode, CancellationToken ct = default);
    Task<List<UserResponse>> GetAllAsync(CancellationToken ct = default);
    Task<UserResponse> UpdateAsync(UpdateUserRequest request, CancellationToken ct = default);
    Task<bool> DeleteUserAsync(int pCode, CancellationToken ct = default);

    Task<UserResponse> GetUserByUserNameAsync(string username, CancellationToken ct = default);
    #endregion

    #region Permission

    Task<List<PermissionDto>> GetPermissionListAsync(CancellationToken ct = default);
    #endregion


    #region Auth
    Task<LoginResponseDTO> LoginAsync(int personalCode, string? username, string pass, CancellationToken ct = default);
    #endregion
}
