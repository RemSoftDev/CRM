using CRM.DAL.Entities;
using System.Linq;

namespace CRM.DAL.Adapters.Extentions
{
    public static class QueryLeadExtentions
    {
        private const string name = "Name";
        private const string email = "Email";
        private const string phone = "Phone";

        public static IQueryable<Lead> AddWhere(
            this IQueryable<Lead> query,
            string whereField,
            string searchValue)
        {
            if (query == null || string.IsNullOrEmpty(whereField) || string.IsNullOrEmpty(searchValue))
                return query;

            switch (whereField)
            {
                case name:
                    return query.Where(e => e.Name.Contains(searchValue));
                case email:
                    return query.Where(e => e.Email.Contains(searchValue));
                case phone:
                    return query.Where(e => e.Phones.Where(p => p.PhoneNumber.Contains(searchValue)).Any());
                default:
                    return query;
            }
        }

        public static IQueryable<Lead> AddOrder(
            this IQueryable<Lead> query,
            string orderField,
            bool isAscending = true)
        {
            if (query == null || string.IsNullOrEmpty(orderField))
                return query;

            switch (orderField)
            {
                case name:
                    return isAscending ? query.OrderBy(e => e.Name) : query.OrderByDescending(e => e.Name);
                case email:
                    return isAscending ? query.OrderBy(e => e.Email) : query.OrderByDescending(e => e.Email);
                case phone:
                    return isAscending ? query.OrderBy(e => e.Phones.FirstOrDefault(p => p.TypeId == 0).PhoneNumber) : query.OrderByDescending(e => e.Phones.FirstOrDefault(p => p.TypeId == 0).PhoneNumber);
                default:
                    return isAscending ? query.OrderBy(e => e.Name) : query.OrderByDescending(e => e.Name);
            }

        }
    }
}
