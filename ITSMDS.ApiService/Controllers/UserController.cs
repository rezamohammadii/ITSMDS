using System.Text.Json;
using Azure.Core;
using ITSMDS.Core.Application.DTOs;
using ITSMDS.Core.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                Console.WriteLine(JsonSerializer.Serialize(userRequest));
                var user = await _userService.CreateAsync(userRequest, ct);
                if (user is not null)
                {
                    return Ok(new
                    {
                        response = true,
                        message = "User create successful"
                    });
                }
                else
                {
                    return BadRequest("Failed create user");
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed create user {MSG}", ex.Message);
                return StatusCode(500);
            }
          
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync(CancellationToken ct )
        {
            var userList = await _userService.GetAllAsync(ct);
            return Ok(userList);
        }

        [HttpGet("{personalCode}")]
        public async Task<IActionResult> GetUserAsunc(int personalCode, CancellationToken ct)
        {

            try
            {
                var user = await _userService.GetUserAsync(personalCode, ct);
                if (user is not null)
                {
                    return Ok(user);
                }
                else
                {
                    return BadRequest("Failed update user");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Failed update user {MSG}", ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPut("edit")]
        public async Task<IActionResult> EditUserAsync(UpdateUserRequest request, CancellationToken ct)
        {
            try
            {
                var user = await _userService.UpdateAsync(request, ct);
                if (user is not null)
                {
                    return Ok(new
                    {
                        response = user.id,
                        message = "User update successful"
                    });
                }
                else
                {
                    return BadRequest("Failed update user");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Failed update user {MSG}", ex.Message);
                return StatusCode(500);
            }
        }

        [HttpDelete("delete/{pCode}")]
        public async Task<IActionResult> DeleteUserAsync(int pCode, CancellationToken ct)
        {
            try
            {
                var res = await _userService.DeleteUserAsync(pCode, ct);
                if (res)
                {
                    return Ok(new
                    {
                        response = res,
                        message = "User deleted successful"
                    });
                }
                else
                {
                    return BadRequest("Failed deleted user");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Failed deleted user {MSG}", ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("permissions")]
        public async Task<IActionResult> GetPermissionsAsync(CancellationToken ct = default)
        {

        }
    } 
}
