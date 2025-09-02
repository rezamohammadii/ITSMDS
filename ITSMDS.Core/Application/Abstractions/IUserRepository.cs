using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITSMDS.Core.Application.DTOs;
using ITSMDS.Domain.Entities;

namespace ITSMDS.Core.Application.Abstractions
{
    public interface IUserRepository
    {
        #region User
        ValueTask<User?> GetUserByPersonalCodeAsync(int code, CancellationToken cancellationToken = default);
        ValueTask<User?> UpdateUserAsync(User request, CancellationToken cancellationToken = default);
        ValueTask AddUserAsync(User user, CancellationToken cancellationToken = default);
        ValueTask<List<User>> GetAllUsersAsync(CancellationToken cancellationToken = default);
        ValueTask<bool> CheckUserExsitAsync(string? userName = null, int? personalCode = null, CancellationToken ct = default);

        #endregion

        #region Permissions
        ValueTask<List<Permission?>> GetPermissionListAsync(CancellationToken cancellationToken = default); 


        #endregion
    }
}
