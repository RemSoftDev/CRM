using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMData.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public int PostCode { get; set; }
        public string Country { get; set; }
        public int Type { get; set; }

        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }
    }
}
