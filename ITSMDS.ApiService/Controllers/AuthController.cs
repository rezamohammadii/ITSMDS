using ITSMDS.Application.Services;
using ITSMDS.Domain.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ITSMDS.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private const string JWT_KEY = "tugf24_213xflpo_oikjhg_jwt_key_12345678000090!!";
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO loginDto, CancellationToken cancellationToken)
        {
            var user = await _userService.LoginAsync(loginDto.PersonalCode, loginDto.Username, loginDto.Password, cancellationToken);

            if (user is null)
            {
                return Unauthorized("User or passwoed invalid");
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(JWT_KEY);

            StringBuilder roleBuilder = new StringBuilder();
            foreach (var role in user.PermissionNames)
            {
                roleBuilder.Append(role);
            }

            StringBuilder permissionName = new StringBuilder();
            foreach (var role in user.RoleNames)
            {
                roleBuilder.Append(role);
            }
            
            var tokenDecriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                      new Claim(ClaimTypes.Name, loginDto.Username ?? ""),
                      new Claim(ClaimTypes.NameIdentifier, user.PersonalCode.ToString() ?? ""),
                      new Claim(ClaimTypes.Role, roleBuilder.ToString()),
                      new Claim("Permissions", permissionName.ToString())

                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = "http://htsc.local",
                Audience = "htsc.info",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };

            var token = tokenHandler.CreateToken(tokenDecriptor);
            var jwt = tokenHandler.WriteToken(token);
            user.Token= jwt;
            return Ok(user);

        }

        //[HttpGet("whoami")]
        //public async Task<IActionResult> WhoAmICheckAsync(CancellationToken ct =default)
        //{
        //    if (HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        var 
        //    }
        //}
    }
}
