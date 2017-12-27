namespace FluorineFx.Messaging.Rtmp.SO
{
    using System;

    internal class SharedObjectTypeMapping
    {
        private static SharedObjectEventType[] _typeMap;

        static SharedObjectTypeMapping()
        {
            SharedObjectEventType[] typeArray = new SharedObjectEventType[12];
            typeArray[1] = SharedObjectEventType.SERVER_CONNECT;
            typeArray[2] = SharedObjectEventType.SERVER_DISCONNECT;
            typeArray[3] = SharedObjectEventType.SERVER_SET_ATTRIBUTE;
            typeArray[4] = SharedObjectEventType.CLIENT_UPDATE_DATA;
            typeArray[5] = SharedObjectEventType.CLIENT_UPDATE_ATTRIBUTE;
            typeArray[6] = SharedObjectEventType.SERVER_SEND_MESSAGE;
            typeArray[7] = SharedObjectEventType.CLIENT_STATUS;
            typeArray[8] = SharedObjectEventType.CLIENT_CLEAR_DATA;
            typeArray[9] = SharedObjectEventType.CLIENT_DELETE_DATA;
            typeArray[10] = SharedObjectEventType.SERVER_DELETE_ATTRIBUTE;
            typeArray[11] = SharedObjectEventType.CLIENT_INITIAL_DATA;
            _typeMap = typeArray;
        }

        public static byte ToByte(SharedObjectEventType type)
        {
            switch (type)
            {
                case SharedObjectEventType.SERVER_CONNECT:
                    return 1;

                case SharedObjectEventType.SERVER_DISCONNECT:
                    return 2;

                case SharedObjectEventType.SERVER_SET_ATTRIBUTE:
                    return 3;

                case SharedObjectEventType.SERVER_DELETE_ATTRIBUTE:
                    return 10;

                case SharedObjectEventType.SERVER_SEND_MESSAGE:
                    return 6;

                case SharedObjectEventType.CLIENT_CLEAR_DATA:
                    return 8;

                case SharedObjectEventType.CLIENT_DELETE_DATA:
                    return 9;

                case SharedObjectEventType.CLIENT_INITIAL_DATA:
                    return 11;

                case SharedObjectEventType.CLIENT_STATUS:
                    return 7;

                case SharedObjectEventType.CLIENT_UPDATE_DATA:
                    return 4;

                case SharedObjectEventType.CLIENT_UPDATE_ATTRIBUTE:
                    return 5;

                case SharedObjectEventType.CLIENT_SEND_MESSAGE:
                    return 6;
            }
            return 0;
        }

        public static string ToString(SharedObjectEventType type)
        {
            switch (type)
            {
                case SharedObjectEventType.SERVER_CONNECT:
                    return "server connect";

                case SharedObjectEventType.SERVER_DISCONNECT:
                    return "server disconnect";

                case SharedObjectEventType.SERVER_SET_ATTRIBUTE:
                    return "server_set_attribute";

                case SharedObjectEventType.SERVER_DELETE_ATTRIBUTE:
                    return "server_delete_attribute";

                case SharedObjectEventType.SERVER_SEND_MESSAGE:
                    return "server_send_message";

                case SharedObjectEventType.CLIENT_CLEAR_DATA:
                    return "client_clear_data";

                case SharedObjectEventType.CLIENT_DELETE_DATA:
                    return "client_delete_data";

                case SharedObjectEventType.CLIENT_INITIAL_DATA:
                    return "client_initial_data";

                case SharedObjectEventType.CLIENT_STATUS:
                    return "client_status";

                case SharedObjectEventType.CLIENT_UPDATE_DATA:
                    return "client_update_data";

                case SharedObjectEventType.CLIENT_UPDATE_ATTRIBUTE:
                    return "client_update_attribute";

                case SharedObjectEventType.CLIENT_SEND_MESSAGE:
                    return "client_send_message";
            }
            return "unknown";
        }

        public static SharedObjectEventType ToType(byte rtmpType)
        {
            return _typeMap[rtmpType];
        }
    }
}

