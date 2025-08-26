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
                var user = await _userService.CreateAsync(userRequest, ct);
                if (user is not null)
                {
                    return Ok(new
                    {
                        response = user.id,
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
    }
}
