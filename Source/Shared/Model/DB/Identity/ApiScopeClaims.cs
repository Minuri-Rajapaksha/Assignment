using System;
using System.Collections.Generic;

namespace Shared.Model.DB.Identity
{
    public partial class ApiScopeClaims
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int ApiScopeId { get; set; }

        public ApiScopes ApiScope { get; set; }
    }
}
