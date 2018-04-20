using CRMData.Contexts;
using CRMData.Entities;
using System.Collections.Generic;
using System.Linq;

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
                    .AddWhere(whereField, searchValue)
                    .AddOrder(byName, byEmail, byPhone, isAscending)
                    .ToList();
            }
        }
    }

    public static class QueryLeadExtentions
    {
        public static IQueryable<Lead> AddWhere(
            this IQueryable<Lead> query, 
            string whereField, 
            string searchValue)
        {
            if (query == null || string.IsNullOrEmpty(whereField) || string.IsNullOrEmpty(searchValue))
                return query;

            if (whereField.Equals("Name"))
                query = query.Where(e => e.Name.Contains(searchValue));
            else if (whereField.Equals("Email"))
                query = query.Where(e => e.Email.Contains(searchValue));
            else if (whereField.Equals("Phone"))
                query = query.Where(e => e.Phones.Where(p => p.PhoneNumber.Contains(searchValue)).Any());

            return query;
        }

        public static IQueryable<Lead> AddOrder(
            this IQueryable<Lead> query, 
            bool? byName = true, 
            bool? byEmail = false, 
            bool? byPhone = false,
            bool isAscending = true)
        {
            if (query == null)
                return query;

            if (byName == true)
                query = isAscending ? query.OrderBy(e => e.Name) : query.OrderByDescending(e => e.Name);
            else if (byEmail == true)
                query = isAscending ? query.OrderBy(e => e.Email) : query.OrderByDescending(e => e.Email);
            if (byPhone == true)
                query = isAscending ? query.OrderBy(e => e.Phones) : query.OrderByDescending(e => e.Phones);

            return query;
        }
    }
}
