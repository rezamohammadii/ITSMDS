using ITSMDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITSMDS.Application.Abstractions;


public interface IServiceRepository 
{
    ValueTask<List<ServiceEntity>> GetServiceListAsync( CancellationToken ct = default);
    ValueTask<IReadOnlyList<ServiceEntity>> GetByServerIdAsync(long serverId, CancellationToken ct = default);
    // CRUD
    ValueTask<ServiceEntity> CreateAsync(ServiceEntity service, CancellationToken ct = default);
    ValueTask<ServiceEntity?> GetByIdAsync(long id, CancellationToken ct = default);
    ValueTask UpdateAsync(ServiceEntity service, CancellationToken ct = default);
    ValueTask DeleteAsync(long id, CancellationToken ct = default); 

}
