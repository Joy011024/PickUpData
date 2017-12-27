namespace FluorineFx.Messaging.Rtmp
{
    using FluorineFx;
    using System;

    public class StatusASO : ASObject
    {
        public const string APP_GC = "Application.GC";
        public const string APP_RESOURCE_LOWMEMORY = "Application.Resource.LowMemory";
        public const string APP_SCRIPT_ERROR = "Application.Script.Error";
        public const string APP_SCRIPT_WARNING = "Application.Script.Warning";
        public const string APP_SHUTDOWN = "Application.Shutdown";
        public const string ERROR = "error";
        public const string NC_CALL_BADVERSION = "NetConnection.Call.BadVersion";
        public const string NC_CALL_FAILED = "NetConnection.Call.Failed";
        public const string NC_CONNECT_APPSHUTDOWN = "NetConnection.Connect.AppShutdown";
        public const string NC_CONNECT_CLOSED = "NetConnection.Connect.Closed";
        public const string NC_CONNECT_FAILED = "NetConnection.Connect.Failed";
        public const string NC_CONNECT_INVALID_APPLICATION = "NetConnection.Connect.InvalidApp";
        public const string NC_CONNECT_REJECTED = "NetConnection.Connect.Rejected";
        public const string NC_CONNECT_SUCCESS = "NetConnection.Connect.Success";
        public const string NS_CLEAR_FAILED = "NetStream.Clear.Failed";
        public const string NS_CLEAR_SUCCESS = "NetStream.Clear.Success";
        public const string NS_DATA_START = "NetStream.Data.Start";
        public const string NS_FAILED = "NetStream.Failed";
        public const string NS_INVALID_ARGUMENT = "NetStream.InvalidArg";
        public const string NS_PAUSE_NOTIFY = "NetStream.Pause.Notify";
        public const string NS_PLAY_COMPLETE = "NetStream.Play.Complete";
        public const string NS_PLAY_FAILED = "NetStream.Play.Failed";
        public const string NS_PLAY_INSUFFICIENT_BW = "NetStream.Play.InsufficientBW";
        public const string NS_PLAY_PUBLISHNOTIFY = "NetStream.Play.PublishNotify";
        public const string NS_PLAY_RESET = "NetStream.Play.Reset";
        public const string NS_PLAY_START = "NetStream.Play.Start";
        public const string NS_PLAY_STOP = "NetStream.Play.Stop";
        public const string NS_PLAY_STREAMNOTFOUND = "NetStream.Play.StreamNotFound";
        public const string NS_PLAY_SWITCH = "NetStream.Play.Switch";
        public const string NS_PLAY_UNPUBLISHNOTIFY = "NetStream.Play.UnpublishNotify";
        public const string NS_PUBLISH_BADNAME = "NetStream.Publish.BadName";
        public const string NS_PUBLISH_START = "NetStream.Publish.Start";
        public const string NS_RECORD_FAILED = "NetStream.Record.Failed";
        public const string NS_RECORD_NOACCESS = "NetStream.Record.NoAccess";
        public const string NS_RECORD_START = "NetStream.Record.Start";
        public const string NS_RECORD_STOP = "NetStream.Record.Stop";
        public const string NS_SEEK_FAILED = "NetStream.Seek.Failed";
        public const string NS_SEEK_NOTIFY = "NetStream.Seek.Notify";
        public const string NS_UNPAUSE_NOTIFY = "NetStream.Unpause.Notify";
        public const string NS_UNPUBLISHED_SUCCESS = "NetStream.Unpublish.Success";
        public const string SO_CREATION_FAILED = "SharedObject.ObjectCreationFailed";
        public const string SO_NO_READ_ACCESS = "SharedObject.NoReadAccess";
        public const string SO_NO_WRITE_ACCESS = "SharedObject.NoWriteAccess";
        public const string SO_NOT_FOUND = "SharedObject.NoObjectFound";
        public const string SO_PERSISTENCE_MISMATCH = "SharedObject.BadPersistence";
        public const string STATUS = "status";
        public const string WARNING = "warning";

        private StatusASO()
        {
        }

        internal StatusASO(string code)
        {
            base.Add("code", code);
            base.Add("level", "status");
        }

        internal StatusASO(string code, string level, string description)
        {
            base.Add("code", code);
            base.Add("level", level);
            base.Add("description", description);
        }

        internal StatusASO(string code, string level, string description, object application, ObjectEncoding objectEncoding)
        {
            base.Add("code", code);
            base.Add("level", level);
            base.Add("description", description);
            base.Add("application", application);
            base.Add("objectEncoding", (double) objectEncoding);
        }

        public static StatusASO GetStatusObject(string statusCode, ObjectEncoding objectEncoding)
        {
            switch (statusCode)
            {
                case "NetConnection.Call.Failed":
                    return new StatusASO("NetConnection.Call.Failed", "error", string.Empty, null, objectEncoding);

                case "NetConnection.Call.BadVersion":
                    return new StatusASO("NetConnection.Call.BadVersion", "error", string.Empty, null, objectEncoding);

                case "NetConnection.Connect.AppShutdown":
                    return new StatusASO("NetConnection.Connect.AppShutdown", "error", string.Empty, null, objectEncoding);

                case "NetConnection.Connect.Closed":
                    return new StatusASO("NetConnection.Connect.Closed", "error", string.Empty, null, objectEncoding);

                case "NetConnection.Connect.Failed":
                    return new StatusASO("NetConnection.Connect.Failed", "error", string.Empty, null, objectEncoding);

                case "NetConnection.Connect.Rejected":
                    return new StatusASO("NetConnection.Connect.Rejected", "error", string.Empty, null, objectEncoding);

                case "NetConnection.Connect.Success":
                    return new StatusASO("NetConnection.Connect.Success", "status", string.Empty, null, objectEncoding);
            }
            return new StatusASO("NetConnection.Call.BadVersion", "error", string.Empty, null, objectEncoding);
        }

        public object Application
        {
            set
            {
                base["application"] = value;
            }
        }

        public int clientid
        {
            get
            {
                if (base.ContainsKey("clientid"))
                {
                    return (int) base["clientid"];
                }
                return 0;
            }
            set
            {
                base["clientid"] = value;
            }
        }

        public string code
        {
            get
            {
                if (base.ContainsKey("code"))
                {
                    return (string) base["code"];
                }
                return null;
            }
            set
            {
                base["code"] = value;
            }
        }

        public string description
        {
            get
            {
                if (base.ContainsKey("description"))
                {
                    return (string) base["description"];
                }
                return null;
            }
            set
            {
                base["description"] = value;
            }
        }

        public string details
        {
            get
            {
                if (base.ContainsKey("details"))
                {
                    return (string) base["details"];
                }
                return null;
            }
            set
            {
                base["details"] = value;
            }
        }

        public string level
        {
            get
            {
                if (base.ContainsKey("level"))
                {
                    return (string) base["level"];
                }
                return null;
            }
            set
            {
                base["level"] = value;
            }
        }

        public double objectEncoding
        {
            get
            {
                if (base.ContainsKey("objectEncoding"))
                {
                    return (double) base["objectEncoding"];
                }
                return 0.0;
            }
            set
            {
                base["objectEncoding"] = value;
            }
        }
    }
}

