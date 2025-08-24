

using ITSMDS.Core.Application.Abstractions;
using ITSMDS.Domain.Entities;
using ITSMDS.Infrastructure.Database;
using ITSMDS.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ITSMDS.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;
    public UserRepository(ApplicationDbContext db) => _db = db;
  
    public async ValueTask AddUserAsync(User user, CancellationToken cancellationToken = default)
        => await _db.Users.AddAsync(user, cancellationToken);

    public async ValueTask<List<User>> GetAllUsersAsync(CancellationToken cancellationToken = default)
        => await _db.Users.AsNoTracking().OrderByDescending(x => x.Id).NoLockToListAsync(cancellationToken);

    public async ValueTask<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _db.Users.AsNoTracking().NoLockFirstOrDefaultAsync(x => x.Id == id, cancellationToken);
}
