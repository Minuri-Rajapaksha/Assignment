﻿using System;
using System.Collections.Generic;

namespace Shared.Model.DB.Identity
{
    public partial class ClientClaims
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public int ClientId { get; set; }

        public Clients Client { get; set; }
    }
}
