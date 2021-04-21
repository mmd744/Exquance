using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exquance.Models
{
    public class Cell
    {
        internal Cell(int value, char action)
        {
            Value = value;
            Action = action;
        }

        internal int Value { get; set; }
        internal char Action { get; set; }
        internal int Priority 
        { 
            get
            {
                return this.Action switch
                {
                    '^' => 4,
                    '*' or '/' => 3,
                    '+' or '-' => 2,
                    _ => 0 // default
                };
            } 
        }
    }
}
