using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAdAuth.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
    }
}
