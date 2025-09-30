using ITSMDS.Application.Abstractions;
using ITSMDS.Domain.Entities;
using ITSMDS.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITSMDS.Infrastructure.Repository;

public class ServiceRepository : IServiceRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<ServiceRepository> _logger;
    public ServiceRepository(ApplicationDbContext dbContext, ILogger<ServiceRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    public async ValueTask<ServiceEntity> CreateAsync(ServiceEntity service, CancellationToken ct = default)
    {
        try
        {
            await _dbContext.Services.AddAsync(service, ct);
            await _dbContext.SaveChangesAsync(ct);
            return service;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating ServiceEntity {@Service}", service);
            throw;
        }
    }
    public async ValueTask<ServiceEntity?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        return await _dbContext.Services.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, ct);
    }
    public async ValueTask UpdateAsync(ServiceEntity service, CancellationToken ct = default)
    {
        try
        {
            _dbContext.Services.Update(service);
            await _dbContext.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating ServiceEntity {@Service}", service);
            throw;
        }
    }
    public async ValueTask DeleteAsync(long id, CancellationToken ct = default)
    {
        try
        {
            var service = await _dbContext.Services.FindAsync(new object[] { id }, ct);
            if (service == null) throw new KeyNotFoundException($"ServiceEntity with Id {id} not found.");
            _dbContext.Services.Remove(service);
            await _dbContext.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting ServiceEntity with Id {Id}", id);
            throw;
        }
    }
    //public async ValueTask<ServiceEntity?> GetWithPortsAsync(long id, CancellationToken ct = default)
    //{ 
    //    try 
    //    { 
    //        return await _dbContext.Services.Include(s => s.PortServices).ThenInclude(ps => ps.Port).Include(s => s.Server).FirstOrDefaultAsync(s => s.Id == id, ct);
    //    } 
    //    catch (Exception ex)
    //    { 
    //        _logger.LogError(ex, "Error fetching ServiceEntity with ports by Id {Id}", id);
    //        throw;
    //    } 
    //}
    public async ValueTask<IReadOnlyList<ServiceEntity>> GetByServerIdAsync(long serverId, CancellationToken ct = default) 
    { 
        try 
        { return await _dbContext.Services.Where(s => s.ServerId == serverId).ToListAsync(ct);
        } 
        catch (Exception ex) 
        { 
            _logger.LogError(ex, "Error fetching ServiceEntity list for ServerId {ServerId}", serverId);
            throw; 
        }
    }
}

