

using AutoMapper;
using Azure.Core;
using ITSMDS.Application.Abstractions;
using ITSMDS.Application.CustomExceptions;
using ITSMDS.Application.Services;
using ITSMDS.Domain.DTOs;
using ITSMDS.Domain.Entities;
using ITSMDS.Domain.Tools;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ITSMDS.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly IRoleRepository _roleRepository;
    public UserService(IUnitOfWork unitOfWork, IUserRepository repo, ILogger<UserService> logger, IMapper mapper, IRoleRepository roleRepository)
    {
        _unitOfWork = unitOfWork;
        _repo = repo;
        _logger = logger;
        _mapper = mapper;
        _roleRepository = roleRepository;
    }

    #region UserMethod
    public async Task<(bool, string)> CreateAsync(CreateUserRequest request, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("CreateAsync called with username: {Username}, personalCode: {Code}", request.userName, request.personalCode);

            if (await _repo.CheckUserExsitAsync(request.userName, int.Parse(request.personalCode), ct))
            {
                _logger.LogWarning("User already exists: {Username}", request.userName);
                return (false, "نام کاربری یا کد پرسنلی در سیستم موجود است");
               // throw new ValidationException("User already exists", new { Field = "Username" });
            }

            string hashPass = HashGenerator.GenerateHashSHA512(request.password);
            var user = new User(request.firstName, request.lastName, request.email, int.Parse(request.personalCode),
                request.phoneNumber!, request.userName ?? "", hashPass, "", request.ipAddress);

            await _repo.AddUserAsync(user, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            _logger.LogInformation("User created successfully: {UserId}", user.HashId);

            return (true, "کاربر با موفقیت دخیره شد");

        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
        {
            // خطای unique constraint violation
            return (false, "خطا هنگام ذخیره سازی کاربر لطفا بعدا تلاش کنید");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while creating user");
            return (false, "خطا هنگام ذخیره سازی کاربر لطفا بعدا تلاش کنید");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateAsync");
            return (false, "کاربر دخیره نشد خطا نامشخص");
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
                ConvertDate.ConvertToShamsi(x.CreateDate), x.PhoneNumber, x.IpAddress, x.UserName, x.PersonalCode,
                x.UserRoles.Select(x => x.Role.Name).ToList(), x.IsActive
                )).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync");
            throw new InvalidOperationException("Failed to fetch users: " + ex.Message);
        }
    }



    public async Task<UserResponse> GetUserByPersonalCodeAsync(int pCode, CancellationToken ct = default)
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


    public async Task<UserResponse> GetUserByUserNameAsync(string username, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Fetching user with Username: {username}", username);
            var user = await _repo.GetUserByUsernameAsync(username, ct);

            if (user == null)
            {
                _logger.LogWarning("User not found: {Username}", username);
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

    #region Auth
    public async Task<LoginResponseDTO> LoginAsync(int personalCode, string? username, string pass, CancellationToken ct = default)
    {
        
        try
        {
            _logger.LogInformation("Fetching user with Username: {username}", username);
            User? user;
            if (!string.IsNullOrEmpty(username))
            {
                user = await _repo.GetUserByUsernameAsync(username, ct);
            }
            else
            {
                _logger.LogInformation("Fetching user with Personal Code: {personalCode}", personalCode);
                user = await _repo.GetUserByPersonalCodeAsync(personalCode, ct);
            }
            if (user == null)
            {
                _logger.LogWarning("User not found: {Username}", username);
                throw new InvalidOperationException("User not found.");
            }
            var hashPass = HashGenerator.GenerateHashSHA512(pass);


            if (user.Password != hashPass)
            {
                _logger.LogWarning("Password not matched");
                throw new InvalidDataException("Password is incorrect");
            }

            var roleQuery = _roleRepository.GetRoleQueryAsync();
            var userRoleList = roleQuery.Where(x => x.UserRoles.Any(x => x.UserId == user.Id)).ToList();
            var roleNames = userRoleList.Select(x => x.Name).ToList();
            var permissionNames = userRoleList.SelectMany(r => r.RolePermissions)
                .Select(rp => rp.Permission.Name)
                .Distinct()
                .ToList();

            var loginResponse = new LoginResponseDTO
            {
                PermissionNames = permissionNames,
                PersonalCode = user.PersonalCode,
                RoleNames = roleNames,
                UserId = user.HashId,
                Username = user.UserName
            };

            return (loginResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in LoginAsync");
            throw new InvalidOperationException("Failed to fetch user: " + ex.Message);
        }


    }
    #endregion

}
