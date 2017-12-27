namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    public enum RemotingServiceAttributeConstraint
    {
        [XmlEnum(Name="access")]
        Access = 2,
        [XmlEnum(Name="browse")]
        Browse = 1
    }
}

