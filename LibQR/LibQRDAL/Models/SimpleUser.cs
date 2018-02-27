using System;
using System.Collections.Generic;

namespace LibQRDAL.Models
{
    public partial class SimpleUser
    {
        public long Iduser { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool ConfirmedByEmail { get; set; }
        public bool IsAdmin { get; set; }
    }
}
