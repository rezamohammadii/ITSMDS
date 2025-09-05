

using ITSMDS.Domain.Common;

namespace ITSMDS.Domain.Entities;

public class UserRole : Entity
{
    public long UserId { get; private set; }
    public virtual User User { get; private set; }

    public long RoleId { get; private set; }
    public virtual Role Role { get; private set; }

    private UserRole() { }

    public UserRole(User user, Role role)
    {
        User = user ?? throw new DomainException("User cannot be null");
        Role = role ?? throw new DomainException("Role cannot be null");
        UserId = user.Id;
        RoleId = role.Id;
    }
}