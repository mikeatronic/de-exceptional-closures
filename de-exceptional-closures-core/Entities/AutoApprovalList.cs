﻿using System;
using System.Collections.Generic;
using System.Text;

namespace de_exceptional_closures_core.Entities
{
    public class AutoApprovalList : BaseEntity<int>
    {
        public string Email { get; set; }
    }
}