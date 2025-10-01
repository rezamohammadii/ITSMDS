

using AutoMapper;
using ITSMDS.Application.Abstractions;
using ITSMDS.Application.Services;
using ITSMDS.Domain.DTOs;
using ITSMDS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ITSMDS.Infrastructure.Services;

public class ServiceProviderService : IServiceProviderService
{
    private readonly IServiceRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ServiceProviderService> _logger;
    private readonly IMapper _mapper;

    public ServiceProviderService(
        IServiceRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<ServiceProviderService> logger, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<(bool Success, string Message)> CreateAsync(ServiceEntity service, CancellationToken ct = default)
    {
        try
        {
            await _repository.CreateAsync(service, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            _logger.LogInformation("Service created successfully: {ServiceId}", service.Id);
            return (true, "Service created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating service {@Service}", service);
            return (false, "Error creating service");
        }
    }

    public async Task<(bool Success, string Message, ServiceDto? Data)> GetByIdAsync(long id, CancellationToken ct = default)
    {
        try
        {
            var service = await _repository.GetByIdAsync(id, ct);
            if (service == null)
            {
                return (false, "Service not found", null);
            }
            var result = _mapper.Map<ServiceDto>(service);
            return (true, "Service fetched successfully", result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching service with id {Id}", id);
            return (false, "Error fetching service", null);
        }
    }

    public async Task<(bool Success, string Message)> UpdateAsync(ServiceEntity service, CancellationToken ct = default)
    {
        try
        {
            await _repository.UpdateAsync(service, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            _logger.LogInformation("Service updated successfully: {ServiceId}", service.Id);
            return (true, "Service updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating service {@Service}", service);
            return (false, "Error updating service");
        }
    }

    public async Task<(bool Success, string Message)> DeleteAsync(long id, CancellationToken ct = default)
    {
        try
        {
            await _repository.DeleteAsync(id, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            _logger.LogInformation("Service deleted successfully: {ServiceId}", id);
            return (true, "Service deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting service with id {Id}", id);
            return (false, "Error deleting service");
        }
    }

    public async Task<IReadOnlyList<ServiceEntity>> GetByServerIdAsync(long serverId, CancellationToken ct = default)
    {
        try
        {
            return await _repository.GetByServerIdAsync(serverId, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching services for server {ServerId}", serverId);
            throw;
        }
    }

    public async Task<List<ServiceDto>> GetServiceListAsync(CancellationToken ct = default)
    {
        try
        {
            var service = await _repository.GetServiceListAsync(ct);
            var result = _mapper.Map<List<ServiceDto>>(service);
            result = result.Select(s => new ServiceDto
            {
                ServiceName = s.ServiceName,
                CreateTime = s.CreateTime,
                CriticalityScore = s.CriticalityScore,
                Description = s.Description,
                ServerName = service.Select(x => x.Server.ServerName).FirstOrDefault(),
                DocumentFilePath = s.DocumentFilePath,
                ExcutionPath = s.ExcutionPath,
                Port = s.Port,
                IsActive = s.IsActive,
                Version = s.Version
            }).ToList();
            return (result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching services ");
            return (null);
        }
    }

    //public async Task<ServiceEntity?> GetWithPortsAsync(long id, CancellationToken ct = default)
    //{
    //    try
    //    {
    //        return await _repository.GetWithPortsAsync(id, ct);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Error fetching service with ports by id {Id}", id);
    //        throw;
    //    }
    //}
}

