

using ITSMDS.Application.Abstractions;
using ITSMDS.Domain.DTOs;
using ITSMDS.Domain.Entities;
using ITSMDS.Infrastructure.Database;
using ITSMDS.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ITSMDS.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;
    public UserRepository(ApplicationDbContext db) => _db = db;


    #region UserMethod
    public async ValueTask AddUserAsync(User user, CancellationToken cancellationToken = default)
       => await _db.Users.AddAsync(user, cancellationToken);

    public async ValueTask<bool> CheckUserExsitAsync(string? userName = null, int? personalCode = null, CancellationToken ct = default)
    {
        if (userName != null)
        {
            return await _db.Users.AnyAsync(x => x.UserName == userName, ct);
        }
        if (personalCode != null)
        {
            return await _db.Users.AnyAsync(x => x.PersonalCode == personalCode, ct);
        }
        else
        {
            return false;
        }
    }
    public async ValueTask<List<User>> GetAllUsersAsync(CancellationToken cancellationToken)
      => await _db.Users.Where(x => !x.IsDeleted)
        .Include(x => x.UserRoles).ThenInclude(x => x.Role)
        .AsNoTracking().OrderByDescending(x => x.Id).ToListAsync(cancellationToken);
    public async ValueTask<User?> GetUserByPersonalCodeAsync(int code, CancellationToken cancellationToken = default)
      => await _db.Users.FirstOrDefaultAsync(x => x.PersonalCode == code, cancellationToken);

    public async ValueTask<User?> UpdateUserAsync(User request, CancellationToken cancellationToken = default)
    {
        var user = await _db.Users.Where(x => x.PersonalCode == request.PersonalCode)
            .FirstOrDefaultAsync(cancellationToken);
        if (user != null)
        {
            try
            {
                user.UpdatePersonalInfo(request.FirstName, request.LastName, request.Email, request.PhoneNumber,
                    request.IpAddress, request.UserName, request.PersonalCode);

                return user;
            }
            catch (Exception ex)
            {
                throw;
            }


        }
        return default;
    }
    #endregion

    #region PermissionMethod
    public async ValueTask<List<Permission?>> GetPermissionListAsync(CancellationToken cancellationToken = default)
    {
        var result = await _db.Permissions.Where(x => x.IsActive && !x.IsDeleted).ToListAsync(cancellationToken);
        return result;
    }

    public async ValueTask<Permission?> GetPermissionByNameAsync(string permissionName, CancellationToken ct = default)
    {
        var result = await _db.Permissions.Where(x => !x.IsDeleted &&  x.Name == permissionName).FirstOrDefaultAsync(ct);
        return result;
    }

    #endregion



}
