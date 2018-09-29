using System;
using System.Collections.Generic;

namespace Shared.Model.DB.Identity
{
    public partial class ApiSecrets
    {
        public DateTime? Expiration { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public int ApiResourceId { get; set; }

        public ApiResources ApiResource { get; set; }
    }
}
