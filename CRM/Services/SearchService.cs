using CRM.Models;
using CRMData.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace CRM.Services
{
    public static class SearchService<TEntity> where TEntity : class
    {
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

        public static List<TEntity> Search(SearchViewModel model, Expression<Func<TEntity, bool>> where = null, Expression<Func<TEntity, string>> order = null, bool orderAsc = true)
        {
            using (BaseContext context = ContextFactory.SingleContextFactory.Get<BaseContext>())
            {
                var query = context.Set<TEntity>().AsQueryable();

                if (where != null)
                    query = query.Where(where);
                if (order != null && orderAsc)
                    query = query.OrderBy(order);
                if (order != null && !orderAsc)
                    query = query.OrderByDescending(order);

                return query.ToList();
            }
        }
    }
}