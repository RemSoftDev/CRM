using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMData.Entities
{
    public class User
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
    }
}