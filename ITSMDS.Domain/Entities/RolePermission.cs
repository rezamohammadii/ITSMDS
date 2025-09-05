using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITSMDS.Domain.Common;

namespace ITSMDS.Domain.Entities;

public class RolePermission : Entity
{


    public long PermissionId { get; private set; }
    public virtual Permission Permission { get; private set; }

    public long RoleId { get; private set; }
    public virtual Role Role { get; private set; }

    private RolePermission() { }

    public RolePermission(Role role, Permission permission )
    {
        Permission = permission ?? throw new DomainException("Permission cannot be null");
        Role = role ?? throw new DomainException("Role cannot be null");
        PermissionId = permission.Id;
        RoleId = role.Id;
    }
}
