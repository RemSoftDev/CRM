using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMData.Entities
{
    public class GridField
    {
        public int Id { get; set; }

        public string ColumnName { get; set; }

        public int Order { get; set; }

        public int GridOrderDirection { get; set; }

        public int GridProfileId { get; set; }
        public GridProfile GridProfile { get; set; }

        public bool IsActive { get; set; }
    }
}
