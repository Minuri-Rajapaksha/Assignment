﻿using System;
using System.Collections.Generic;

namespace Shared.Model.DB.Identity
{
    public partial class ClientIdPrestrictions
    {
        public int Id { get; set; }
        public string Provider { get; set; }
        public int ClientId { get; set; }

        public Clients Client { get; set; }
    }
}
