namespace FluorineFx.IO
{
    using System;

    [CLSCompliant(false)]
    public interface IStreamableFile
    {
        ITagWriter GetAppendWriter();
        ITagReader GetReader();
        ITagWriter GetWriter();
    }
}

