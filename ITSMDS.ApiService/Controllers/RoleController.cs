using ITSMDS.Application.Services;
using ITSMDS.Domain.DTOs;
using ITSMDS.Domain.Enums;
using ITSMDS.Infrastructure.Extensions;
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
            return Ok(ApiResponse<PageResultDto<RoleDto>>.Ok(result));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRoleAsync(RoleDtoIn modelIn, CancellationToken ct= default)
        {
            var result = await _roleService.CreateRoleAsync(modelIn, ct);
            if (result.Item1)
            {
                return Ok(ApiResponse<bool>.Ok(true, ErrorCode.RoleCreateSuccessfully.GetMessage()));

            }
            else
            {
                return BadRequest(ApiResponse<object>.Fail(ErrorCode.ValidationError, result.Item2));

            }
        }

        [HttpPut("assignRole")]
        public async Task<IActionResult> AssignRoleToUser(
            [FromQuery] string personalCode,
            [FromQuery] int roleId, CancellationToken ct = default
            )
        {
            var result = await _roleService.AssignRoleToUserAsync(personalCode, roleId, ct);
            if (result)
            {
                return Ok(ApiResponse<bool>.Ok(true, ErrorCode.RoleAsignToUserSuccessfully.GetMessage()));

            }
            return BadRequest(ApiResponse<object>.Fail(ErrorCode.ValidationError));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRole([FromQuery] int roleId, CancellationToken ct= default)
        {
            var result = await _roleService.DeleteRoleAsync(roleId, ct);
            if (result.Item1)
            {
                return Ok(ApiResponse<bool>.Ok(true, result.Item2));

            }
            else
            {
                return BadRequest(ApiResponse<object>.Fail(ErrorCode.DeleteFailed, result.Item2));

            }
        }
    }
}
