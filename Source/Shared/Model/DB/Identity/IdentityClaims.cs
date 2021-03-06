﻿using System;
using System.Collections.Generic;

namespace Shared.Model.DB.Identity
{
    public partial class IdentityClaims
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int IdentityResourceId { get; set; }

        public IdentityResources IdentityResource { get; set; }
    }
}
