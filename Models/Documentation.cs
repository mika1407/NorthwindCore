using System;
using System.Collections.Generic;

namespace NorthwindCore.Models
{
    public partial class Documentation
    {
        public int DocumentationId { get; set; }
        public string AvailableRoute { get; set; }
        public string Method { get; set; }
        public string Description { get; set; }
        public string Keycode { get; set; }
    }
}
