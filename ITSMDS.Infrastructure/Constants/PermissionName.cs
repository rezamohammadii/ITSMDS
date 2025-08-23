using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITSMDS.Infrastructure.CustomAttribute;

namespace ITSMDS.Infrastructure.Constants
{
    public static class PermissionName
    {
        [PermissionComment("show admin list")]
        public const string ADMIN_VIEW_LIST = "admin.view.list";
        public const string ADMIN_NEW_CREATE = "admin.new.create";
        public const string ADMIN_NEW_EDIT = "admin.new.edit";
        public const string ADMIN_DELETE = "admin.new.delete";
    }
}
