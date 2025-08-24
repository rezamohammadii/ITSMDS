

using ITSMDS.Core.Application.Abstractions;
using ITSMDS.Core.Application.DTOs;
using ITSMDS.Core.Application.Services;
using ITSMDS.Domain.Entities;

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
        if (await _repo.CheckUserExsitAsync(request.userName, request.personalCode, ct))
            throw new InvalidOperationException("Email already exsist");

        var user = new User(null, null, null, request.personalCode ?? 0, null, request
            .userName ?? "", request.password, null);
        await _repo.AddUserAsync(user, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return new UserResponse(user.Id, user.Email, user.FirstName, user.LastName, user.CreateDate, user.PhoneNumber, user.PersonalCode);
       
    }

    public Task<List<UserResponse>> GetAllAsync(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
