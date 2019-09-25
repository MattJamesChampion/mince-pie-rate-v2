﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace MincePieRateV2.Models.Metadata
{
    public class ModelMetadata
    {
        public Guid CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
    }
}
