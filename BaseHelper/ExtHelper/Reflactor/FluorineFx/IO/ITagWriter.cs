namespace FluorineFx.IO
{
    using FluorineFx.Util;
    using System;

    [CLSCompliant(false)]
    public interface ITagWriter
    {
        void Close();
        void WriteHeader();
        bool WriteStream(byte[] buffer);
        bool WriteTag(ITag tag);
        bool WriteTag(byte type, ByteBuffer data);

        long BytesWritten { get; }

        IStreamableFile File { get; }

        long Position { get; }
    }
}

