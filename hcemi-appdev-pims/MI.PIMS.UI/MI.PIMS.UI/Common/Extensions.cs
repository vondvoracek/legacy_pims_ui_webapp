using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Common
{
    public static class Extensions
    {
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

        public static string GetEnumDescription(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : enumValue.ToString();
        }
    }
}
