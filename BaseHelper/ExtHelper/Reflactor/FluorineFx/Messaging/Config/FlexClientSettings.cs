namespace FluorineFx.Messaging.Config
{
    using System;
    using System.Xml;

    public sealed class FlexClientSettings
    {
        private int _timeoutMinutes;

        internal FlexClientSettings()
        {
            this._timeoutMinutes = 0;
        }

        internal FlexClientSettings(XmlNode flexClientNode)
        {
            this._timeoutMinutes = 0;
            XmlNode node = flexClientNode.SelectSingleNode("timeout-minutes");
            if (node != null)
            {
                this._timeoutMinutes = Convert.ToInt32(node.InnerXml);
            }
        }

        public int TimeoutMinutes
        {
            get
            {
                return this._timeoutMinutes;
            }
            set
            {
                this._timeoutMinutes = value;
            }
        }
    }
}

