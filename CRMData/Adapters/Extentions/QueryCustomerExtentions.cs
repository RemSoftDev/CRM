using CRMData.Entities;
using System.Linq;

namespace CRMData.Adapters.Extentions
{
    public static class QueryCustomerExtentions
    {
        public static IQueryable<Customer> AddWhere(
            this IQueryable<Customer> query,
            string whereField,
            string searchValue)
        {
            if (query == null || string.IsNullOrEmpty(whereField) || string.IsNullOrEmpty(searchValue))
                return query;

            switch(whereField)
            {
                case "FirstName":
                    return query.Where(e => e.FirstName.Contains(searchValue));
                case "LastName":
                    return query.Where(e => e.LastName.Contains(searchValue));
                case "Email":
                    return query.Where(e => e.Email.Contains(searchValue));
                case "Phone":
                    return query.Where(e => e.Phones.Where(p => p.PhoneNumber.Contains(searchValue)).Any());
                default:
                    return query;
            }
        }

        public static IQueryable<Customer> AddOrder(
            this IQueryable<Customer> query,
            bool? byName = true,
            bool? byEmail = false,
            bool? byPhone = false,
            bool isAscending = true)
        {
            if (query == null)
                return query;

            if (byName == true)
                query = isAscending ? query.OrderBy(e => e.FirstName) : query.OrderByDescending(e => e.FirstName);
            else if (byEmail == true)
                query = isAscending ? query.OrderBy(e => e.Email) : query.OrderByDescending(e => e.Email);
            else if (byPhone == true)
                query = isAscending ? query.OrderBy(e => e.Phones.FirstOrDefault(p => p.TypeId == 0).PhoneNumber) : query.OrderByDescending(e => e.Phones.FirstOrDefault(p => p.TypeId == 0).PhoneNumber);

            return query;
        }
    }
}
