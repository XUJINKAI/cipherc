﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CipherTool.Parse
{
    public class GetExpression : ExpressionBase, IExpression
    {
        public override bool IsDataType => false;

        public override void ContinueParse(Parser parser) => throw new NotImplementedException();

    }
}