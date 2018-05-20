using CRM.Models;
using CRM.DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace CRM.Services
{
    public static class SearchService<TEntity> where TEntity : class
    {
        /// <summary>
        /// DEPRICATED
        /// </summary>
        public static List<TEntity> Search(SearchViewModel model)
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
            List<TEntity> items;
            using (BaseContext context = new BaseContext())
            {
                items = context.Database.SqlQuery<TEntity>($"SELECT * FROM {model.TableName} {whereClause} {orderClause}").ToList();
            }
            return items;
        }
    }
}