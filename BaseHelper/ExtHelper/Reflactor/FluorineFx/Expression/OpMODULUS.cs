﻿namespace FluorineFx.Expression
{
    using FluorineFx.Util;
    using System;

    internal class OpMODULUS : BinaryOperator
    {
        protected override object Evaluate(object context, BaseNode.EvaluationContext evalContext)
        {
            object number = base.Left.EvaluateInternal(context, evalContext);
            object obj3 = base.Right.EvaluateInternal(context, evalContext);
            if (!NumberUtils.IsNumber(number) || !NumberUtils.IsNumber(obj3))
            {
                throw new ArgumentException("Cannot calculate modulus for instances of '" + number.GetType().FullName + "' and '" + obj3.GetType().FullName + "'.");
            }
            return NumberUtils.Modulus(number, obj3);
        }
    }
}

