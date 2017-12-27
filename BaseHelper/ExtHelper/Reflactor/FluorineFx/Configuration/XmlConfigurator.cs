namespace FluorineFx.Configuration
{
    using log4net;
    using System;
    using System.Configuration;
    using System.Xml;
    using System.Xml.Serialization;

    internal sealed class XmlConfigurator : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            ILog logger = null;
            try
            {
                logger = LogManager.GetLogger(typeof(XmlConfigurator));
            }
            catch
            {
            }
            object obj2 = null;
            if (section != null)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(FluorineSettings));
                XmlNodeReader xmlReader = new XmlNodeReader(section);
                try
                {
                    obj2 = serializer.Deserialize(xmlReader);
                }
                catch (Exception exception)
                {
                    if ((logger != null) && logger.get_IsErrorEnabled())
                    {
                        logger.Error(exception.Message, exception);
                    }
                }
                finally
                {
                    serializer = null;
                }
            }
            return obj2;
        }
    }
}

