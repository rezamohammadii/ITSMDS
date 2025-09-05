

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

    public async Task<bool> CreateRoleAsync(RoleDtoIn roleInput, CancellationToken ct)
    {
        try
        {
           
            var role = new Role(roleInput.RoleName, roleInput.Description);
            role.Activate();
            foreach (var item in roleInput.SelectedPermissions)
            {
                var permission = await _userRepository.GetPermissionByNameAsync(item);
                if (permission is null) return false;
                role.AddPermission(permission);
            }
            
            await _roleRepository.CreateRoleAsync(role, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<PageResultDto<RoleDto>> GetAllRoleAsync(int pageNumber, int pageSize, string searchTerm, CancellationToken ct)
    {

        try
        {
            var rolesQuery = _roleRepository.GetListRoleAsync();
            
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
                .Skip(itemToSkip)
                .Take(pageSize)                
                .Select(x => new RoleDto
            {
                UpdateTime = ConvertDate.ConvertToShamsi(x.ModifiedTime),
                RoleName = x.Name,
                UserCount = x.UserRoles.Count,
                IsActive = x.IsActive,
                AccessNames = x.RolePermissions.Select(p => new PermissionDto
                {
                    Name = p.Permission.Name,
                    Description = p.Permission.Description,
                }).ToList()
            }).ToListAsync(ct);

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
}
