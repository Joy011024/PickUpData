namespace FluorineFx.Expression
{
    using System;
    using System.Reflection.Emit;

    internal interface IExpressionGenerator
    {
        void Emit(ILGenerator ilg);
    }
}

