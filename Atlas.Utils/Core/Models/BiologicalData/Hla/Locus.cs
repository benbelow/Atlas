﻿using System;

namespace Atlas.Utils.Core.Models
{
    [Obsolete("Locus is deprecated, please use Antigen instead.")]
    public class Locus
    {
        public int? Id { get; set; }
        public string HlaString { get; set; }
        public string NmdpString { get; set; }
    }
}
