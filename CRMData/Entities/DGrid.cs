using System.Collections;
using System.Collections.Generic;

namespace CRM.DAL.Entities
{
    public class DGrid
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public IList<GridProfile> GridProfiles { get; set; }
    }
}
