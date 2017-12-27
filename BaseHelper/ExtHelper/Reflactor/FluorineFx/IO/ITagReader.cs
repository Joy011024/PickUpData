namespace FluorineFx.IO
{
    using System;

    [CLSCompliant(false)]
    public interface ITagReader
    {
        void Close();
        void DecodeHeader();
        bool HasMoreTags();
        bool HasVideo();
        ITag ReadTag();

        long BytesRead { get; }

        long Duration { get; }

        IStreamableFile File { get; }

        int Offset { get; }

        long Position { get; set; }
    }
}

