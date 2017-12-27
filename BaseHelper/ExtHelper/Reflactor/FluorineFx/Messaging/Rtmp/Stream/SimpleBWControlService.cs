namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx.Collections;
    using FluorineFx.Configuration;
    using FluorineFx.Messaging.Api;
    using log4net;
    using System;
    using System.Collections;
    using System.Threading;
    using System.Timers;

    internal class SimpleBWControlService : IBWControlService, IService
    {
        protected SynchronizedHashtable _contextMap = new SynchronizedHashtable();
        protected long _defaultCapacity = FluorineConfiguration.Instance.FluorineSettings.BWControlService.DefaultCapacity;
        protected long _interval = FluorineConfiguration.Instance.FluorineSettings.BWControlService.Interval;
        private object _syncLock = new object();
        private Timer _tokenDistributor;
        private static ILog log = LogManager.GetLogger(typeof(SimpleBWControlService));

        public ITokenBucket GetAudioBucket(IBWControlContext context)
        {
            if (!(context is BWContext))
            {
                return null;
            }
            BWContext context2 = (BWContext) context;
            return context2.buckets[0];
        }

        public ITokenBucket GetDataBucket(IBWControlContext context)
        {
            if (!(context is BWContext))
            {
                return null;
            }
            BWContext context2 = (BWContext) context;
            return context2.buckets[2];
        }

        public ITokenBucket GetVideoBucket(IBWControlContext context)
        {
            if (!(context is BWContext))
            {
                return null;
            }
            BWContext context2 = (BWContext) context;
            return context2.buckets[1];
        }

        private void InvokeCallback(BWContext context)
        {
            for (int i = 0; i < 3; i++)
            {
                IList list = context.pendingRequestArray[i] as IList;
                if (list.Count > 0)
                {
                    foreach (TokenRequest request in list)
                    {
                        IBWControllable bWControllable = context.GetBWControllable();
                        while (bWControllable != null)
                        {
                            BWContext context2 = this._contextMap[bWControllable] as BWContext;
                            if (context2 == null)
                            {
                                break;
                            }
                            lock (context2)
                            {
                                if (!((context2.bwConfig == null) || this.ProcessNonblockingRequest(request, context2)))
                                {
                                    break;
                                }
                            }
                            TokenRequestContext context3 = new TokenRequestContext {
                                acquiredToken = request.requestToken,
                                bc = bWControllable
                            };
                            request.acquiredStack.Push(context3);
                            bWControllable = bWControllable.GetParentBWControllable();
                        }
                        if (bWControllable == null)
                        {
                            try
                            {
                                request.callback.Available(context.buckets[request.channel], (long) request.requestToken);
                            }
                            catch (Exception exception)
                            {
                                log.Error("Error calling request's callback", exception);
                            }
                        }
                    }
                    list.Clear();
                }
            }
        }

        public IBWControlContext LookupContext(IBWControllable bc)
        {
            lock (this.SyncRoot)
            {
                return (this._contextMap[bc] as IBWControlContext);
            }
        }

        private bool ProcessBestEffortRequest(TokenRequest request, BWContext context)
        {
            if (context.bwConfig[3] >= 0L)
            {
                if (context.tokenRc[3] >= request.requestToken)
                {
                    context.tokenRc[3] -= request.requestToken;
                }
                else
                {
                    request.requestToken = context.tokenRc[3];
                    context.tokenRc[3] = 0.0;
                }
            }
            else
            {
                if (context.tokenRc[request.channel] < 0.0)
                {
                    return true;
                }
                if (context.tokenRc[request.channel] >= request.requestToken)
                {
                    context.tokenRc[request.channel] -= request.requestToken;
                }
                else
                {
                    request.requestToken = context.tokenRc[request.channel];
                    context.tokenRc[request.channel] = 0.0;
                }
            }
            if (request.requestToken == 0.0)
            {
                return false;
            }
            return true;
        }

        private bool ProcessBlockingRequest(TokenRequest request, BWContext context)
        {
            context.timeToWait = request.timeout;
            do
            {
                if (context.bwConfig[3] >= 0L)
                {
                    if (context.tokenRc[3] >= request.requestToken)
                    {
                        context.tokenRc[3] -= request.requestToken;
                        request.timeout = context.timeToWait;
                        return true;
                    }
                }
                else
                {
                    if (context.tokenRc[request.channel] < 0.0)
                    {
                        return true;
                    }
                    if (context.tokenRc[request.channel] >= request.requestToken)
                    {
                        context.tokenRc[request.channel] -= request.requestToken;
                        request.timeout = context.timeToWait;
                        return true;
                    }
                }
                long tickCount = Environment.TickCount;
                try
                {
                    Monitor.Wait(this, (int) context.timeToWait);
                }
                catch (ThreadInterruptedException)
                {
                }
                context.timeToWait -= Environment.TickCount - tickCount;
            }
            while (context.timeToWait > 0L);
            return false;
        }

        private bool ProcessNonblockingRequest(TokenRequest request, BWContext context)
        {
            if (context.bwConfig[3] >= 0L)
            {
                if (context.tokenRc[3] >= request.requestToken)
                {
                    context.tokenRc[3] -= request.requestToken;
                    return true;
                }
            }
            else
            {
                if (context.tokenRc[request.channel] < 0.0)
                {
                    return true;
                }
                if (context.tokenRc[request.channel] >= request.requestToken)
                {
                    context.tokenRc[request.channel] -= request.requestToken;
                    return true;
                }
            }
            (context.pendingRequestArray[request.channel] as IList).Add(request);
            return false;
        }

        private bool ProcessRequest(TokenRequest request)
        {
            for (IBWControllable controllable = request.initialBC; controllable != null; controllable = controllable.GetParentBWControllable())
            {
                BWContext context = this._contextMap[controllable] as BWContext;
                if (context == null)
                {
                    this.RollbackRequest(request);
                    return false;
                }
                lock (context)
                {
                    if (context.bwConfig != null)
                    {
                        bool flag;
                        if (request.type == TokenRequestType.BLOCKING)
                        {
                            flag = this.ProcessBlockingRequest(request, context);
                        }
                        else if (request.type == TokenRequestType.NONBLOCKING)
                        {
                            flag = this.ProcessNonblockingRequest(request, context);
                        }
                        else
                        {
                            flag = this.ProcessBestEffortRequest(request, context);
                        }
                        if (!flag)
                        {
                            if (request.type != TokenRequestType.NONBLOCKING)
                            {
                                this.RollbackRequest(request);
                            }
                            return false;
                        }
                    }
                    TokenRequestContext context2 = new TokenRequestContext {
                        acquiredToken = request.requestToken,
                        bc = controllable
                    };
                    request.acquiredStack.Push(context2);
                }
            }
            if (request.type == TokenRequestType.BEST_EFFORT)
            {
                this.RollbackRequest(request);
            }
            return true;
        }

        public IBWControlContext RegisterBWControllable(IBWControllable bc)
        {
            int num;
            BWContext context = new BWContext(bc);
            long[] channelInitialBurst = null;
            if (bc.BandwidthConfiguration != null)
            {
                context.bwConfig = new long[4];
                for (num = 0; num < 4; num++)
                {
                    context.bwConfig[num] = bc.BandwidthConfiguration.GetChannelBandwidth()[num];
                }
                channelInitialBurst = bc.BandwidthConfiguration.GetChannelInitialBurst();
            }
            context.buckets[0] = new Bucket(this, bc, 0);
            context.buckets[1] = new Bucket(this, bc, 1);
            context.buckets[2] = new Bucket(this, bc, 2);
            context.tokenRc = new double[4];
            if (context.bwConfig != null)
            {
                for (num = 0; num < 4; num++)
                {
                    if (channelInitialBurst[num] >= 0L)
                    {
                        context.tokenRc[num] = channelInitialBurst[num];
                    }
                    else
                    {
                        context.tokenRc[num] = this._defaultCapacity / 2L;
                    }
                }
                context.lastSchedule = Environment.TickCount;
            }
            else
            {
                context.lastSchedule = -1L;
            }
            lock (this.SyncRoot)
            {
                this._contextMap.Add(bc, context);
            }
            return context;
        }

        public void ResetBuckets(IBWControlContext context)
        {
            if (context is BWContext)
            {
                BWContext context2 = (BWContext) context;
                for (int i = 0; i < 3; i++)
                {
                    context2.buckets[i].Reset();
                }
            }
        }

        private void RollbackRequest(TokenRequest request)
        {
            while (request.acquiredStack.Count > 0)
            {
                TokenRequestContext context = request.acquiredStack.Pop() as TokenRequestContext;
                BWContext context2 = this._contextMap[context.bc] as BWContext;
                if (context2 != null)
                {
                    lock (context2)
                    {
                        if (context2.bwConfig != null)
                        {
                            if (context2.bwConfig[3] >= 0L)
                            {
                                if (request.type == TokenRequestType.BEST_EFFORT)
                                {
                                    context2.tokenRc[3] += context.acquiredToken - request.requestToken;
                                }
                                else
                                {
                                    context2.tokenRc[3] += context.acquiredToken;
                                }
                            }
                            else if (context2.bwConfig[request.channel] >= 0L)
                            {
                                if (request.type == TokenRequestType.BEST_EFFORT)
                                {
                                    context2.tokenRc[request.channel] += context.acquiredToken - request.requestToken;
                                }
                                else
                                {
                                    context2.tokenRc[request.channel] += context.acquiredToken;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Shutdown()
        {
            this._tokenDistributor.Enabled = false;
        }

        public void Start(ConfigurationSection configuration)
        {
            this._tokenDistributor = new Timer();
            this._tokenDistributor.Elapsed += new ElapsedEventHandler(this.TokenDistributor_Elapsed);
            this._tokenDistributor.Interval = this._interval;
            this._tokenDistributor.AutoReset = true;
            this._tokenDistributor.Enabled = true;
        }

        private void TokenDistributor_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this._contextMap.Count != 0)
            {
                lock (this.SyncRoot)
                {
                    BWContext context2;
                    ICollection values = this._contextMap.Values;
                    foreach (BWContext context in values)
                    {
                        lock ((context2 = context))
                        {
                            if (context.bwConfig != null)
                            {
                                long tickCount = Environment.TickCount;
                                long num2 = tickCount - context.lastSchedule;
                                context.lastSchedule = tickCount;
                                if (context.bwConfig[3] >= 0L)
                                {
                                    if (this._defaultCapacity >= context.tokenRc[3])
                                    {
                                        context.tokenRc[3] += (context.bwConfig[3] * num2) / 8000.0;
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < 3; i++)
                                    {
                                        if ((context.bwConfig[i] >= 0L) && (this._defaultCapacity >= context.tokenRc[i]))
                                        {
                                            context.tokenRc[i] += (context.bwConfig[i] * num2) / 8000.0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    foreach (BWContext context in values)
                    {
                        lock ((context2 = context))
                        {
                            Monitor.PulseAll(context);
                            this.InvokeCallback(context);
                        }
                    }
                }
            }
        }

        public void UnregisterBWControllable(IBWControlContext context)
        {
            this.ResetBuckets(context);
            lock (this.SyncRoot)
            {
                this._contextMap.Remove(context.GetBWControllable());
            }
        }

        public void UpdateBWConfigure(IBWControlContext context)
        {
            if (context is BWContext)
            {
                BWContext context2 = (BWContext) context;
                IBWControllable bWControllable = context2.GetBWControllable();
                lock (context2)
                {
                    if (bWControllable.BandwidthConfiguration == null)
                    {
                        context2.bwConfig = null;
                        context2.lastSchedule = -1L;
                    }
                    else
                    {
                        int num;
                        long[] bwConfig = context2.bwConfig;
                        context2.bwConfig = new long[4];
                        for (num = 0; num < 4; num++)
                        {
                            context2.bwConfig[num] = bWControllable.BandwidthConfiguration.GetChannelBandwidth()[num];
                        }
                        if (bwConfig == null)
                        {
                            context2.lastSchedule = Environment.TickCount;
                            long[] channelInitialBurst = bWControllable.BandwidthConfiguration.GetChannelInitialBurst();
                            for (num = 0; num < 4; num++)
                            {
                                if (channelInitialBurst[num] >= 0L)
                                {
                                    context2.tokenRc[num] = channelInitialBurst[num];
                                }
                                else
                                {
                                    context2.tokenRc[num] = this._defaultCapacity / 2L;
                                }
                            }
                        }
                        else if ((context2.bwConfig[3] >= 0L) && (bwConfig[3] < 0L))
                        {
                            context2.tokenRc[3] += (context2.tokenRc[0] + context2.tokenRc[1]) + context2.tokenRc[2];
                            for (num = 0; num < 3; num++)
                            {
                                context2.tokenRc[num] = 0.0;
                            }
                        }
                        else if ((context2.bwConfig[3] < 0L) && (bwConfig[3] >= 0L))
                        {
                            for (num = 0; num < 3; num++)
                            {
                                if (context2.bwConfig[num] >= 0L)
                                {
                                    context2.tokenRc[num] += context2.tokenRc[3];
                                    break;
                                }
                            }
                            context2.tokenRc[3] = 0.0;
                        }
                    }
                }
            }
        }

        internal SynchronizedHashtable ContextMap
        {
            get
            {
                return this._contextMap;
            }
        }

        internal long DefaultCapacity
        {
            get
            {
                return this._defaultCapacity;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this._syncLock;
            }
        }

        private class Bucket : ITokenBucket
        {
            private IBWControllable _bc;
            private int _channel;
            private SimpleBWControlService _simpleBWControlService;
            private static ILog log = LogManager.GetLogger(typeof(SimpleBWControlService.Bucket));

            public Bucket(SimpleBWControlService simpleBWControlService, IBWControllable bc, int channel)
            {
                this._bc = bc;
                this._channel = channel;
                this._simpleBWControlService = simpleBWControlService;
            }

            public bool AcquireToken(long tokenCount, long wait)
            {
                if (wait < 0L)
                {
                    return false;
                }
                SimpleBWControlService.TokenRequest request = new SimpleBWControlService.TokenRequest {
                    type = SimpleBWControlService.TokenRequestType.BLOCKING,
                    timeout = wait,
                    channel = this._channel,
                    initialBC = this._bc,
                    requestToken = tokenCount
                };
                return this._simpleBWControlService.ProcessRequest(request);
            }

            public long AcquireTokenBestEffort(long upperLimitCount)
            {
                SimpleBWControlService.TokenRequest request = new SimpleBWControlService.TokenRequest {
                    type = SimpleBWControlService.TokenRequestType.BEST_EFFORT,
                    channel = this._channel,
                    initialBC = this._bc,
                    requestToken = upperLimitCount
                };
                if (this._simpleBWControlService.ProcessRequest(request))
                {
                    return (long) request.requestToken;
                }
                return 0L;
            }

            public bool AcquireTokenNonblocking(long tokenCount, ITokenBucketCallback callback)
            {
                SimpleBWControlService.TokenRequest request = new SimpleBWControlService.TokenRequest {
                    type = SimpleBWControlService.TokenRequestType.NONBLOCKING,
                    callback = callback,
                    channel = this._channel,
                    initialBC = this._bc,
                    requestToken = tokenCount
                };
                return this._simpleBWControlService.ProcessRequest(request);
            }

            public void Reset()
            {
                for (IBWControllable controllable = this._bc; controllable != null; controllable = controllable.GetParentBWControllable())
                {
                    SimpleBWControlService.BWContext context = this._simpleBWControlService.ContextMap[controllable] as SimpleBWControlService.BWContext;
                    if (context == null)
                    {
                        break;
                    }
                    lock (context)
                    {
                        IList list = context.pendingRequestArray[this._channel] as IList;
                        SimpleBWControlService.TokenRequest request = null;
                        foreach (SimpleBWControlService.TokenRequest request2 in list)
                        {
                            if (request2.initialBC == this._bc)
                            {
                                this._simpleBWControlService.RollbackRequest(request2);
                                request = request2;
                                break;
                            }
                        }
                        if (request != null)
                        {
                            list.Remove(request);
                            try
                            {
                                request.callback.Reset(this, (long) request.requestToken);
                            }
                            catch (Exception exception)
                            {
                                log.Error("Error reset request's callback", exception);
                            }
                            break;
                        }
                    }
                }
            }

            public long Capacity
            {
                get
                {
                    return this._simpleBWControlService.DefaultCapacity;
                }
            }

            public double Speed
            {
                get
                {
                    SimpleBWControlService.BWContext context = this._simpleBWControlService.ContextMap[this._bc] as SimpleBWControlService.BWContext;
                    if (context.bwConfig[3] >= 0L)
                    {
                        return (double) ((context.bwConfig[3] * 0x3e8L) / 8L);
                    }
                    if (context.bwConfig[this._channel] >= 0L)
                    {
                        return (double) ((context.bwConfig[this._channel] * 0x3e8L) / 8L);
                    }
                    return -1.0;
                }
            }
        }

        private class BWContext : IBWControlContext
        {
            private IBWControllable _controllable;
            public ITokenBucket[] buckets = new ITokenBucket[3];
            public long[] bwConfig;
            public long lastSchedule;
            public ArrayList pendingRequestArray;
            public long timeToWait;
            public double[] tokenRc = new double[4];

            public BWContext(IBWControllable controllable)
            {
                this._controllable = controllable;
                this.pendingRequestArray = new ArrayList();
                this.pendingRequestArray.AddRange(new IList[] { new CopyOnWriteArray(), new CopyOnWriteArray(), new CopyOnWriteArray() });
            }

            public IBWControllable GetBWControllable()
            {
                return this._controllable;
            }
        }

        private class TokenRequest
        {
            public Stack acquiredStack = new Stack();
            public ITokenBucketCallback callback;
            public int channel;
            public IBWControllable initialBC;
            public double requestToken;
            public long timeout;
            public SimpleBWControlService.TokenRequestType type;
        }

        private class TokenRequestContext
        {
            public double acquiredToken;
            public IBWControllable bc;
        }

        private enum TokenRequestType
        {
            BLOCKING,
            NONBLOCKING,
            BEST_EFFORT
        }
    }
}

