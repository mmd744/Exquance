using Exquance.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exquance.Services.Implementation
{
    public class ExpressionEvaluator : IExpressionEvaluator
    {
        private readonly ICellProcessor _cellProcessor;

        public ExpressionEvaluator()
        {
            _cellProcessor = new CellProcessor();
        }

        public int EvaluateExpression(string finalExpression)
        {
            var cells = _cellProcessor.CreateList(finalExpression);
            if (cells.Count > 1)
            {
                for (int i = 0; i < cells.Count - 1;)
                {
                    if (_cellProcessor.CanMergeCells(cells[i], cells[i + 1]))
                    {
                        _cellProcessor.MergeCells(cells[i], cells[i + 1]);
                        cells.RemoveAt(i + 1);
                        i = 0;
                    }
                    else
                    {
                        i++;
                        continue;
                    }
                }
                return cells.First().Value;
            }
            return 0;
        }
    }
}
