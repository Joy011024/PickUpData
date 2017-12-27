namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx.Configuration;
    using FluorineFx.Context;
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Stream;
    using FluorineFx.Messaging.Api.Stream.Support;
    using FluorineFx.Messaging.Rtmp;
    using log4net;
    using System;
    using System.Collections;
    using System.IO;

    internal class StreamService : IStreamService, IScopeService, IService
    {
        private static ILog log = LogManager.GetLogger(typeof(StreamService));

        public void closeStream()
        {
            IConnection connection = FluorineContext.Current.Connection;
            if (connection is IStreamCapableConnection)
            {
                IClientStream streamById = (connection as IStreamCapableConnection).GetStreamById(this.GetCurrentStreamId());
                if (streamById != null)
                {
                    if (streamById is IClientBroadcastStream)
                    {
                        IClientBroadcastStream stream2 = streamById as IClientBroadcastStream;
                        IBroadcastScope broadcastScope = this.GetBroadcastScope(connection.Scope, stream2.PublishedName);
                        if ((broadcastScope != null) && (connection is BaseConnection))
                        {
                            (connection as BaseConnection).UnregisterBasicScope(broadcastScope);
                        }
                    }
                    streamById.Close();
                }
                (connection as IStreamCapableConnection).DeleteStreamById(this.GetCurrentStreamId());
            }
        }

        public int createStream()
        {
            IConnection connection = FluorineContext.Current.Connection;
            if (connection is IStreamCapableConnection)
            {
                return (connection as IStreamCapableConnection).ReserveStreamId();
            }
            return -1;
        }

        public void deleteStream(int streamId)
        {
            IConnection connection = FluorineContext.Current.Connection;
            if (connection is IStreamCapableConnection)
            {
                IStreamCapableConnection connection2 = connection as IStreamCapableConnection;
                this.deleteStream(connection2, streamId);
            }
        }

        public void deleteStream(IStreamCapableConnection connection, int streamId)
        {
            IClientStream streamById = connection.GetStreamById(streamId);
            if (streamById != null)
            {
                if (streamById is IClientBroadcastStream)
                {
                    IClientBroadcastStream stream2 = streamById as IClientBroadcastStream;
                    IBroadcastScope broadcastScope = this.GetBroadcastScope(connection.Scope, stream2.PublishedName);
                    if ((broadcastScope != null) && (connection is BaseConnection))
                    {
                        (connection as BaseConnection).UnregisterBasicScope(broadcastScope);
                    }
                }
                streamById.Close();
            }
            connection.UnreserveStreamId(streamId);
        }

        public IBroadcastScope GetBroadcastScope(IScope scope, string name)
        {
            return (scope.GetBasicScope("bs", name) as IBroadcastScope);
        }

        private int GetCurrentStreamId()
        {
            return (int) FluorineContext.Current.Connection.GetAttribute("__@fluorinestreamid");
        }

        public void pause(bool pausePlayback, double position)
        {
            IConnection connection = FluorineContext.Current.Connection;
            if (connection is IStreamCapableConnection)
            {
                IStreamCapableConnection connection2 = connection as IStreamCapableConnection;
                int currentStreamId = this.GetCurrentStreamId();
                IClientStream streamById = connection2.GetStreamById(currentStreamId);
                if ((streamById != null) && (streamById is ISubscriberStream))
                {
                    ISubscriberStream stream2 = streamById as ISubscriberStream;
                    if (pausePlayback)
                    {
                        stream2.Pause((int) position);
                    }
                    else
                    {
                        stream2.Resume((int) position);
                    }
                }
            }
        }

        public void play(bool dontStop)
        {
            if (!dontStop)
            {
                IConnection connection = FluorineContext.Current.Connection;
                if (connection is IStreamCapableConnection)
                {
                    IStreamCapableConnection connection2 = connection as IStreamCapableConnection;
                    int currentStreamId = this.GetCurrentStreamId();
                    IClientStream streamById = connection2.GetStreamById(currentStreamId);
                    if (streamById != null)
                    {
                        streamById.Stop();
                    }
                }
            }
        }

        public void play(string name)
        {
            this.play(name, -2000.0, -1000.0, true);
        }

        public void play(string name, double start)
        {
            this.play(name, start, -1000.0, true);
        }

        public void play(string name, double start, double length)
        {
            this.play(name, start, length, true);
        }

        public void play(string name, double start, double length, bool flushPlaylist)
        {
            IConnection connection = FluorineContext.Current.Connection;
            if (connection is IStreamCapableConnection)
            {
                IStreamCapableConnection connection2 = connection as IStreamCapableConnection;
                IScope scope = connection.Scope;
                int currentStreamId = this.GetCurrentStreamId();
                if ((name == null) || string.Empty.Equals(name))
                {
                    this.SendNSFailed(connection2 as RtmpConnection, "The stream name may not be empty.", name, currentStreamId);
                }
                else
                {
                    IStreamSecurityService scopeService = ScopeUtils.GetScopeService(scope, typeof(IStreamSecurityService)) as IStreamSecurityService;
                    if (scopeService != null)
                    {
                        IEnumerator streamPlaybackSecurity = scopeService.GetStreamPlaybackSecurity();
                        while (streamPlaybackSecurity.MoveNext())
                        {
                            IStreamPlaybackSecurity current = streamPlaybackSecurity.Current as IStreamPlaybackSecurity;
                            if (!current.IsPlaybackAllowed(scope, name, (long) start, (long) length, flushPlaylist))
                            {
                                this.SendNSFailed(connection2 as RtmpConnection, "You are not allowed to play the stream.", name, currentStreamId);
                                return;
                            }
                        }
                    }
                    IClientStream streamById = connection2.GetStreamById(currentStreamId);
                    bool flag = false;
                    if (streamById == null)
                    {
                        streamById = connection2.NewPlaylistSubscriberStream(currentStreamId);
                        streamById.Start();
                        flag = true;
                    }
                    if (streamById is ISubscriberStream)
                    {
                        ISubscriberStream stream2 = streamById as ISubscriberStream;
                        SimplePlayItem item = new SimplePlayItem {
                            Name = name,
                            Start = (long) start,
                            Length = (long) length
                        };
                        if (stream2 is IPlaylistSubscriberStream)
                        {
                            IPlaylistSubscriberStream stream3 = stream2 as IPlaylistSubscriberStream;
                            if (flushPlaylist)
                            {
                                stream3.RemoveAllItems();
                            }
                            stream3.AddItem(item);
                        }
                        else if (stream2 is ISingleItemSubscriberStream)
                        {
                            ISingleItemSubscriberStream stream4 = stream2 as ISingleItemSubscriberStream;
                            stream4.PlayItem = item;
                        }
                        else
                        {
                            return;
                        }
                        try
                        {
                            stream2.Play();
                        }
                        catch (IOException exception)
                        {
                            if (flag)
                            {
                                streamById.Close();
                                connection2.DeleteStreamById(currentStreamId);
                            }
                            this.SendNSFailed(connection2 as RtmpConnection, exception.Message, name, currentStreamId);
                        }
                    }
                }
            }
        }

        public void publish(bool dontStop)
        {
            if (!dontStop)
            {
                IConnection connection = FluorineContext.Current.Connection;
                if (connection is IStreamCapableConnection)
                {
                    IStreamCapableConnection connection2 = connection as IStreamCapableConnection;
                    int currentStreamId = this.GetCurrentStreamId();
                    IClientStream streamById = connection2.GetStreamById(currentStreamId);
                    if (streamById is IBroadcastStream)
                    {
                        IBroadcastStream stream2 = streamById as IBroadcastStream;
                        if (stream2.PublishedName != null)
                        {
                            IBroadcastScope broadcastScope = this.GetBroadcastScope(connection.Scope, stream2.PublishedName);
                            if (broadcastScope != null)
                            {
                                broadcastScope.Unsubscribe(stream2.Provider);
                                if (connection is BaseConnection)
                                {
                                    (connection as BaseConnection).UnregisterBasicScope(broadcastScope);
                                }
                            }
                            stream2.Close();
                            connection2.DeleteStreamById(currentStreamId);
                        }
                    }
                }
            }
        }

        public void publish(string name)
        {
            this.publish(name, "live");
        }

        public void publish(string name, string mode)
        {
            IConnection connection = FluorineContext.Current.Connection;
            if (connection is IStreamCapableConnection)
            {
                IStreamCapableConnection connection2 = connection as IStreamCapableConnection;
                IScope scope = connection.Scope;
                int currentStreamId = this.GetCurrentStreamId();
                if ((name == null) || string.Empty.Equals(name))
                {
                    this.SendNSFailed(connection2 as RtmpConnection, "The stream name may not be empty.", name, currentStreamId);
                }
                else
                {
                    IStreamSecurityService scopeService = ScopeUtils.GetScopeService(scope, typeof(IStreamSecurityService)) as IStreamSecurityService;
                    if (scopeService != null)
                    {
                        IEnumerator streamPublishSecurity = scopeService.GetStreamPublishSecurity();
                        while (streamPublishSecurity.MoveNext())
                        {
                            IStreamPublishSecurity current = streamPublishSecurity.Current as IStreamPublishSecurity;
                            if (!current.IsPublishAllowed(scope, name, mode))
                            {
                                this.SendNSFailed(connection2 as RtmpConnection, "You are not allowed to publish the stream.", name, currentStreamId);
                                return;
                            }
                        }
                    }
                    IBroadcastScope broadcastScope = this.GetBroadcastScope(scope, name);
                    if ((broadcastScope != null) && (broadcastScope.GetProviders().Count > 0))
                    {
                        StatusASO status = new StatusASO("NetStream.Publish.BadName") {
                            clientid = currentStreamId,
                            details = name,
                            level = "error"
                        };
                        (connection2 as RtmpConnection).GetChannel((byte) (4 + ((currentStreamId - 1) * 5))).SendStatus(status);
                    }
                    else
                    {
                        IClientStream streamById = connection2.GetStreamById(currentStreamId);
                        if ((streamById == null) || (streamById is IClientBroadcastStream))
                        {
                            bool flag = false;
                            if (streamById == null)
                            {
                                streamById = connection2.NewBroadcastStream(currentStreamId);
                                flag = true;
                            }
                            IClientBroadcastStream broadcastStream = streamById as IClientBroadcastStream;
                            try
                            {
                                broadcastStream.PublishedName = name;
                                IScopeContext context = connection.Scope.Context;
                                IProviderService service2 = ScopeUtils.GetScopeService(connection.Scope, typeof(IProviderService)) as IProviderService;
                                if (service2.RegisterBroadcastStream(connection.Scope, name, broadcastStream))
                                {
                                    broadcastScope = this.GetBroadcastScope(connection.Scope, name);
                                    broadcastScope.SetAttribute("_transient_publishing_stream", broadcastStream);
                                    if (connection is BaseConnection)
                                    {
                                        (connection as BaseConnection).RegisterBasicScope(broadcastScope);
                                    }
                                }
                                if ("record".Equals(mode))
                                {
                                    broadcastStream.Start();
                                    broadcastStream.SaveAs(name, false);
                                }
                                else if ("append".Equals(mode))
                                {
                                    broadcastStream.Start();
                                    broadcastStream.SaveAs(name, true);
                                }
                                else if ("live".Equals(mode))
                                {
                                    broadcastStream.Start();
                                }
                                broadcastStream.StartPublishing();
                            }
                            catch (IOException exception)
                            {
                                StatusASO saso2 = new StatusASO("NetStream.Record.NoAccess") {
                                    clientid = currentStreamId,
                                    description = "The file could not be created/written to." + exception.Message,
                                    details = name,
                                    level = "error"
                                };
                                (connection2 as RtmpConnection).GetChannel((byte) (4 + ((currentStreamId - 1) * 5))).SendStatus(saso2);
                                broadcastStream.Close();
                                if (flag)
                                {
                                    connection2.DeleteStreamById(currentStreamId);
                                }
                            }
                            catch (Exception exception2)
                            {
                                log.Warn("Publish caught exception", exception2);
                            }
                        }
                    }
                }
            }
        }

        public void receiveAudio(bool receive)
        {
            IConnection connection = FluorineContext.Current.Connection;
            if (connection is IStreamCapableConnection)
            {
                IStreamCapableConnection connection2 = connection as IStreamCapableConnection;
                int currentStreamId = this.GetCurrentStreamId();
                IClientStream streamById = connection2.GetStreamById(currentStreamId);
                if ((streamById != null) && (streamById is ISubscriberStream))
                {
                    (streamById as ISubscriberStream).ReceiveAudio(receive);
                }
            }
        }

        public void receiveVideo(bool receive)
        {
            IConnection connection = FluorineContext.Current.Connection;
            if (connection is IStreamCapableConnection)
            {
                IStreamCapableConnection connection2 = connection as IStreamCapableConnection;
                int currentStreamId = this.GetCurrentStreamId();
                IClientStream streamById = connection2.GetStreamById(currentStreamId);
                if ((streamById != null) && (streamById is ISubscriberStream))
                {
                    (streamById as ISubscriberStream).ReceiveVideo(receive);
                }
            }
        }

        public void releaseStream(string streamName)
        {
        }

        public void seek(double position)
        {
            IConnection connection = FluorineContext.Current.Connection;
            if (connection is IStreamCapableConnection)
            {
                IStreamCapableConnection connection2 = connection as IStreamCapableConnection;
                int currentStreamId = this.GetCurrentStreamId();
                IClientStream streamById = connection2.GetStreamById(currentStreamId);
                if ((streamById != null) && (streamById is ISubscriberStream))
                {
                    ISubscriberStream stream2 = streamById as ISubscriberStream;
                    try
                    {
                        stream2.Seek((int) position);
                    }
                    catch (NotSupportedException exception)
                    {
                        StatusASO status = new StatusASO("NetStream.Seek.Failed") {
                            clientid = currentStreamId,
                            description = "The stream doesn't support seeking.",
                            level = "error",
                            details = exception.Message
                        };
                        (connection2 as RtmpConnection).GetChannel((byte) (4 + ((currentStreamId - 1) * 5))).SendStatus(status);
                    }
                }
            }
        }

        private void SendNSFailed(RtmpConnection connection, string description, string name, int streamId)
        {
            StatusASO status = new StatusASO("NetStream.Failed") {
                clientid = streamId,
                description = description,
                details = name,
                level = "error"
            };
            connection.GetChannel((byte) (4 + ((streamId - 1) * 5))).SendStatus(status);
        }

        public void Shutdown()
        {
        }

        public void Start(ConfigurationSection configuration)
        {
        }
    }
}

