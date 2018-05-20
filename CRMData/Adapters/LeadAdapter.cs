﻿using CRM.DAL.Contexts;
using CRM.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using CRM.DAL.Adapters.Extentions;

namespace CRM.DAL.Adapters
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
                    .Where(l => !l.IsConverted)
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

                var skipItems = (page - 1) * itemsPerPage;
                if(skipItems > totalRecords)
                {
                    skipItems = totalRecords / itemsPerPage;
                }

                return context.Leads
                    .Include(e => e.Phones)
                    .Include(l => l.Notes)
                    .Where(l => !l.IsConverted)
                    .AddWhere(whereField, searchValue)
                    .AddOrder(ordered.Trim(), isAscending)
                    .Skip(skipItems)
                    .Take(itemsPerPage)
                    .ToList();
            }
        }

        public List<GridProfile> GetUserProfiles(string userEmail, string profileName = "")
        {
            using (var context = new BaseContext())
            {
                var query = context
                   .GridProfiles
                   .Include(p => p.GridFields)
                   .Where(p => p
                        .DGrid
                            .Type
                            .Equals("Lead") &&
                        p.User
                            .Email
                            .Equals(userEmail));

                if (!string.IsNullOrEmpty(profileName))
                {
                    query = query
                        .Where(p => p
                            .ProfileName
                            .Equals(profileName));
                }

                return query.OrderBy(i => i.Id).ToList();
            }
        }
    }
}
