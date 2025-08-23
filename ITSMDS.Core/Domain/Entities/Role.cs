using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITSMDS.Core.Domain.Common;

namespace ITSMDS.Domain.Entities;

public class Role : Entity<long>, IAggregateRoot
{
    public string Name { get; private set; }
    public string? Description { get; private set; }

    private readonly List<UserRole> _userRoles = new();
    public virtual IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    private readonly List<RolePermission> _rolePermissions = new();
    public virtual IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();

    // Private constructor for EF Core
    private Role() { }

    public Role(string name, string? description = null)
    {
        ValidateName(name);

        Name = name;
        Description = description;
    }

    public void UpdateName(string name)
    {
        ValidateName(name);
        Name = name;
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
    }

    public void AddUser(User user)
    {
        if (user == null)
            throw new DomainException("User cannot be null");

        if (!_userRoles.Any(ur => ur.UserId == user.Id))
        {
            _userRoles.Add(new UserRole(user, this));
        }
    }

    public void RemoveUser(User user)
    {
        if (user == null)
            throw new DomainException("User cannot be null");

        var userRole = _userRoles.FirstOrDefault(ur => ur.UserId == user.Id);
        if (userRole != null)
        {
            _userRoles.Remove(userRole);
        }
    }

    public void AddPermission(Permission permission)
    {
        if (permission == null)
            throw new DomainException("Permission cannot be null");

        if (!_rolePermissions.Any(rp => rp.PermissionId == permission.Id))
        {
            _rolePermissions.Add(new RolePermission(this, permission));
        }
    }

    public void RemovePermission(Permission permission)
    {
        if (permission == null)
            throw new DomainException("Permission cannot be null");

        var rolePermission = _rolePermissions.FirstOrDefault(rp => rp.PermissionId == permission.Id);
        if (rolePermission != null)
        {
            _rolePermissions.Remove(rolePermission);
        }
    }

    public bool HasPermission(string permissionName)
    {
        return _rolePermissions.Any(rp =>
            rp.Permission.Name.Equals(permissionName, StringComparison.OrdinalIgnoreCase));
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Role name cannot be empty");

        if (name.Length > 100)
            throw new DomainException("Role name cannot exceed 100 characters");
    }
}