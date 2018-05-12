﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class SearchViewModel
    {
        public string TableName { get; set; }
        public string Field { get; set; }
        public string SearchValue { get; set; }
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public string OrderField { get; set; }
        public string OrderDirection { get; set; }
        public List<LeadViewModel> Items { get; set; }
        public Dictionary<string, bool> Columns { get; set; }

        public SearchViewModel()
        {
            this.Page = 1;
            this.ItemsPerPage = 25;
            this.OrderDirection = "ASC";
            this.Items = new List<LeadViewModel>();
            this.Columns = new Dictionary<string, bool>();
        }
    }
}