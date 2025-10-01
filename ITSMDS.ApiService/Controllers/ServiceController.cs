using ITSMDS.Application.Services;
using ITSMDS.Domain.DTOs;
using ITSMDS.Domain.Entities;
using ITSMDS.Domain.Enums;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITSMDS.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {


        private readonly IServiceProviderService _serviceService;
        private readonly ILogger<ServiceController> _logger;

        public ServiceController(IServiceProviderService serviceService, ILogger<ServiceController> logger)
        {
            _serviceService = serviceService;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] ServiceEntity service, CancellationToken ct)
        {
            try
            {
                var result = await _serviceService.CreateAsync(service, ct);
                if (result.Success)
                    return Ok(ApiResponse<bool>.Ok(result.Item1, "سرویس با موفقیت ایجاد شد."));


                return BadRequest(ApiResponse<object>.Fail(ErrorCode.ValidationError, result.Item2));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateAsync");
                return StatusCode(500, ApiResponse<object>.Fail(ErrorCode.ServerError));

            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetServiceListAsync(CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("GetServiceListAsync called");

                var result = await _serviceService.GetServiceListAsync(ct);
                return Ok(ApiResponse<List<ServiceDto>>.Ok(result));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetServiceListAsync ");
                return StatusCode(500, ApiResponse<object>.Fail(ErrorCode.ServerError));
            }
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetByIdAsync(long id, CancellationToken ct)
        {
            try
            {
                var result = await _serviceService.GetByIdAsync(id, ct);
                if (result.Success)
                    return Ok(ApiResponse<ServiceDto>.Ok(result.Data));

                return NotFound(ApiResponse<object>.Fail(ErrorCode.NotFound, "سرویس یافت نشد"));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetByIdAsync for id {Id}", id);
                return StatusCode(500, ApiResponse<object>.Fail(ErrorCode.ServerError));
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] ServiceEntity service, CancellationToken ct)
        {
            try
            {
                var result = await _serviceService.UpdateAsync(service, ct);
                if (result.Success)
                    return Ok(ApiResponse<bool>.Ok(result.Item1, result.Item2));

                return BadRequest(ApiResponse<object>.Fail(ErrorCode.ValidationError, result.Item2));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateAsync");
                return StatusCode(500, ApiResponse<object>.Fail(ErrorCode.ServerError));
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteAsync(long id, CancellationToken ct)
        {
            try
            {
                var result = await _serviceService.DeleteAsync(id, ct);
                if (result.Success)
                    return Ok(ApiResponse<bool>.Ok(result.Item1, result.Item2));

                return BadRequest(ApiResponse<object>.Fail(ErrorCode.ValidationError, result.Item2));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteAsync for id {Id}", id);
                return StatusCode(500, ApiResponse<object>.Fail(ErrorCode.ServerError));
            }
        }

        [HttpGet("server/{serverId:long}")]
        public async Task<IActionResult> GetByServerIdAsync(long serverId, CancellationToken ct)
        {
            try
            {
                var services = await _serviceService.GetByServerIdAsync(serverId, ct);
                return Ok(new { Success = true, Data = services });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching services for server {ServerId}", serverId);
                return StatusCode(500, new { Success = false, Message = "Server error" });
            }
        }

        //[HttpGet("with-ports/{id:long}")]
        //public async Task<IActionResult> GetWithPortsAsync(long id, CancellationToken ct)
        //{
        //    try
        //    {
        //        var service = await _serviceService.GetWithPortsAsync(id, ct);
        //        if (service != null)
        //            return Ok(new { Success = true, Data = service });

        //        return NotFound(new { Success = false, Message = "Service not found" });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error fetching service with ports for id {Id}", id);
        //        return StatusCode(500, new { Success = false, Message = "Server error" });
        //    }
        //}
    }
}
