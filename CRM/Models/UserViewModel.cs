using CRM.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class UserViewModel
    {
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public string Password { get; set; }
    }
}