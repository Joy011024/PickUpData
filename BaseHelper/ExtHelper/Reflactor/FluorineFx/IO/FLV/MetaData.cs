namespace FluorineFx.IO.FLV
{
    using System;
    using System.Collections;

    public class MetaData : Hashtable
    {
        private FluorineFx.IO.FLV.MetaCue[] _cuePoints;

        public void PutAll(IDictionary data)
        {
            foreach (DictionaryEntry entry in data)
            {
                this.Add(entry.Key, entry.Value);
            }
        }

        public int AudioCodecId
        {
            get
            {
                return (int) this["audiocodecid"];
            }
            set
            {
                this["audiocodecid"] = value;
            }
        }

        public bool CanSeekToEnd
        {
            get
            {
                return (bool) this["canSeekToEnd"];
            }
            set
            {
                this["canSeekToEnd"] = value;
            }
        }

        public double Duration
        {
            get
            {
                return (double) this["duration"];
            }
            set
            {
                this["duration"] = value;
            }
        }

        public double FrameRate
        {
            get
            {
                return (double) this["framerate"];
            }
            set
            {
                this["framerate"] = value;
            }
        }

        public int Height
        {
            get
            {
                return (int) this["height"];
            }
            set
            {
                this["height"] = value;
            }
        }

        public FluorineFx.IO.FLV.MetaCue[] MetaCue
        {
            get
            {
                return this._cuePoints;
            }
            set
            {
                Hashtable hashtable = new Hashtable();
                this._cuePoints = value;
                int index = 0;
                for (index = 0; index < this._cuePoints.Length; index++)
                {
                    hashtable.Add(index.ToString(), this._cuePoints[index]);
                }
                this["cuePoints"] = hashtable;
            }
        }

        public int VideoCodecId
        {
            get
            {
                return (int) this["videocodecid"];
            }
            set
            {
                this["videocodecid"] = value;
            }
        }

        public int VideoDataRate
        {
            get
            {
                return (int) this["videodatarate"];
            }
            set
            {
                this["videodatarate"] = value;
            }
        }

        public int Width
        {
            get
            {
                return (int) this["width"];
            }
            set
            {
                this["width"] = value;
            }
        }
    }
}

