namespace FluorineFx.Configuration
{
    using FluorineFx.HttpCompress;
    using System;
    using System.Xml.Serialization;

    public sealed class HttpCompressSettings
    {
        private CompressionLevels _compressionLevel = CompressionLevels.Default;
        private PathEntryCollection _excludedPaths = new PathEntryCollection();
        private MimeTypeEntryCollection _excludedTypes = new MimeTypeEntryCollection();
        private FluorineFx.HttpCompress.HandleRequest _handleRequest = FluorineFx.HttpCompress.HandleRequest.None;
        private Algorithms _preferredAlgorithm = Algorithms.Default;
        private int _threshold = 0x5000;

        public bool IsExcludedMimeType(string mimetype)
        {
            return this._excludedTypes.Contains(mimetype.ToLower());
        }

        public bool IsExcludedPath(string relUrl)
        {
            return this._excludedPaths.Contains(relUrl.ToLower());
        }

        [XmlAttribute(AttributeName="compressionLevel")]
        public CompressionLevels CompressionLevel
        {
            get
            {
                return this._compressionLevel;
            }
            set
            {
                this._compressionLevel = value;
            }
        }

        public static HttpCompressSettings Default
        {
            get
            {
                return new HttpCompressSettings();
            }
        }

        [XmlArray("excludedMimeTypes"), XmlArrayItem("add", typeof(MimeTypeEntry))]
        public MimeTypeEntryCollection ExcludedMimeTypes
        {
            get
            {
                if (this._excludedTypes == null)
                {
                    this._excludedTypes = new MimeTypeEntryCollection();
                }
                return this._excludedTypes;
            }
        }

        [XmlArray("excludedPaths"), XmlArrayItem("add", typeof(PathEntry))]
        public PathEntryCollection ExcludedPaths
        {
            get
            {
                if (this._excludedPaths == null)
                {
                    this._excludedPaths = new PathEntryCollection();
                }
                return this._excludedPaths;
            }
        }

        [XmlAttribute(AttributeName="handleRequest")]
        public FluorineFx.HttpCompress.HandleRequest HandleRequest
        {
            get
            {
                return this._handleRequest;
            }
            set
            {
                this._handleRequest = value;
            }
        }

        [XmlAttribute(AttributeName="preferredAlgorithm")]
        public Algorithms PreferredAlgorithm
        {
            get
            {
                return this._preferredAlgorithm;
            }
            set
            {
                this._preferredAlgorithm = value;
            }
        }

        [XmlElement(ElementName="threshold")]
        public int Threshold
        {
            get
            {
                return this._threshold;
            }
            set
            {
                this._threshold = value;
            }
        }
    }
}

