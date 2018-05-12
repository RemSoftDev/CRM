using System.Collections;
using System.Collections.Generic;

namespace CRMData.Entities
{
    public class DGrid
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public IList<GridProfile> GridProfiles { get; set; }
    }
}
