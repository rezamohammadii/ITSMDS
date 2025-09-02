

namespace ITSMDS.Core.Application.DTOs;

public class RoleDto
{
    public string RoleName { get; set; }
    public List<PermissionDto> AccessNames { get; set; }
    public string UpdateTime { get; set; }
    public string Description { get; set; }
    public int UserCount { get; set; }
    public bool IsActive { get; set; }
}

public class RoleDtoIn
{
    public string RoleName { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }

}
