
using AutoMapper;
using ITSMDS.Application.Abstractions;
using ITSMDS.Application.Services;
using ITSMDS.Domain.DTOs;
using ITSMDS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ITSMDS.Infrastructure.Services;

public class ServerService : IServerService
{
    private readonly IServerRepository _repo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ServerService> _logger;
    private readonly IMapper _mapper;
    public ServerService(IServerRepository repo, IUnitOfWork unitOfWork, ILogger<ServerService> logger, IMapper mapper)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    #region CRUD

    public async Task<(bool, string)> CreateAsync(CreateServerRequest request, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("CreateAsync called for server: {ServerName}", request.ServerName);

            // بررسی اینکه سرور با همان IP یا نام وجود نداشته باشد
            if (await _repo.ExistsAsync(request.ServerName, request.IpAddress, ct))
            {
                _logger.LogWarning("Server already exists with Name: {ServerName} or IP: {IP}", request.ServerName, request.IpAddress);
                return (false, "سرور با این نام یا آدرس IP قبلا ثبت شده است");
            }

            var server = new ServerEntity(
                request.ServerName,
                request.RAM,
                request.CPU,
                request.MainBoardModel,
                request.StorageSize,
                request.StorageType,
                request.Status,
                request.OS,
                DateTimeOffset.UtcNow,
                request.IpAddress,
                request.Location
            );

            if (request.DepartmentId.HasValue)
            {
                // می‌تونی department رو ریپازیتوری بگیری و assign کنی
                var department = await _repo.GetDepartmentByIdAsync(request.DepartmentId.Value, ct);
                if (department != null)
                    server.AssignToDepartment(department);
            }

            await _repo.AddAsync(server, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            _logger.LogInformation("Server created successfully: {ServerName}", server.ServerName);
            return (true, "سرور با موفقیت ثبت شد");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in CreateAsync");
            return (false, "خطا در ثبت سرور");
        }
    }

    public async Task<ServerEntity?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        return await _repo.GetByIdAsync(id, ct);
    }

    public async Task<List<ServerDto>> GetAllAsync(CancellationToken ct = default)
    {
        var serverList = await _repo.GetAllAsync(ct);
        var result = _mapper.Map<List<ServerDto>>(serverList);
        return result;
    }

    public async Task<(bool, string)> UpdateAsync(long id, UpdateServerRequest request, CancellationToken ct = default)
    {
        try
        {
            var server = await _repo.GetByIdAsync(id, ct);
            if (server == null)
            {
                _logger.LogWarning("Server not found for update: {Id}", id);
                return (false, "سرور یافت نشد");
            }

            server.UpdateServerName(request.ServerName);
            server.UpdateHardwareSpecs(request.RAM, request.CPU, request.MainBoardModel, request.StorageSize,
                request.StorageType, request.Status);
            server.UpdateSoftware(request.OS);
            server.UpdateNetworkInfo(request.IpAddress, request.Location);

            if (request.DepartmentId.HasValue)
            {
                var department = await _repo.GetDepartmentByIdAsync(request.DepartmentId.Value, ct);
                if (department != null)
                    server.AssignToDepartment(department);
            }

            await _unitOfWork.SaveChangesAsync(ct);

            _logger.LogInformation("Server updated successfully: {Id}", id);
            return (true, "اطلاعات سرور با موفقیت به‌روزرسانی شد");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in UpdateAsync");
            return (false, "خطا در به‌روزرسانی سرور");
        }
    }

    public async Task<(bool, string)> DeleteAsync(long id, CancellationToken ct = default)
    {
        try
        {
            var server = await _repo.GetByIdAsync(id, ct);
            if (server == null)
            {
                _logger.LogWarning("Server not found for delete: {Id}", id);
                return (false, "سرور یافت نشد");
            }

            server.MarkAsDeleted();

            await _unitOfWork.SaveChangesAsync(ct);

            _logger.LogInformation("Server deleted successfully: {Id}", id);
            return (true, "سرور با موفقیت حذف شد");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in DeleteAsync");
            return (false, "خطا در حذف سرور");
        }
    }

    #endregion
}

