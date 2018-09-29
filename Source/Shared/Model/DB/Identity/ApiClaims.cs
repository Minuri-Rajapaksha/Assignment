using System;
using System.Collections.Generic;

namespace Shared.Model.DB.Identity
{
    public partial class ApiClaims
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int ApiResourceId { get; set; }

        public ApiResources ApiResource { get; set; }
    }
}
