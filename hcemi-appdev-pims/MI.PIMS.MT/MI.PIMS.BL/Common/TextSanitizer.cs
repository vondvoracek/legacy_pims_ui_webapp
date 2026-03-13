using System.Text.RegularExpressions;
using System;

namespace MI.PIMS.UI.Common
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

        public static void Main()
        {
            string userInput = "<script>alert('Hello!');</script>";
            string safeInput = Sanitize(userInput);
            Console.WriteLine(safeInput); // Output: scriptalertHelloscript
        }
    }
}
