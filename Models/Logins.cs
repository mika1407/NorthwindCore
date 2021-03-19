using System;
using System.Collections.Generic;

namespace NorthwindCore.Models
{
    public partial class Logins
    {
        public int LoginId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public int AccesslevelId { get; set; }
        public string Token { get; set; }
    }
}
