using CRM.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class AddressViewModel
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public int PostCode { get; set; }
        public string Country { get; set; }
        public AddressType Type { get; set; }
    }
}