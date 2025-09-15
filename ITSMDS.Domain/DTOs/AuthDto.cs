

namespace ITSMDS.Domain.DTOs;

public class LoginDTO
{
    public int PersonalCode { get; set; }
    public string? Username { get; set; }
    public string Password { get; set; }
}

public class LoginResponseDTO 
{
    public string? Token { get; set; }
    public string UserId { get; set; }
    public int PersonalCode { get; set; }
    public string? Username { get; set; }
    public List<string>? RoleNames { get; set; }
    public List<string>? PermissionNames { get; set; }
}
