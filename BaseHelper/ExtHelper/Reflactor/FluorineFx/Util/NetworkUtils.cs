namespace FluorineFx.Util
{
    using System;
    using System.Net.Sockets;

    public abstract class NetworkUtils
    {
        protected NetworkUtils()
        {
        }

        protected static bool SetSocketOption(Socket socket, SocketOptionLevel level, SocketOptionName name, bool value)
        {
            if (((int) socket.GetSocketOption(level, name)) == (value ? 1 : 0))
            {
                return value;
            }
            socket.SetSocketOption(level, name, value);
            if (((int) socket.GetSocketOption(level, name)) != 1)
            {
                return false;
            }
            return true;
        }

        protected static int SetSocketOption(Socket socket, SocketOptionLevel level, SocketOptionName name, int value)
        {
            if (((int) socket.GetSocketOption(level, name)) == value)
            {
                return value;
            }
            socket.SetSocketOption(level, name, value);
            return (int) socket.GetSocketOption(level, name);
        }
    }
}

