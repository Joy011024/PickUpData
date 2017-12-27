namespace FluorineFx.IO
{
    using System;

    [CLSCompliant(false)]
    public interface ITag
    {
        byte[] Body { get; set; }

        int BodySize { get; }

        byte DataType { get; set; }

        int PreviousTagSize { get; set; }

        int Timestamp { get; set; }
    }
}

