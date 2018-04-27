using CRMData.Contexts;
using CRMData.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using CRMData.Adapters.Extentions;

namespace CRMData.Adapters
{
    public sealed class UserAdapter
    {
        public List<User> GetUsersByFilter(
            string whereField,
            string searchValue,
            string ordered,
            int userTypeId,
            bool isAscending = true)
        {
            using (BaseContext context = new BaseContext())
            {
                return context.Users
                    .Include(e => e.Phones)
                    .Where(e => e.UserTypeId == userTypeId)
                    .AddWhere(whereField.Trim(), searchValue)
                    .AddOrder(ordered.Trim(), isAscending)
                    .ToList();
            }
        }
    }
}
