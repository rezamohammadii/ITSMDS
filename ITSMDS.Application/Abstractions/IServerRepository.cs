using ITSMDS.Domain.Entities;


namespace ITSMDS.Application.Abstractions;

public interface IServerRepository
{
    ValueTask AddAsync(ServerEntity server, CancellationToken ct = default);
    ValueTask<ServerEntity?> GetByIdAsync(long id, CancellationToken ct = default);
    ValueTask<List<ServerEntity>> GetAllAsync(CancellationToken ct = default);
    ValueTask<bool> ExistsAsync(string serverName, string ipAddress, CancellationToken ct = default);
    ValueTask<Department?> GetDepartmentByIdAsync(long id, CancellationToken ct = default);
}
