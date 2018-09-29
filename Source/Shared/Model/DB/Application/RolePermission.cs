using System;
using System.Collections.Generic;

namespace Shared.Model.DB.Application
{
    public partial class RolePermission
    {
        public int RolePermissionId { get; set; }
        public int PermissionId { get; set; }
        public int RoleId { get; set; }

        public Permission Permission { get; set; }
        public Roles Role { get; set; }
    }
}
