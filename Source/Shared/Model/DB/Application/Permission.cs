using System;
using System.Collections.Generic;

namespace Shared.Model.DB.Application
{
    public partial class Permission
    {
        public Permission()
        {
            RolePermission = new HashSet<RolePermission>();
        }

        public int PermissionId { get; set; }
        public string Discription { get; set; }

        public ICollection<RolePermission> RolePermission { get; set; }
    }
}
