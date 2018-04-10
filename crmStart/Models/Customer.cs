using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace crmStart.Models
{
    public class Customer
    {
        [Key]
        [Required]
        public int CustomerID { get; set; }

        public int LeadID { get; set; }

        public DateTime Converted { get; set; }

        public int ConvertedBy_UserID { get; set; }
    }
}