using System.Collections.Generic;

namespace CRMData.Entities
{
    public sealed class DUserType
    {
        public int Id { get; set; }

        public string TypeName { get; set; }

        public IList<User> Users { get; set; }
    }
}
