using System;
using System.Collections.Generic;

namespace Shared.Model.DB.Application
{
    public partial class Roles
    {
        public Roles()
        {
            RolePermission = new HashSet<RolePermission>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Discription { get; set; }

        public ICollection<RolePermission> RolePermission { get; set; }
    }
}
