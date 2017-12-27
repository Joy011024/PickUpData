namespace FluorineFx.IO.Bytecode.Lightweight
{
    using FluorineFx.AMF3;
    using FluorineFx.IO;
    using System;
    using System.Runtime.CompilerServices;

    internal delegate object ReadDataInvoker(AMFReader reader, ClassDefinition classDefinition);
}

