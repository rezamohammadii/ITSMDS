using ITSMDS.Application.Services;
using ITSMDS.Domain.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITSMDS.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetRoleListAsync(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string searchTerm = "", CancellationToken ct = default)
        {
            var result  = await _roleService.GetAllRoleAsync(pageNumber, pageSize, searchTerm, ct);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRoleAsync(RoleDtoIn modelIn, CancellationToken ct= default)
        {
            var result = await _roleService.CreateRoleAsync(modelIn, ct);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
