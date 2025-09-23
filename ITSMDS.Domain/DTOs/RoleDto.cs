using System.ComponentModel.DataAnnotations;

namespace ITSMDS.Domain.DTOs;

public class RoleDto
{
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public List<PermissionDto> Permissions { get; set; }
    public string UpdateTime { get; set; }
    public string Description { get; set; }
    public int UserCount { get; set; }
    public bool IsActive { get; set; }
}

public class RoleDtoIn
{
    [Required(ErrorMessage = "🛑 نام الزامی است.")]
    [MaxLength(20, ErrorMessage = "نام نباید بیشتر از 20 کاراکتر باشد")]
    [MinLength(3, ErrorMessage = "نام باید حداقل 3 کاراکتر باشد")]
    public string RoleName { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public List<string> SelectedPermissions { get; set; } = new();


}
