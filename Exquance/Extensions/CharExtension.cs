using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exquance.Extensions
{
    public static class CharExtension
    {
        /// <summary>
        /// Indicates whether passed argument is valid math action or not.
        /// Note: '|' is action indicating end of formula.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static bool IsValidAction(this char ch)
        {
            return ch == '*' || ch == '/' || ch == '+' || ch == '-' || ch == '^' || ch == '|';
        }
    }
}
