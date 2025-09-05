

using AutoMapper;
using Azure.Core;
using ITSMDS.Application.Abstractions;
using ITSMDS.Domain.DTOs;
using ITSMDS.Application.Services;
using ITSMDS.Domain.Tools;
using ITSMDS.Domain.Entities;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        try
        {
            _logger.LogInformation("CreateAsync called with username: {Username}, personalCode: {Code}", request.userName, request.personalCode);

            if (await _repo.CheckUserExsitAsync(request.userName, int.Parse(request.personalCode), ct))
            {
                _logger.LogWarning("User already exists: {Username}", request.userName);
                throw new InvalidOperationException("User with this email or personal code already exists.");
            }

            string hashPass = HashGenerator.HashPassword(request.password);
            var user = new User(request.firstName, request.lastName, request.email, int.Parse(request.personalCode),
                request.phoneNumber!, request.userName ?? "", hashPass, "", request.ipAddress);

            await _repo.AddUserAsync(user, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            _logger.LogInformation("User created successfully: {UserId}", user.HashId);

            return new UserResponse(user.HashId, user.Email, user.FirstName, user.LastName,
                ConvertDate.ConvertToShamsi(user.CreateDate), user.PhoneNumber, user.IpAddress, user.UserName, user.PersonalCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateAsync");
            throw;
        }
    }


    public async Task<bool> DeleteUserAsync(int pCode, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Attempting to delete user with personalCode: {Code}", pCode);
            var user = await _repo.GetUserByPersonalCodeAsync(pCode, ct);

            if (user == null)
            {
                _logger.LogWarning("User not found for deletion: {Code}", pCode);
                return false;
            }

            user.MarkAsDeleted();
            await _unitOfWork.SaveChangesAsync(ct);

            _logger.LogInformation("User marked as deleted: {Code}", pCode);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteUserAsync");
            throw new InvalidOperationException("Failed to delete user: " + ex.Message);
        }
    }


    public async Task<List<UserResponse>> GetAllAsync(CancellationToken ct)
    {
        try
        {
            var items = await _repo.GetAllUsersAsync(ct);
            _logger.LogInformation("Fetched {Count} users", items.Count);

            return items.Select(x => new UserResponse(x.HashId, x.Email, x.FirstName, x.LastName,
                ConvertDate.ConvertToShamsi(x.CreateDate), x.PhoneNumber, x.IpAddress, x.UserName, x.PersonalCode)).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync");
            throw new InvalidOperationException("Failed to fetch users: " + ex.Message);
        }
    }



    public async Task<UserResponse> GetUserAsync(int pCode, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Fetching user with personalCode: {Code}", pCode);
            var user = await _repo.GetUserByPersonalCodeAsync(pCode, ct);

            if (user == null)
            {
                _logger.LogWarning("User not found: {Code}", pCode);
                throw new InvalidOperationException("User not found.");
            }

            return _mapper.Map<UserResponse>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetUserAsync");
            throw new InvalidOperationException("Failed to fetch user: " + ex.Message);
        }
    }


    public async Task<UserResponse> UpdateAsync(UpdateUserRequest request, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("User update failed for ID: {PersonalCode}", request.PersonalCode);
            var user = await _repo.UpdateUserAsync(_mapper.Map<User>(request), ct);
            await _unitOfWork.SaveChangesAsync(ct);

            _logger.LogInformation("User updated successfully: {Id}", user.HashId);
            return _mapper.Map<UserResponse>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateAsync");
            throw new InvalidOperationException("Failed to update user: " + ex.Message);
        }
    }

    #endregion

    #region PermissionMethod

    public async Task<List<PermissionDto>> GetPermissionListAsync(CancellationToken ct = default)
    {
        try
        {
            var listPermissionDb = await _repo.GetPermissionListAsync(ct);
            _logger.LogInformation("Fetched {Count} permissions", listPermissionDb.Count);

            var grouped = listPermissionDb
                .Select(item => {
                    var parts = item.Name.Split('.');
                    return new { Name = parts[0], Ability = parts[1] };
                })
                .GroupBy(x => x.Name)
                .Select(g => new PermissionDto
                {
                    Name = g.Key,
                    Abilites = g.Select(x => x.Ability).ToList()
                })
                .ToList();

            return grouped;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetPermissionListAsync");
            throw new InvalidOperationException("Failed to fetch permissions: " + ex.Message);
        }
    }

    #endregion

}
