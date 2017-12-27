namespace FluorineFx.Messaging.Rtmp.SO
{
    using System;

    public enum SharedObjectEventType
    {
        Unknown,
        SERVER_CONNECT,
        SERVER_DISCONNECT,
        SERVER_SET_ATTRIBUTE,
        SERVER_DELETE_ATTRIBUTE,
        SERVER_SEND_MESSAGE,
        CLIENT_CLEAR_DATA,
        CLIENT_DELETE_ATTRIBUTE,
        CLIENT_DELETE_DATA,
        CLIENT_INITIAL_DATA,
        CLIENT_STATUS,
        CLIENT_UPDATE_DATA,
        CLIENT_UPDATE_ATTRIBUTE,
        CLIENT_SEND_MESSAGE
    }
}

