using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class GridProfileViewModel
    {
        public int Id { get; set; }
        public string ProfileName { get; set; }
        public string SearchValue { get; set; }
        public string SearchField { get; set; }
        public bool IsDefault { get; set; }

        public List<GridFieldViewModel> GridFields { get; set; }

        public GridProfileViewModel()
        {
            this.GridFields = new List<GridFieldViewModel>();
        }
    }
}