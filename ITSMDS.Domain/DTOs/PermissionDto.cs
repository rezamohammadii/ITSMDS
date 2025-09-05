namespace ITSMDS.Domain.DTOs;

public class PermissionDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<string> Abilites { get; set; }
}
