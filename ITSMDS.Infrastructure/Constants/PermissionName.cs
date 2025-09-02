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
        [PermissionComment("show user list")]
        public const string USER_READ = "user.list";
        public const string USER_CREATE = "user.create";
        public const string USER_EDIT = "user.edit";
        public const string USER_DELETE = "user.delete";


    }
}
