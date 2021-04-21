﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exquance.Services.Abstract
{
    public interface IFormulaEvaluator
    {
        int EvaluateExpression(string formula);
    }
}