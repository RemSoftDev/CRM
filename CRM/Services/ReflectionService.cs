using CRM.Attributes;
using CRM.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Services
{
    public static class ReflectionService
    {
        public static List<string> GetModelProperties<T>()
        {
            List<string> fieldsList = new List<string>();
            var fields = typeof(T).GetProperties();
            foreach (var property in fields)
            {
                bool? show = (property.GetCustomAttributes(typeof(GridAttribute), true)
                    .FirstOrDefault() as GridAttribute)?.ShowOnGrid;
                if (show.HasValue && show.Value)
                {
                    fieldsList.Add(property.Name);
                }
            }
            return fieldsList;
        }

        public static object GetPropValue<T>(T model, string propName)
        {
            var value = model.GetType().GetProperty(propName).GetValue(model, null);
            if (value is ICollection)
            {
                if ((value as ICollection).Count > 0)
                {
                    var fields = GetModelProperties<PhoneViewModel>();
                    var enumerator = (value as IEnumerable).GetEnumerator();
                    enumerator.MoveNext();

                    return GetPropValue(enumerator.Current, fields.FirstOrDefault());
                }
                return null;
            }
            return value;
        }
    }
}