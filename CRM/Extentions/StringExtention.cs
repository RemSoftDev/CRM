using System;

namespace CRM.Extentions
{
    public static class StringExtention
    {
        /// <summary>
        /// Проверяет что строка не null, пустая или заполнена пробелами. Выкидывает ошибку если условия нарушаются
        /// </summary>
        public static string ValidateNotEmpty(this string obj, string name)
        {
            if (string.IsNullOrWhiteSpace(obj))
                throw new ArgumentException($"incoming string '{name}' can't be null, empty or white space");

            return obj;
        }
    }
}