

using ITSMDS.Domain.DTOs;
using ITSMDS.Domain.Entities;

namespace ITSMDS.Application.Services;

public interface IServiceProviderService
{
    Task<(bool Success, string Message)> CreateAsync(ServiceEntity service, CancellationToken ct = default); 
    Task<(bool Success, string Message, ServiceDto? Data)> GetByIdAsync(long id, CancellationToken ct = default); 
    Task<List<ServiceDto>> GetServiceListAsync(CancellationToken ct = default); 
    Task<(bool Success, string Message)> UpdateAsync(ServiceEntity service, CancellationToken ct = default); 
    Task<(bool Success, string Message)> DeleteAsync(long id, CancellationToken ct = default); 
    Task<IReadOnlyList<ServiceEntity>> GetByServerIdAsync(long serverId, CancellationToken ct = default); 
  //  Task<ServiceEntity?> GetWithPortsAsync(long id, CancellationToken ct = default);
}
