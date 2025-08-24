

using ITSMDS.Core.Application.Abstractions;
using ITSMDS.Core.Application.DTOs;
using ITSMDS.Core.Application.Services;

namespace ITSMDS.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    private readonly IUnitOfWork _unitOfWork;
    public UserService(IUnitOfWork unitOfWork, IUserRepository repo)
    {
        _unitOfWork = unitOfWork;
        _repo = repo;
    }
    public async Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken ct = default)
    {
        if (await _re)
        {
            
        }
    }

    public Task<List<UserResponse>> GetAllAsync(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
