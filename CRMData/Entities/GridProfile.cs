using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMData.Entities
{
    public class GridProfile
    {
        public int Id { get; set; }

        public int DGridId { get; set; }
        public DGrid DGrid { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public bool IsDefault { get; set; }

        public IList<GridField> GridFields { get; set; }
    }
}
