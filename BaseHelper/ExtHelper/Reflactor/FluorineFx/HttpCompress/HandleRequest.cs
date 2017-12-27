namespace FluorineFx.HttpCompress
{
    using System;
    using System.Xml.Serialization;

    public enum HandleRequest
    {
        [XmlEnum(Name="all")]
        All = 0,
        [XmlEnum(Name="amf")]
        Amf = 1,
        [XmlEnum(Name="none")]
        None = -1,
        [XmlEnum(Name="swx")]
        Swx = 2
    }
}

