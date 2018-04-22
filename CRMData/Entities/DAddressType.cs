using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRMData.Entities
{
    public sealed class DAddressType
    {
        public int Id { get; set; }

        public string TypeName { get; set; }

        public IList<Address> Addresses { get; set; }
    }
}
