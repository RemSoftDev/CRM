using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class SearchModel
    {
        public string TableName { get; set; }
        [Required]
        public string Field { get; set; }
        public string SearchValue { get; set; }
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public string OrderField { get; set; }
        [Required]
        public string OrderDirection { get; set; }

        public SearchModel()
        {
            this.Page = 1;
            this.ItemsPerPage = 25;
            this.OrderDirection = "ASC";
        }
    }
}