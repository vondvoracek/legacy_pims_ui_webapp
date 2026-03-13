using System;
using System.Collections.Generic;
using System.Text;

namespace MI.PIMS.BL
{
    public static class Extension
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static string ToStringNullSafe(this object value)
        {
            return (value ?? string.Empty).ToString().Trim();
        }

        /// <summary>
        /// Returns string value if not EMPTY otherwise returns NULL
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToStringOrNull(this object value)
        {
            return string.IsNullOrEmpty((string)value.ToString().Trim()) ? null : value.ToString().Trim();
        }
    }
}
