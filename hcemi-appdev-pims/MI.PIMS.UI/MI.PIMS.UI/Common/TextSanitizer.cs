using System.Text.RegularExpressions;
using System;

namespace MI.PIMS.BL.Common
{
    public class TextSanitizer
    {
        public static string Sanitize(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            // Remove potentially harmful characters
            string sanitized = Regex.Replace(input, @"[^\w\s]", string.Empty);

            // Optionally, you can also escape certain characters
            sanitized = sanitized.Replace("<", "&lt;").Replace(">", "&gt;");

            return sanitized;
        }
    }
}
