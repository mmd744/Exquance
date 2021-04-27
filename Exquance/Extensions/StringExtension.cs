using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exquance.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Removes all white spaces from argument value.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string RemoveAllWhiteSpaces(this string val)
        {
            if (string.IsNullOrEmpty(val)) 
                return null;

            return val.Replace(" ", string.Empty);
        }
    }
}
