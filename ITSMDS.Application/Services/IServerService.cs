

using ITSMDS.Domain.DTOs;
using ITSMDS.Domain.Entities;

namespace ITSMDS.Application.Services;

public interface IServerService
{
    Task<(bool, string)> CreateAsync(CreateServerRequest request, CancellationToken ct = default);
    Task<ServerDto?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<List<ServerDto>> GetAllAsync(CancellationToken ct = default);
    Task<(bool, string)> UpdateAsync(long id, UpdateServerRequest request, CancellationToken ct = default);
    Task<(bool, string)> DeleteAsync(long id, CancellationToken ct = default);
    Task<ServerWidget> GetServerWidgetAsync(CancellationToken ct = default);


}
