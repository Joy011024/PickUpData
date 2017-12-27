namespace FluorineFx.HttpCompress
{
    using System;
    using System.Xml.Serialization;

    public enum Algorithms
    {
        [XmlEnum(Name="default")]
        Default = -1,
        [XmlEnum(Name="deflate")]
        Deflate = 0,
        [XmlEnum(Name="gzip")]
        GZip = 1
    }
}

