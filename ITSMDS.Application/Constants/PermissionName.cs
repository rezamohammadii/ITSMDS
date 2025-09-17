using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITSMDS.Domain.CustomAttribute;

namespace ITSMDS.Application.Constants
{
    public static class PermissionName
    {
        [PermissionComment("show user list")]
        public const string USER_ALL = "user.all";
        public const string USER_READ = "user.read";
        public const string USER_CREATE = "user.create";
        public const string USER_EDIT = "user.edit";
        public const string USER_DELETE = "user.delete";

        public const string ROLE_READ = "role.read";
        public const string ROLE_ALL = "role.all";
        public const string ROLE_CREATE = "role.create";
        public const string ROLE_EDIT = "role.edit";
        public const string ROLE_DELETE = "role.delete";

        public const string SERVICE_READ = "service.read";
        public const string SERVICE_ALL = "service.all";
        public const string SERVICE_CREATE = "service.create";
        public const string SERVICE_EDIT = "service.edit";
        public const string SERVICE_DELETE = "service.delete";

        public const string SERVER_READ = "server.read";
        public const string SERVER_ALL = "server.all";
        public const string SERVER_CREATE = "server.create";
        public const string SERVER_EDIT = "server.edit";
        public const string SERVER_DELETE = "server.delete";

    }
}
