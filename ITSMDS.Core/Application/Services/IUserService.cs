

using ITSMDS.Core.Application.DTOs;

namespace ITSMDS.Core.Application.Services;

public interface IUserService
{
    Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken ct = default);
    Task<List<UserResponse>> GetAllAsync(CancellationToken ct = default);
}
