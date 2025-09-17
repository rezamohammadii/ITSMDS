

using ITSMDS.Domain.Common;

namespace ITSMDS.Domain.Entities;

public class Permission : Entity<long>, IAggregateRoot
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public DateTime CreateDate { get; private set; }
    public DateTime ModifiedTime { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    private readonly List<RolePermission> _rolePermissions = new();
    public virtual IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();

    // Private constructor for EF Core
    private Permission() { }

    public Permission(string name, string? description = null, bool isActive = default, bool isDeleted = default)
    {
        ValidateName(name);

        Name = name;
        Description = description;
        IsActive = isActive;
        IsDeleted = isDeleted;
    }

    public void UpdateName(string name)
    {
        ValidateName(name);
        Name = name;
        ModifiedTime = DateTime.UtcNow;

    }

    public void UpdateDescription(string? description)
    {
        Description = description;
        ModifiedTime = DateTime.UtcNow;

    }

    public void AssignToRole(Role role)
    {
        if (role == null)
            throw new DomainException("Role cannot be null");

        if (!_rolePermissions.Any(rp => rp.RoleId == role.Id))
        {
            _rolePermissions.Add(new RolePermission(role, this));
        }
    }

    public void RemoveFromRole(Role role)
    {
        if (role == null)
            throw new DomainException("Role cannot be null");

        var rolePermission = _rolePermissions.FirstOrDefault(rp => rp.RoleId == role.Id);
        if (rolePermission != null)
        {
            _rolePermissions.Remove(rolePermission);
        }
    }

    public bool IsAssignedToRole(string roleName)
    {
        return _rolePermissions.Any(rp =>
            rp.Role.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Permission name cannot be empty");

        if (name.Length > 150)
            throw new DomainException("Permission name cannot exceed 150 characters");
    }
    public void Activate()
    {
        IsActive = true;
        ModifiedTime = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        ModifiedTime = DateTime.UtcNow;
    }
    public void MarkAsDeleted()
    {
        IsDeleted = true;
        IsActive = false;
        ModifiedTime = DateTime.UtcNow;
    }
}