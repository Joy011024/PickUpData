namespace FluorineFx.Messaging.Messages
{
    using System;

    [CLSCompliant(false)]
    public class CommandMessage : AsyncMessage
    {
        private string _messageRefType;
        private int _operation;
        public const string AuthenticationMessageRefType = "flex.messaging.messages.AuthenticationMessage";
        public const int ClientPingOperation = 5;
        public const int ClientSyncOperation = 4;
        public const int ClusterRequestOperation = 7;
        public static string FluorineMessageClientTimeoutHeader = "FluorineMessageClientTimeout";
        internal static string FluorineSuppressPollWaitHeader = "FluorineSuppressPollWait";
        public const int LoginOperation = 8;
        public const int LogoutOperation = 9;
        public const int PollOperation = 2;
        public static string SelectorHeader = "DSSelector";
        public static string SessionInvalidatedHeader = "DSSessionInvalidated";
        public const int SessionInvalidateOperation = 10;
        public const int SubscribeOperation = 0;
        public const int UnknownOperation = 0x2710;
        public const int UnsubscribeOperation = 1;

        public CommandMessage()
        {
            this._operation = 0x2710;
        }

        public CommandMessage(int operation)
        {
            this._operation = operation;
        }

        public string messageRefType
        {
            get
            {
                return this._messageRefType;
            }
            set
            {
                this._messageRefType = value;
            }
        }

        public int operation
        {
            get
            {
                return this._operation;
            }
            set
            {
                this._operation = value;
            }
        }
    }
}

