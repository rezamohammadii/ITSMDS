using ITSMDS.Application.Services;
using ITSMDS.Domain.Entities;
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
                    return Ok(new { Success = true, Message = result.Message, Data = result.Data });

                return BadRequest(new { Success = false, Message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateAsync");
                return StatusCode(500, new { Success = false, Message = "Server error" });
            }
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetByIdAsync(long id, CancellationToken ct)
        {
            try
            {
                var result = await _serviceService.GetByIdAsync(id, ct);
                if (result.Success)
                    return Ok(new { Success = true, Message = result.Message, Data = result.Data });

                return NotFound(new { Success = false, Message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetByIdAsync for id {Id}", id);
                return StatusCode(500, new { Success = false, Message = "Server error" });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] ServiceEntity service, CancellationToken ct)
        {
            try
            {
                var result = await _serviceService.UpdateAsync(service, ct);
                if (result.Success)
                    return Ok(new { Success = true, Message = result.Message });

                return BadRequest(new { Success = false, Message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateAsync");
                return StatusCode(500, new { Success = false, Message = "Server error" });
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteAsync(long id, CancellationToken ct)
        {
            try
            {
                var result = await _serviceService.DeleteAsync(id, ct);
                if (result.Success)
                    return Ok(new { Success = true, Message = result.Message });

                return BadRequest(new { Success = false, Message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteAsync for id {Id}", id);
                return StatusCode(500, new { Success = false, Message = "Server error" });
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
