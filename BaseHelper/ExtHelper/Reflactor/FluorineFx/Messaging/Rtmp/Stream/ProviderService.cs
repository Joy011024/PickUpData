namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx.Configuration;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Messaging;
    using FluorineFx.Messaging.Api.Stream;
    using FluorineFx.Messaging.Rtmp.IO;
    using FluorineFx.Messaging.Rtmp.Stream.Provider;
    using log4net;
    using System;
    using System.Collections;
    using System.IO;

    internal class ProviderService : IProviderService, IScopeService, IService
    {
        private static ILog log = LogManager.GetLogger(typeof(ProviderService));

        public IEnumerator GetBroadcastStreamNames(IScope scope)
        {
            return scope.GetBasicScopeNames("bs");
        }

        public IMessageInput GetLiveProviderInput(IScope scope, string name, bool needCreate)
        {
            IBasicScope basicScope = scope.GetBasicScope("bs", name);
            if (basicScope == null)
            {
                if (!needCreate)
                {
                    return null;
                }
                lock (scope.SyncRoot)
                {
                    basicScope = scope.GetBasicScope("bs", name);
                    if (basicScope == null)
                    {
                        basicScope = new BroadcastScope(scope, name);
                        scope.AddChildScope(basicScope);
                    }
                }
            }
            if (basicScope is IBroadcastScope)
            {
                return (basicScope as IBroadcastScope);
            }
            return null;
        }

        public IMessageInput GetProviderInput(IScope scope, string name)
        {
            IMessageInput input = this.GetLiveProviderInput(scope, name, false);
            if (input == null)
            {
                return this.GetVODProviderInput(scope, name);
            }
            return input;
        }

        private FileInfo GetStreamFile(IScope scope, string name)
        {
            IStreamableFileFactory scopeService = ScopeUtils.GetScopeService(scope, typeof(IStreamableFileFactory)) as IStreamableFileFactory;
            if ((name.IndexOf(':') == -1) && (name.IndexOf('.') == -1))
            {
                name = "flv:" + name;
            }
            log.Info(string.Concat(new object[] { "GetStreamFile factory: ", scopeService, " name: ", name }));
            foreach (IStreamableFileService service in scopeService.GetServices())
            {
                if (name.StartsWith(service.Prefix + ':'))
                {
                    name = service.PrepareFilename(name);
                    break;
                }
            }
            IStreamFilenameGenerator generator = ScopeUtils.GetScopeService(scope, typeof(IStreamFilenameGenerator)) as IStreamFilenameGenerator;
            string fileName = generator.GenerateFilename(scope, name, GenerationType.PLAYBACK);
            if (generator.ResolvesToAbsolutePath)
            {
                return new FileInfo(fileName);
            }
            return scope.Context.GetResource(fileName).File;
        }

        public FileInfo GetVODProviderFile(IScope scope, string name)
        {
            FileInfo streamFile = null;
            try
            {
                log.Info("GetVODProviderFile scope path: " + scope.ContextPath + " name: " + name);
                streamFile = this.GetStreamFile(scope, name);
            }
            catch (IOException exception)
            {
                log.Error("Problem getting file: " + name, exception);
            }
            if (!((streamFile != null) && streamFile.Exists))
            {
                return null;
            }
            return streamFile;
        }

        public IMessageInput GetVODProviderInput(IScope scope, string name)
        {
            FileInfo vODProviderFile = this.GetVODProviderFile(scope, name);
            if (vODProviderFile == null)
            {
                return null;
            }
            IPipe pipe = new InMemoryPullPullPipe();
            pipe.Subscribe(new FileProvider(scope, vODProviderFile), null);
            return pipe;
        }

        public bool RegisterBroadcastStream(IScope scope, string name, IBroadcastStream broadcastStream)
        {
            bool flag = false;
            lock (scope.SyncRoot)
            {
                IBasicScope basicScope = scope.GetBasicScope("bs", name);
                if (basicScope == null)
                {
                    basicScope = new BroadcastScope(scope, name);
                    scope.AddChildScope(basicScope);
                }
                if (basicScope is IBroadcastScope)
                {
                    (basicScope as IBroadcastScope).Subscribe(broadcastStream.Provider, null);
                    flag = true;
                }
            }
            return flag;
        }

        public void Shutdown()
        {
        }

        public void Start(ConfigurationSection configuration)
        {
        }

        public bool UnregisterBroadcastStream(IScope scope, string name)
        {
            bool flag = false;
            lock (scope.SyncRoot)
            {
                IBasicScope basicScope = scope.GetBasicScope("bs", name);
                if (basicScope is IBroadcastScope)
                {
                    scope.RemoveChildScope(basicScope);
                    flag = true;
                }
            }
            return flag;
        }
    }
}

