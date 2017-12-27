namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    public enum TimezoneCompensation
    {
        [XmlEnum(Name="auto")]
        Auto = 1,
        [XmlEnum(Name="none")]
        None = 0
    }
}

