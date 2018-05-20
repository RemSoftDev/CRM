using System.Collections.Generic;

namespace CRM.DAL.Entities
{
    public class GridProfile
    {
        public int Id { get; set; }

        public string ProfileName { get; set; }

        public int DGridId { get; set; }
        public DGrid DGrid { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public bool IsDefault { get; set; }

        public string SearchValue { get; set; }
        public string SearchField { get; set; }

        public IList<GridField> GridFields { get; set; }
    }
}
