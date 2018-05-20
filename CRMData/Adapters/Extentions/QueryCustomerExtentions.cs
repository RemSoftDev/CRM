using CRM.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CRM.DAL.Adapters.Extentions
{
    public static class QueryUserExtentions
    {
        private const string firstName = "First Name";
        private const string lastName = "Last Name";
        private const string email = "Email";
        private const string phone = "Phones";

        public static IQueryable<User> AddWhere(
            this IQueryable<User> query,
            string whereField,
            string searchValue)
        {
            if (query == null || string.IsNullOrEmpty(whereField) || string.IsNullOrEmpty(searchValue))
                return query;

            switch(whereField)
            {               
                case phone:
                    return query.Where(e => e.Phones.Where(p => p.PhoneNumber.Contains(searchValue)).Any());
                default:
                    return query.Where(GetWhereExpression<User>(whereField, searchValue)); ;
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
                case phone:
                    return isAscending ? query.OrderBy(e => e.Phones.FirstOrDefault(p => p.TypeId == 0).PhoneNumber) : query.OrderByDescending(e => e.Phones.FirstOrDefault(p => p.TypeId == 0).PhoneNumber);
                default:
                    return isAscending ? query.OrderBy(orderField) : query.OrderByDescending(orderField);
            }
        }

        public static Expression<Func<TSource, bool>> GetWhereExpression<TSource>(
            string propertyName, string searchvalue)
        {
            var entityType = typeof(TSource);

            var propertyInfo = entityType.GetProperty(propertyName);
            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = Expression.Property(arg, propertyName);

            var searchExp = Expression.Constant(searchvalue);

            var containsExp = Expression.Call(property, typeof(String)
                .GetMethod("Contains"), searchExp);

            var equal = Expression.Equal(
                containsExp,
                Expression.Constant(true));

            return Expression.Lambda<Func<TSource, bool>>(equal, arg);
        }
    }
}
