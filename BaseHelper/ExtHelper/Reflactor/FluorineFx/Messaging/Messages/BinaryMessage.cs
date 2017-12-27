namespace FluorineFx.Messaging.Messages
{
    using FluorineFx.Messaging.Api;
    using System;

    internal class BinaryMessage : MessageBase
    {
        private byte[] _lastPattern = DestinationClientGuidPattern;
        public const string DestinationClientGuid = "05537234-9CF3-4907-8DD0-CD50C28C8409";
        public static byte[] DestinationClientGuidPattern = new UTF8Encoding().GetBytes("05537234-9CF3-4907-8DD0-CD50C28C8409");

        private static int Scan(byte[] value, byte[] find, int start)
        {
            bool flag = false;
            for (int i = start; i <= (value.Length - find.Length); i++)
            {
                flag = true;
                for (int j = 0; j < find.Length; j++)
                {
                    if (find[j] != value[i + j])
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    return i;
                }
            }
            return -1;
        }

        public void Update(IMessageClient messageClient)
        {
            byte[] body = base.body as byte[];
            byte[] binaryId = messageClient.GetBinaryId();
            for (int i = Scan(body, this._lastPattern, 0); i != -1; i = Scan(body, this._lastPattern, 0))
            {
                Array.Copy(binaryId, 0, body, i, binaryId.Length);
            }
            this._lastPattern = binaryId;
        }
    }
}

