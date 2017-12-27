namespace FluorineFx.Messaging.Services.Messaging
{
    using FluorineFx;
    using FluorineFx.Messaging.Config;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Messaging.Services;
    using log4net;
    using System;
    using System.Collections;
    using System.Messaging;
    using System.Reflection;
    using System.Runtime.Serialization;

    internal class MsmqAdapter : ServiceAdapter
    {
        private Hashtable _consumers;
        private IMessageFormatter _messageFormatter;
        private MessageQueue _messageQueue;
        private MsmqSettings _msmqSettings;
        private Hashtable _producers;
        private static readonly ILog log = LogManager.GetLogger(typeof(MsmqAdapter));

        private AsyncMessage ConvertMsmqMessage(Message message)
        {
            AsyncMessage message2 = null;
            message2 = new AsyncMessage {
                body = message.Body,
                destination = base.DestinationSettings.Id,
                clientId = Guid.NewGuid().ToString("D"),
                messageId = Guid.NewGuid().ToString("D"),
                timestamp = Environment.TickCount
            };
            message2.headers["MSMQId"] = message.Id;
            message2.headers["MSMQLabel"] = message.Label;
            if (message.Body != null)
            {
                PropertyInfo[] properties = message.Body.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo info in properties)
                {
                    if ((info.GetGetMethod() != null) && (info.GetGetMethod().GetParameters().Length == 0))
                    {
                        message2.headers[info.Name] = info.GetValue(message.Body, null);
                    }
                }
                FieldInfo[] fields = message.Body.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (FieldInfo info2 in fields)
                {
                    message2.headers[info2.Name] = info2.GetValue(message.Body);
                }
            }
            return message2;
        }

        private void EnableReceiving(bool enable)
        {
            log.Debug(__Res.GetString("Msmq_Enable", new object[] { enable }));
            if (enable)
            {
                if (this._messageQueue != null)
                {
                    this._messageQueue.ReceiveCompleted += new ReceiveCompletedEventHandler(this.OnReceiveCompleted);
                    this._messageQueue.BeginReceive(new TimeSpan(1, 0, 0), this._messageQueue);
                }
            }
            else if (this._messageQueue != null)
            {
                this._messageQueue.ReceiveCompleted -= new ReceiveCompletedEventHandler(this.OnReceiveCompleted);
            }
        }

        public override void Init()
        {
            base.Init();
            this._producers = new Hashtable();
            this._consumers = new Hashtable();
            this._msmqSettings = base.DestinationSettings.MsmqSettings;
            if (this._msmqSettings != null)
            {
                MessageQueue.EnableConnectionCache = false;
                log.Debug(__Res.GetString("Msmq_StartQueue", new object[] { this._msmqSettings.Name }));
                this._messageQueue = new MessageQueue(this._msmqSettings.Name);
                string formatter = "BinaryMessageFormatter";
                if ((this._msmqSettings.Formatter != null) && (this._msmqSettings.Formatter != string.Empty))
                {
                    formatter = this._msmqSettings.Formatter;
                }
                log.Debug(__Res.GetString("Msmq_InitFormatter", new object[] { formatter }));
                if (formatter == "BinaryMessageFormatter")
                {
                    this._messageFormatter = new BinaryMessageFormatter();
                    this._messageQueue.Formatter = this._messageFormatter;
                }
                else if (formatter.StartsWith("XmlMessageFormatter"))
                {
                    string[] strArray = formatter.Split(new char[] { ';' });
                    Type[] targetTypes = null;
                    if (strArray.Length == 1)
                    {
                        targetTypes = new Type[] { typeof(string) };
                    }
                    else
                    {
                        targetTypes = new Type[strArray.Length - 1];
                        for (int i = 1; i < strArray.Length; i++)
                        {
                            Type type = ObjectFactory.Locate(strArray[i]);
                            if (type != null)
                            {
                                targetTypes[i - 1] = type;
                            }
                            else
                            {
                                log.Error(__Res.GetString("Type_InitError", new object[] { strArray[i] }));
                            }
                        }
                    }
                    this._messageFormatter = new XmlMessageFormatter(targetTypes);
                    this._messageQueue.Formatter = this._messageFormatter;
                }
                else
                {
                    log.Error(__Res.GetString("Type_InitError", new object[] { formatter }));
                    this._messageQueue.Close();
                    this._messageQueue = null;
                }
            }
            else
            {
                log.Error(__Res.GetString("ServiceAdapter_MissingSettings"));
            }
        }

        public override object Invoke(IMessage message)
        {
            if (this._messageQueue != null)
            {
                log.Debug(__Res.GetString("Msmq_Send", new object[] { message.clientId }));
                Message message2 = new Message {
                    Formatter = this._messageFormatter,
                    Body = message.body
                };
                if ((this._msmqSettings != null) && (this._msmqSettings.Label != null))
                {
                    message2.Label = this._msmqSettings.Label;
                }
                else
                {
                    message2.Label = message.clientId.ToString();
                }
                this._messageQueue.Send(message2);
            }
            return null;
        }

        public override object Manage(CommandMessage commandMessage)
        {
            object obj4;
            object obj2 = base.Manage(commandMessage);
            switch (commandMessage.operation)
            {
                case 0:
                    lock ((obj4 = base.SyncRoot))
                    {
                        bool flag = this._consumers.Count == 0;
                        this._consumers[commandMessage.clientId] = null;
                        if (flag)
                        {
                            this.EnableReceiving(true);
                        }
                    }
                    return obj2;

                case 1:
                    lock ((obj4 = base.SyncRoot))
                    {
                        if (!this._consumers.Contains(commandMessage.clientId))
                        {
                            return obj2;
                        }
                        this._consumers.Remove(commandMessage.clientId);
                        if (this._consumers.Count == 0)
                        {
                            this.EnableReceiving(false);
                        }
                    }
                    return obj2;
            }
            return obj2;
        }

        private void OnReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            Message message = ((MessageQueue) e.AsyncResult.AsyncState).EndReceive(e.AsyncResult);
            log.Debug(__Res.GetString("Msmq_Receive", new object[] { message.Label, message.Id }));
            try
            {
                AsyncMessage message2 = this.ConvertMsmqMessage(message);
                (base.Destination.Service as MessageService).PushMessageToClients(message2);
            }
            catch (SerializationException exception)
            {
                log.Error(__Res.GetString("Msmq_Poison"), exception);
            }
            IAsyncResult result = ((MessageQueue) e.AsyncResult.AsyncState).BeginReceive(new TimeSpan(1, 0, 0), (MessageQueue) e.AsyncResult.AsyncState);
        }

        public override void Stop()
        {
            log.Debug(__Res.GetString("ServiceAdapter_Stop"));
            if (this._messageQueue != null)
            {
                this.EnableReceiving(false);
                this._messageQueue.Close();
            }
            base.Stop();
        }

        public override bool HandlesSubscriptions
        {
            get
            {
                return true;
            }
        }
    }
}

