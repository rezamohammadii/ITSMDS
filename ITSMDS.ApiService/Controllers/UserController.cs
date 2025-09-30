using Azure.Core;
using ITSMDS.Application.Services;
using ITSMDS.Domain.DTOs;
using ITSMDS.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ITSMDS.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUserAsync(CreateUserRequest userRequest, CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("CreateUserAsync called with data: {@userRequest}", userRequest);
                var user = await _userService.CreateAsync(userRequest, ct);

                if (user.Item1)
                {
                    _logger.LogInformation("User created successfully with Personali Code: {PC}", userRequest.personalCode);
                    return Ok(ApiResponse<bool>.Ok(user.Item1, "کاربر با موفقیت ایجاد شد."));
                }

                _logger.LogWarning("User creation failed for data: {@userRequest}", userRequest);
                return BadRequest(ApiResponse<object>.Fail(ErrorCode.ValidationError));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in CreateUserAsync");
                return StatusCode(500, ApiResponse<object>.Fail(ErrorCode.ServerError));
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync(CancellationToken ct)
        {
            try
            {
                var userList = await _userService.GetAllAsync(ct);
                _logger.LogInformation("Fetched {Count} users", userList.Count);
                return Ok(ApiResponse<List<UserResponse>>.Ok(userList));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetAllAsync");
                return StatusCode(500, ApiResponse<object>.Fail(ErrorCode.ServerError));
            }
        }

        [HttpGet("{personalCode}")]
        public async Task<IActionResult> GetUserAsync(int personalCode, CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Fetching user with personalCode: {Code}", personalCode);
                var user = await _userService.GetUserByPersonalCodeAsync(personalCode, ct);

                if (user is not null)
                {
                    _logger.LogInformation("User found: {@user}", user);
                    return Ok(ApiResponse<UserResponse>.Ok(user));
                }

                _logger.LogWarning("User not found with personalCode: {Code}", personalCode);
                return NotFound(ApiResponse<object>.Fail(ErrorCode.NotFound));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetUserAsync");
                return StatusCode(500, ApiResponse<object>.Fail(ErrorCode.ServerError));
            }
        }

        [HttpPut("edit")]
        public async Task<IActionResult> EditUserAsync(UpdateUserRequest request, CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("EditUserAsync called with data: {@request}", request);
                var user = await _userService.UpdateAsync(request, ct);

                if (user is not null)
                {
                    _logger.LogInformation("User updated successfully: {UserId}", user.id);
                    return Ok(ApiResponse<UserResponse>.Ok(user, "ویرایش کاربر با موفقیت انجام شد."));
                }

                _logger.LogWarning("User update failed for ID: {PersonalCode}", request.PersonalCode);
                return BadRequest(ApiResponse<object>.Fail(ErrorCode.UpdateFailed , "خطا در ویرایش کاربر"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in EditUserAsync");
                return StatusCode(500, ApiResponse<object>.Fail(ErrorCode.ServerError));
            }
        }

        [HttpDelete("delete/{pCode}")]
        public async Task<IActionResult> DeleteUserAsync(int pCode, CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Attempting to delete user with code: {Code}", pCode);
                var res = await _userService.DeleteUserAsync(pCode, ct);

                if (res)
                {
                    _logger.LogInformation("User deleted successfully: {Code}", pCode);
                    return Ok(ApiResponse<bool>.Ok(true, "کاربر با موفقیت حذف شد."));
                }

                _logger.LogWarning("User deletion failed: {Code}", pCode);
                return BadRequest(ApiResponse<object>.Fail(ErrorCode.DeleteFailed, "امکان حذف کاربر وجود ندارد"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in DeleteUserAsync");
                return StatusCode(500, ApiResponse<object>.Fail(ErrorCode.ServerError));
            }
        }

        [HttpGet("permissions")]
        public async Task<IActionResult> GetPermissionsAsync(CancellationToken ct = default)
        {
            try
            {
                var result = await _userService.GetPermissionListAsync(ct);
                _logger.LogInformation("Permissions fetched: {Count}", result.Count);
                return Ok(ApiResponse<List<PermissionDto>>.Ok(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetPermissionsAsync");
                return StatusCode(500, ApiResponse<object>.Fail(ErrorCode.ServerError));
            }
        }
    }
}
