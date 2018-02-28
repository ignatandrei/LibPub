using System;

namespace LibQRDAL.Models
{
    partial class SimpleUser
    {
        public SimpleUser()
        {
            this.DateAdd = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        }
    }
}
