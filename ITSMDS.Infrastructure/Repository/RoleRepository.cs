

using System.Security;
using ITSMDS.Application.Abstractions;
using ITSMDS.Domain.Entities;
using ITSMDS.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ITSMDS.Infrastructure.Repository;

public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext _context;

    public RoleRepository(ApplicationDbContext context) => _context = context;
  
    public async ValueTask CreateRoleAsync(Role role, CancellationToken ct)
        => await _context.Roles.AddAsync(role, ct);

    public IQueryable<Role> GetListRoleAsync(CancellationToken ct = default)
    {

        var query =  _context.Roles.Where(x => !x.IsDeleted).Include(x => x.RolePermissions)
            .ThenInclude(x => x.Permission)
            .Include(x => x.UserRoles).ThenInclude(x => x.User)
            .AsQueryable();

        return query;
    }

    public async ValueTask<Role> GetRoleByRoleIdAsync(int roleId, CancellationToken ct = default)
    {
        var role = await _context.Roles.Where(x => x.Id == roleId && !x.IsDeleted).FirstOrDefaultAsync(ct);
        return role;
    }
}
