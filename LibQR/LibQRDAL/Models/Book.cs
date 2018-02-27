using System;
using System.Collections.Generic;

namespace LibQRDAL.Models
{
    public partial class Book
    {
        public long Idbook { get; set; }
        public string IdtinRead { get; set; }
        public string Title { get; set; }
        public string Creator { get; set; }
        public string Identifier { get; set; }
        public bool IsCorrect { get; set; }
        public string ErrorMessage { get; set; }
    }
}
