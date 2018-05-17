using System.Collections.Generic;

namespace CRM.DAL.Entities
{
    public sealed class DAddressType
    {
        public int Id { get; set; }

        public string TypeName { get; set; }

        public IList<Address> Addresses { get; set; }
    }
}
