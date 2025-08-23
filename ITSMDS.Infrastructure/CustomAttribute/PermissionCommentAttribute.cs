

namespace ITSMDS.Infrastructure.CustomAttribute;

public sealed class PermissionCommentAttribute : Attribute
{
    public string Comment { get;  }

    public PermissionCommentAttribute(string comment) => this.Comment = comment;
}
