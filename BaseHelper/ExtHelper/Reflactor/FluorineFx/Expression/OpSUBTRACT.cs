namespace FluorineFx.Expression
{
    using FluorineFx.Util;
    using System;

    internal class OpSUBTRACT : BinaryOperator
    {
        protected override object Evaluate(object context, BaseNode.EvaluationContext evalContext)
        {
            object number = base.Left.EvaluateInternal(context, evalContext);
            object obj3 = base.Right.EvaluateInternal(context, evalContext);
            if (NumberUtils.IsNumber(number) && NumberUtils.IsNumber(obj3))
            {
                return NumberUtils.Subtract(number, obj3);
            }
            if ((number is DateTime) && (((obj3 is TimeSpan) || (obj3 is string)) || NumberUtils.IsNumber(obj3)))
            {
                if (NumberUtils.IsNumber(obj3))
                {
                    obj3 = TimeSpan.FromDays(Convert.ToDouble(obj3));
                }
                else if (obj3 is string)
                {
                    obj3 = TimeSpan.Parse((string) obj3);
                }
                return (((DateTime) number) - ((TimeSpan) obj3));
            }
            if (!(number is DateTime) || !(obj3 is DateTime))
            {
                throw new ArgumentException("Cannot subtract instances of '" + number.GetType().FullName + "' and '" + obj3.GetType().FullName + "'.");
            }
            return (TimeSpan) (((DateTime) number) - ((DateTime) obj3));
        }
    }
}

