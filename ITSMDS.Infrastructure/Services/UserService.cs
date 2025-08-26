

using ITSMDS.Core.Application.Abstractions;
using ITSMDS.Core.Application.DTOs;
using ITSMDS.Core.Application.Services;
using ITSMDS.Core.Tools;
using ITSMDS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ITSMDS.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserService> _logger;
    public UserService(IUnitOfWork unitOfWork, IUserRepository repo, ILogger<UserService> logger)
    {
        _unitOfWork = unitOfWork;
        _repo = repo;
        _logger = logger;
    }
    public async Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken ct = default)
    {
        if (await _repo.CheckUserExsitAsync(request.userName, int.Parse(request.personalCode), ct))
            throw new InvalidOperationException("Email already exsist");

        string hashPass = HashGenerator.HashPassword(request.password);
        var user = new User(null, null, null, int.Parse(request.personalCode), 0, request
            .userName ?? "", hashPass, null, "");
        await _repo.AddUserAsync(user, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return new UserResponse(user.HashId, user.Email, user.FirstName, user.LastName, user.CreateDate, user.PhoneNumber, user.IpAddress, user.UserName,  user.PersonalCode);
       
    }

    public async Task<List<UserResponse>> GetAllAsync(CancellationToken ct)
    {
        try
        {
            var items = await _repo.GetAllUsersAsync(ct);
            return items.Select(x => new UserResponse(x.HashId, x.Email, x.FirstName, x.LastName, x.CreateDate,
            x.PhoneNumber, x.IpAddress, x.UserName, x.PersonalCode)).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError("Database Error {MSG}", ex.Message);
            throw new InvalidOperationException(ex.Message);
        }
       
    }
}
