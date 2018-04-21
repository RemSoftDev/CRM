﻿using CRMData.Contexts;
using CRMData.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using CRMData.Adapters.Extentions;

namespace CRMData.Adapters
{
    public sealed class LeadAdapter
    {
        public List<Lead> GetLeadsByFilter(
            string whereField, 
            string searchValue, 
            bool? byName = true,
            bool? byEmail = null,
            bool? byPhone = null,
            bool isAscending = true)
        {
            using (BaseContext context = new BaseContext())
            {
                return context.Leads
                    .Include(e => e.Phones)
                    .AddWhere(whereField, searchValue)
                    .AddOrder(byName, byEmail, byPhone, isAscending)
                    .ToList();
            }
        }
    }
}
