using ITSMDS.Application.Abstractions;
using ITSMDS.Domain.DTOs;
using ITSMDS.Domain.Entities;
using ITSMDS.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class ServerRepository : IServerRepository
{
    private readonly ApplicationDbContext _db;

    public ServerRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async ValueTask AddAsync(ServerEntity server, CancellationToken ct = default)
    {
        await _db.Servers.AddAsync(server, ct);
    }

    public async ValueTask<ServerEntity?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        return await _db.Servers
                        .Include(s => s.Department)
                        .Include(s => s.Services)
                        .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted, ct);
    }

    public async ValueTask<List<ServerEntity>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Servers
                        .Include(s => s.Department)
                        .Include(s => s.Services)
                        .Where(s => !s.IsDeleted)
                        .ToListAsync(ct);
    }

    public async ValueTask<bool> ExistsAsync(string serverName, string ipAddress, CancellationToken ct = default)
    {
        return await _db.Servers.AnyAsync(s =>
            !s.IsDeleted &&
            (s.ServerName == serverName || s.IpAddress == ipAddress),
            ct);
    }

    public async ValueTask<Department?> GetDepartmentByIdAsync(long id, CancellationToken ct = default)
    {
        return await _db.Departments.FirstOrDefaultAsync(d => d.Id == id, ct);
    }
}
