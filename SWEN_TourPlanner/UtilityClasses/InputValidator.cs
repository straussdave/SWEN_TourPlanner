using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SWEN_TourPlanner.Utility
{
    internal class InputValidator
    {
        /// <summary>
        /// sanitizes a string using Regex (by MSDN http://msdn.microsoft.com/en-us/library/844skk0h(v=vs.71).aspx)
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns>sanitized string</returns>
        public static string SanitizeString(string strIn)
        {
            // Replace invalid characters with empty strings.
            return Regex.Replace(strIn, @"[^\w\.@-]", "");
        }
    }
}
