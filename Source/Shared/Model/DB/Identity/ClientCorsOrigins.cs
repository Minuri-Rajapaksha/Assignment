﻿using System;
using System.Collections.Generic;

namespace Shared.Model.DB.Identity
{
    public partial class ClientCorsOrigins
    {
        public int Id { get; set; }
        public string Origin { get; set; }
        public int ClientId { get; set; }

        public Clients Client { get; set; }
    }
}
