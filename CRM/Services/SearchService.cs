using CRM.Models;
using CRMData.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Services
{
    public static class SearchService<T>
    {
        public static List<T> Search(SearchViewModel model)
        {
            string orderClause = "";
            string whereClause = "";
            if (!string.IsNullOrEmpty(model.OrderField))
            {
                orderClause = $"ORDER BY {model.OrderField} {model.OrderDirection}";
            }
            if (!string.IsNullOrEmpty(model.SearchValue))
            {
                whereClause = $"WHERE {model.Field} LIKE '%{model.SearchValue}%'";
            }
            List<T> items;
            using (BaseContext context = new BaseContext())
            {
                items = context.Database.SqlQuery<T>($"SELECT * FROM {model.TableName} {whereClause} {orderClause}").ToList();
            }
            return items;
        }
    }
}