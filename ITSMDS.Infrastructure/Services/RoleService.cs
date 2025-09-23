

using AutoMapper;
using ITSMDS.Application.Abstractions;
using ITSMDS.Domain.DTOs;
using ITSMDS.Application.Services;
using ITSMDS.Domain;
using ITSMDS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ITSMDS.Domain.Tools;

namespace ITSMDS.Infrastructure.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RoleService> _logger;
    private readonly IUserRepository _userRepository;
    public RoleService(IRoleRepository roleRepository,
        IUnitOfWork unitOfWork, ILogger<RoleService> logger, IUserRepository userRepository
        )
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task<bool> AssignRoleToUserAsync(string personalCode, int roleId, CancellationToken ct)
    {
        var role = await _roleRepository.GetRoleByRoleIdAsync(roleId);
        if (role is null)
        {
            return false;
        }
        var user = await _userRepository.GetUserByPersonalCodeAsync(int.Parse(personalCode), ct);
        if (user is null)
        {
            return false;
        }
        role.AddUser(user);
        await _unitOfWork.SaveChangesAsync(ct);
        return true;
    }

    public async Task<(bool, string)> CreateRoleAsync(RoleDtoIn roleInput, CancellationToken ct)
    {
        try
        {

            if (await _roleRepository.CheckRoleNameExsistAsync(roleInput.RoleName, ct))
            {
                return (false, $"این نقش قبلا در سیستم ثبت شده است : {roleInput.RoleName}");
            }
            var role = new Role(roleInput.RoleName, roleInput.Description);
            role.Activate();
            foreach (var item in roleInput.SelectedPermissions)
            {
                var permission = await _userRepository.GetPermissionByNameAsync(item);
                if (permission is null) return (false  , "دسترسی ها صحیح نمی باشند");
                role.AddPermission(permission);
            }
            
            await _roleRepository.CreateRoleAsync(role, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return (true,  "نقش با موفقیت اضافه شد");
        }
        catch (Exception ex)
        {
            _logger.LogError("error crate new role : {error}", ex.Message);
            return (false, "خطا در اضافه کردن نقش");
        }
    }

    public async Task<(bool, string)> DeleteRoleAsync(int roleId, CancellationToken ct)
    {
        try
        {
            var role = await _roleRepository.GetRoleByRoleIdAsync(roleId);
            if (role == null)
            {
                return (false, "نقش موجود نمی باشد");
            }
            role.MarkAsDeleted();
           
            await _unitOfWork.SaveChangesAsync(ct);
            return (true, "نقش با موفقیت حذف شد");
        }
        catch (Exception ex)
        {
            _logger.LogError("error delete new role : {error}", ex.Message);
            return (false, "خطا در حذف کردن نقش");
        }
    }

    public async Task<PageResultDto<RoleDto>> GetAllRoleAsync(int pageNumber, int pageSize, string searchTerm, CancellationToken ct)
    {

        try
        {
            var rolesQuery = _roleRepository.GetRoleQueryAsync();
            
            int itemToSkip = (pageNumber - 1) * pageSize;

            // search filter
            if (!string.IsNullOrEmpty(searchTerm))
            {
                rolesQuery = rolesQuery.Where(x =>
                    x.Name.Contains(searchTerm) ||
                    (x.Description != null && x.Description.Contains(searchTerm)) ||
                    x.RolePermissions.Any(rp => rp.Permission.Name.Contains(searchTerm))
                    );
            }

            // Get all items
            int totalCount = await rolesQuery.CountAsync();

            var roleList = await rolesQuery
              .OrderBy(x => x.Name)
              //.Skip(itemToSkip)
              //.Take(pageSize)
              .Select(x => new RoleDto
              {
                  UpdateTime = ConvertDate.ConvertToShamsi(x.ModifiedTime),
                  RoleName = x.Name,
                  UserCount = x.UserRoles.Count,
                  IsActive = x.IsActive,
                  RoleId = (int)x.Id,
                  Permissions = x.RolePermissions.Select(
                      rp => new PermissionDto
                      {
                          Name = rp.Permission.Name
                      }
                      ).ToList(),
              }).ToListAsync(ct);

            foreach (var role in roleList)
            {

                role.Permissions = role.Permissions
                    .Select(permissionName =>
                    {
                        var parts = permissionName.Name.Split('.');
                        return new
                        {
                            Category = parts[0],
                            Ability = parts.Length > 1 ? parts[1] : string.Empty
                        };
                    })
                    .Where(x => !string.IsNullOrEmpty(x.Ability))
                    .GroupBy(x => x.Category)
                    .Select(g => new PermissionDto
                    {
                        Name = g.Key,
                        Abilites = g.Select(x => x.Ability).Distinct().ToList()
                    })
                   .ToList();
            }


            // Pagination result
            var result = new PageResultDto<RoleDto>
            {
                TotalCount = totalCount,
                Items = roleList,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
            };

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database Error {MSG}", ex.Message);
            throw new InvalidOperationException(ex.Message);
        }

    }

    private List<PermissionDto> GroupPermissions(List<string> permissionNames)
    {
        return permissionNames
            .Select(permissionName =>
            {
                var parts = permissionName.Split('.');
                return new
                {
                    Category = parts[0],
                    Ability = parts.Length > 1 ? parts[1] : string.Empty
                };
            })
            .Where(x => !string.IsNullOrEmpty(x.Ability))
            .GroupBy(x => x.Category)
            .Select(g => new PermissionDto
            {
                Name = g.Key,
                Abilites = g.Select(x => x.Ability).Distinct().ToList()
            })
            .ToList();
    }
}
