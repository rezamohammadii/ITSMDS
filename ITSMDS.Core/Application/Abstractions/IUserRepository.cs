using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITSMDS.Domain.Entities;

namespace ITSMDS.Core.Application.Abstractions
{
    public interface IUserRepository
    {
        #region General
        ValueTask<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
        ValueTask AddUserAsync(User user, CancellationToken cancellationToken = default);
        ValueTask<List<User>> GetAllUsersAsync(CancellationToken cancellationToken = default);
        ValueTask<>
        #endregion

        #region Auth

        #endregion
    }
}
