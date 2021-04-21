using Exquance.Extensions;
using Exquance.Models;
using Exquance.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exquance.Services.Implementation
{
    public class CellProcessor : ICellProcessor
    {
        public bool CanMergeCells(Cell leftCell, Cell rightCell)
        {
            return leftCell.Priority >= rightCell.Priority;
        }

        public List<Cell> CreateList(string formula)
        {
            List<Cell> cells = new();
            int digitsCount = 0;
            string trimmedFormula = formula.RemoveAllWhiteSpaces() + '|'; // -3-44-25+2*3/3|
            for (int i = 0; i < trimmedFormula.Length; i++)
            {
                if (i == 0 && trimmedFormula[i] == '-') // add new cell with 0 value and - action
                {
                    cells.Add(new Cell(0, trimmedFormula[i]));
                }
                else if (trimmedFormula[i].IsValidAction())
                {
                    var value = int.Parse(trimmedFormula.Substring(i - digitsCount, digitsCount));
                    cells.Add(new Cell(value, trimmedFormula[i]));
                    digitsCount = 0;
                }
                else
                {
                    digitsCount++;
                }
            }
            if (!cells.Any()) throw new Exception("Wrong formula");
            return cells;
        }

        public void MergeCells(Cell leftCell, Cell rightCell)
        {
            switch (leftCell.Action)
            {
                case '^':
                    leftCell.Value = (int)Math.Pow(leftCell.Value, rightCell.Value);
                    break;
                case '*':
                    leftCell.Value *= rightCell.Value;
                    break;
                case '/':
                    leftCell.Value /= rightCell.Value;
                    break;
                case '+':
                    leftCell.Value += rightCell.Value;
                    break;
                case '-':
                    leftCell.Value -= rightCell.Value;
                    break;
                case '|':
                    break;
            }
            leftCell.Action = rightCell.Action;
        }
    }
}
