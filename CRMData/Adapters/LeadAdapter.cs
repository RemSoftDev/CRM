using CRMData.Contexts;
using CRMData.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using CRMData.Adapters.Extentions;
using System;
using System.Linq.Expressions;

namespace CRMData.Adapters
{
    public sealed class LeadAdapter
    {
        public List<Lead> GetLeadsByFilter(
            string whereField,
            string searchValue,
            string ordered,
            bool isAscending = true)
        {
            using (BaseContext context = new BaseContext())
            {
                return context.Leads
                    .Include(e => e.Phones)
                    .AddWhere(whereField, searchValue)
                    .AddOrder(ordered.Trim(), isAscending)
                    .ToList();
            }
        }

        public static Func<Lead, Lead> SelectFieldsExpression(List<string> fields)
        {
            // input parameter "o"
            var xParameter = Expression.Parameter(typeof(Lead), "o");

            // new statement "new Data()"
            var xNew = Expression.New(typeof(Lead));

            // create initializers
            var bindings = fields
                .Select(o =>
                {
                    // property "Field1"
                    var mi = typeof(Lead).GetProperty(o);

                    // original value "o.Field1"
                    var xOriginal = Expression.Property(xParameter, mi);

                    // set value "Field1 = o.Field1"
                    return Expression.Bind(mi, xOriginal);
                }
            );

            // initialization "new Data { Field1 = o.Field1, Field2 = o.Field2 }"
            var xInit = Expression.MemberInit(xNew, bindings);

            // expression "o => new Data { Field1 = o.Field1, Field2 = o.Field2 }"
            var lambda = Expression.Lambda<Func<Lead, Lead>>(xInit, xParameter);

            // compile to Func<Data, Data>
            return lambda.Compile();
        }
    }
}
