using Exquance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exquance.Services.Abstract
{
    public interface ICellProcessor
    {
        List<Cell> CreateList(string formula);
        bool CanMergeCells(Cell leftCell, Cell rightCell);
        void MergeCells(Cell leftCell, Cell rightCell);
    }
}
