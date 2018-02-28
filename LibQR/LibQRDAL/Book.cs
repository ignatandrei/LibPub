using System;
using System.Collections.Generic;
using System.Text;

namespace LibQRDAL.Models
{
    partial class Book
    {
        public Book()
        {
            this.DateAdd = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        }
    }
}
