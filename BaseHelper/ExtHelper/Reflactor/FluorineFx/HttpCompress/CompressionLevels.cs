namespace FluorineFx.HttpCompress
{
    using System;
    using System.Xml.Serialization;

    public enum CompressionLevels
    {
        [XmlEnum(Name="default")]
        Default = -1,
        [XmlEnum(Name="high")]
        High = 7,
        [XmlEnum(Name="higher")]
        Higher = 8,
        [XmlEnum(Name="highest")]
        Highest = 9,
        [XmlEnum(Name="less")]
        Less = 4,
        [XmlEnum(Name="low")]
        Low = 3,
        [XmlEnum(Name="lower")]
        Lower = 2,
        [XmlEnum(Name="lowest")]
        Lowest = 1,
        [XmlEnum(Name="more")]
        More = 6,
        [XmlEnum(Name="none")]
        None = 0,
        [XmlEnum(Name="normal")]
        Normal = 5
    }
}

