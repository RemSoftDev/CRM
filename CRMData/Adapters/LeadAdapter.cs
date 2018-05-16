using CRMData.Contexts;
using CRMData.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using CRMData.Adapters.Extentions;
using System;
using System.Linq.Expressions;

namespace CRMData.Adapters
{
    public sealed class LeadAdapter
    {
        public List<Lead> GetLeadsByFilter(
            string whereField,
            string searchValue,
            string ordered,
            int page,
            int itemsPerPage,
            out int totalRecords,
            string orderDirection)
        {
            using (BaseContext context = new BaseContext())
            {
                bool isAscending = true;
                totalRecords = context
                    .Leads
                    .AddWhere(whereField, searchValue)
                    .Count();

                if (string.IsNullOrWhiteSpace(ordered))
                {
                    ordered = "Id";
                }

                if (!string.IsNullOrWhiteSpace(orderDirection))
                {
                    isAscending = orderDirection.Equals("ASC");
                }

                return context.Leads
                    .Include(e => e.Phones)
                    .Include(l => l.Notes)
                    .AddWhere(whereField, searchValue)
                    .AddOrder(ordered.Trim(), isAscending)
                    .Skip((page - 1) * itemsPerPage)
                    .Take(itemsPerPage)
                    .ToList();                              
            }
        }
    }
}
