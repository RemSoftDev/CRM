using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CRM.Extentions
{
    public static class ObjectExtension
    {
        /// <summary>
        /// Проверяет любое значение на равенство с значением по умолчанию для указанного типа 
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDefault<T>(this T obj)
        {
            return EqualityComparer<T>.Default.Equals(obj, default(T));
        }

        /// <summary>
        /// Выполнит проверку IsDefault, выкинет ошибку если результат TRUE, или вернёт исходное значение если результат FALSE
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ValidateNotDefault<T>(this T obj, string objName)
        {
            bool isDefault = IsDefault(obj);

            if (!isDefault && typeof(T) == typeof(string))
                isDefault = string.IsNullOrWhiteSpace(obj.ToString());

            if (isDefault)
                throw new ArgumentException($"Incoming value for parameter '{objName}' of type '{typeof(T).FullName}' is equal to default '{default(T)}'");

            return obj;
        }

        /// <summary>
        /// Выполняет проверку на null и DBNull
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNull<T>(this T o)
            where T : class
        {
            return o == null || o == DBNull.Value;
        }
    }
}