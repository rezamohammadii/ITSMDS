

using AutoMapper;
using Azure.Core;
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
    private readonly IMapper _mapper;
    public UserService(IUnitOfWork unitOfWork, IUserRepository repo, ILogger<UserService> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _repo = repo;
        _logger = logger;
        _mapper = mapper;
    }

    #region UserMethod
    public async Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken ct = default)
    {
        if (await _repo.CheckUserExsitAsync(request.userName, int.Parse(request.personalCode), ct))
            throw new InvalidOperationException("Email already exsist");

        string hashPass = HashGenerator.HashPassword(request.password);
        var user = new User(request.firstName, request.lastName, request.email, int.Parse(request.personalCode),
            request.phoneNumber!, request
            .userName ?? "", hashPass, "", request.ipAddress);
        await _repo.AddUserAsync(user, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return new UserResponse(user.HashId, user.Email, user.FirstName, user.LastName, ConvertDate.ConvertToShamsi(user.CreateDate), user.PhoneNumber, user.IpAddress, user.UserName, user.PersonalCode);

    }

    public async Task<bool> DeleteUserAsync(int pCode, CancellationToken ct = default)
    {
        try
        {
            var user = await _repo.GetUserByPersonalCodeAsync(pCode, ct);
            if (user == null)
            {
                return false;
            }
            user.MarkAsDeleted();
            await _unitOfWork.SaveChangesAsync(ct);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database Error {MSG}", ex.Message);
            throw new InvalidOperationException(ex.Message);
        }
    }

    public async Task<List<UserResponse>> GetAllAsync(CancellationToken ct)
    {
        try
        {
            var items = await _repo.GetAllUsersAsync(ct);
            return items.Select(x => new UserResponse(x.HashId, x.Email, x.FirstName, x.LastName,
                ConvertDate.ConvertToShamsi(x.CreateDate),
            x.PhoneNumber, x.IpAddress, x.UserName, x.PersonalCode)).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError("Database Error {MSG}", ex.Message);
            throw new InvalidOperationException(ex.Message);
        }

    }


    public async Task<UserResponse> GetUserAsync(int pCode, CancellationToken ct = default)
    {
        try
        {
            var user = await _repo.GetUserByPersonalCodeAsync(pCode, ct);

            return _mapper.Map<UserResponse>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database Error {MSG}", ex.Message);
            throw new InvalidOperationException(ex.Message);
        }
    }

    public async Task<UserResponse> UpdateAsync(UpdateUserRequest request, CancellationToken ct = default)
    {
        try
        {
            var user = await _repo.UpdateUserAsync(_mapper.Map<User>(request), ct);
            await _unitOfWork.SaveChangesAsync(ct);
            var userMap = _mapper.Map<UserResponse>(user);
            return userMap;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database Error {MSG}", ex.Message);
            throw new InvalidOperationException(ex.Message);
        }

    }
    #endregion

    #region PermissionMethod

    public async Task<List<PermissionDto>> GetPermissionList(CancellationToken ct = default)
    {
        var listPermissionDb = await _repo.GetPermissionListAsync(ct);
        var resultList = new List<PermissionDto>();
         listPermissionDb
    }
    #endregion

}
