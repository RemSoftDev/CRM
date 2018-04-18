using CRM.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class PhoneViewModel
    {
        public string PhoneNumber { get; set; }
        public PhoneType Type { get; set; }
    }
}