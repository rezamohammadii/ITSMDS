using ITSMDS.Application.Services;
using ITSMDS.Domain.DTOs;
using ITSMDS.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITSMDS.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly IServerService _serverService;
        private readonly ILogger<ServerController> _logger;

        public ServerController(IServerService serverService, ILogger<ServerController> logger)
        {
            _serverService = serverService;
            _logger = logger;
        }

        /// <summary>
        /// ایجاد سرور جدید
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> CreateServerAsync(CreateServerRequest request, CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("CreateServerAsync called with data: {@request}", request);

                var result = await _serverService.CreateAsync(request, ct);

                if (result.Item1)
                {
                    _logger.LogInformation("Server created successfully with name: {ServerName}", request.ServerName);
                    return Ok(ApiResponse<bool>.Ok(result.Item1, "سرور با موفقیت ایجاد شد."));
                }

                _logger.LogWarning("Server creation failed for data: {@request}", request);
                return BadRequest(ApiResponse<object>.Fail(ErrorCode.ValidationError, result.Item2));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in CreateServerAsync");
                return StatusCode(500, ApiResponse<object>.Fail(ErrorCode.ServerError));
            }
        }

        /// <summary>
        /// دریافت اطلاعات یک سرور بر اساس Id
        /// </summary>
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetServerByIdAsync(long id, CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("GetServerByIdAsync called with id: {Id}", id);

                var server = await _serverService.GetByIdAsync(id, ct);
                if (server == null)
                {
                    _logger.LogWarning("Server not found with id: {Id}", id);
                    return NotFound(ApiResponse<object>.Fail(ErrorCode.NotFound, "سرور یافت نشد"));
                }

                return Ok(ApiResponse<object>.Ok(server));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetServerByIdAsync");
                return StatusCode(500, ApiResponse<object>.Fail(ErrorCode.ServerError));
            }
        }

        /// <summary>
        /// دریافت لیست همه سرورها
        /// </summary>
        [HttpGet("list")]
        public async Task<IActionResult> GetServersAsync(CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("GetServersAsync called");

                var servers = await _serverService.GetAllAsync(ct);
                return Ok(ApiResponse<List<ServerDto>>.Ok(servers));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetServersAsync");
                return StatusCode(500, ApiResponse<object>.Fail(ErrorCode.ServerError));
            }
        }

        /// <summary>
        /// ویرایش اطلاعات سرور
        /// </summary>
        [HttpPut("update/{id:long}")]
        public async Task<IActionResult> UpdateServerAsync(long id, UpdateServerRequest request, CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("UpdateServerAsync called with id: {Id}", id);

                var result = await _serverService.UpdateAsync(id, request, ct);

                if (result.Item1)
                {
                    _logger.LogInformation("Server updated successfully: {Id}", id);
                    return Ok(ApiResponse<bool>.Ok(result.Item1, result.Item2));
                }

                _logger.LogWarning("Server update failed: {Id}", id);
                return BadRequest(ApiResponse<object>.Fail(ErrorCode.ValidationError, result.Item2));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in UpdateServerAsync");
                return StatusCode(500, ApiResponse<object>.Fail(ErrorCode.ServerError));
            }
        }

        /// <summary>
        /// حذف سرور
        /// </summary>
        [HttpDelete("delete/{id:long}")]
        public async Task<IActionResult> DeleteServerAsync(long id, CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("DeleteServerAsync called with id: {Id}", id);

                var result = await _serverService.DeleteAsync(id, ct);

                if (result.Item1)
                {
                    _logger.LogInformation("Server deleted successfully: {Id}", id);
                    return Ok(ApiResponse<bool>.Ok(result.Item1, result.Item2));
                }

                _logger.LogWarning("Server delete failed: {Id}", id);
                return BadRequest(ApiResponse<object>.Fail(ErrorCode.ValidationError, result.Item2));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in DeleteServerAsync");
                return StatusCode(500, ApiResponse<object>.Fail(ErrorCode.ServerError));
            }
        }
    }
}

