namespace FluorineFx.IO.FLV
{
    using FluorineFx.IO;
    using System;
    using System.Collections;

    [CLSCompliant(false)]
    public interface IFlv : IStreamableFile
    {
        void FlushHeaders();
        ITagReader ReaderFromNearestKeyFrame(int seekPoint);
        void RefreshHeaders();
        ITagWriter WriterFromNearestKeyFrame(int seekPoint);

        bool HasKeyFrameData { get; }

        bool HasMetaData { get; }

        Hashtable KeyFrameData { get; set; }

        FluorineFx.IO.FLV.MetaData MetaData { get; set; }

        FluorineFx.IO.FLV.MetaService MetaService { get; set; }
    }
}

