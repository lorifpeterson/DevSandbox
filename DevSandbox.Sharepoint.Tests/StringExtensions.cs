using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSandbox.Sharepoint.Tests
{
    public static class StringExtensions
    {
        public static string UpdateVariables(this string value, DateTime date)
        {
            return value?
                   .Replace("<FULLMONTH>", date.ToString("MMMM"))
                   .Replace("<YEAR>", date.Year.ToString())
                   ?? string.Empty;
        }

        public static string ToSearchFolder(this string path)
        {
            return path?.Split('/')
                .Where(str => !string.IsNullOrWhiteSpace(str))
                .LastOrDefault()?.Trim()
                ?? string.Empty;
        }

        public static string ToFileUrl(this string serverRelativeUrl, string searchFolder, string sharepointFileName)
        {
            return string.IsNullOrWhiteSpace(searchFolder)
                                ? $"{serverRelativeUrl}/{sharepointFileName}"
                                : $"{serverRelativeUrl}/{searchFolder}/{sharepointFileName}";
        }

    }
}
