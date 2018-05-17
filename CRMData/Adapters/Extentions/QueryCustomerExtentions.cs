using CRM.DAL.Entities;
using System.Linq;

namespace CRM.DAL.Adapters.Extentions
{
    public static class QueryUserExtentions
    {
        private const string firstName = "First Name";
        private const string lastName = "Last Name";
        private const string email = "Email";
        private const string phone = "Phone";

        public static IQueryable<User> AddWhere(
            this IQueryable<User> query,
            string whereField,
            string searchValue)
        {
            if (query == null || string.IsNullOrEmpty(whereField) || string.IsNullOrEmpty(searchValue))
                return query;

            switch(whereField)
            {
                case firstName:
                    return query.Where(e => e.FirstName.Contains(searchValue));
                case lastName:
                    return query.Where(e => e.LastName.Contains(searchValue));
                case email:
                    return query.Where(e => e.Email.Contains(searchValue));
                case phone:
                    return query.Where(e => e.Phones.Where(p => p.PhoneNumber.Contains(searchValue)).Any());
                default:
                    return query;
            }
        }

        public static IQueryable<User> AddOrder(
            this IQueryable<User> query,
            string orderField,
            bool isAscending = true)
        {
            if (query == null || string.IsNullOrEmpty(orderField))
                return query;

            switch (orderField)
            {
                case firstName:
                    return isAscending ? query.OrderBy(e => e.FirstName) : query.OrderByDescending(e => e.FirstName);
                case lastName:
                    return isAscending ? query.OrderBy(e => e.FirstName) : query.OrderByDescending(e => e.FirstName);
                case email:
                    return isAscending ? query.OrderBy(e => e.Email) : query.OrderByDescending(e => e.Email);
                case phone:
                    return isAscending ? query.OrderBy(e => e.Phones.FirstOrDefault(p => p.TypeId == 0).PhoneNumber) : query.OrderByDescending(e => e.Phones.FirstOrDefault(p => p.TypeId == 0).PhoneNumber);
                default:
                    return isAscending ? query.OrderBy(e => e.FirstName) : query.OrderByDescending(e => e.FirstName);
            }
        }
    }
}
