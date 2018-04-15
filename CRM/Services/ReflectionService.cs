using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Services
{
    public static class ReflectionService<T>
    {
        public static List<string> GetModelProperties()
        {
            List<string> fieldsList = new List<string>();
            var fields =  typeof (T).GetProperties();
            foreach(var property in fields)
            {
                fieldsList.Add(property.Name);
            }
            return fieldsList;
        }
    }
}