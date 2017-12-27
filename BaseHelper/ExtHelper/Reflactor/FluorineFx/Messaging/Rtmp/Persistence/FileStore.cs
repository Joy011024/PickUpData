namespace FluorineFx.Messaging.Rtmp.Persistence
{
    using FluorineFx;
    using FluorineFx.IO;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Persistence;
    using System;
    using System.IO;

    internal class FileStore : MemoryStore
    {
        private string _extension;
        private string _path;

        public FileStore(IScope scope) : base(scope)
        {
            this._path = "persistence";
            this._extension = ".bin";
        }

        private string GetObjectFilename(IPersistable obj)
        {
            string name = obj.Name;
            if (name == null)
            {
                name = "__null__";
            }
            return PersistenceUtils.GetFilename(base._scope, this._path, name, this._extension);
        }

        private string GetObjectFilePath(IPersistable obj)
        {
            return this.GetObjectFilePath(obj, false);
        }

        private string GetObjectFilePath(IPersistable obj, bool completePath)
        {
            return PersistenceUtils.GetPath(base._scope, this._path);
        }

        public override bool Load(IPersistable obj)
        {
            return (obj.IsPersistent || (this.LoadObject(this.GetObjectFilename(obj), obj) != null));
        }

        public override IPersistable Load(string name)
        {
            IPersistable persistable = base.Load(name);
            if (persistable != null)
            {
                return persistable;
            }
            return this.LoadObject(name);
        }

        private IPersistable LoadObject(string name)
        {
            return this.LoadObject(name, null);
        }

        private IPersistable LoadObject(string name, IPersistable obj)
        {
            string location = PersistenceUtils.GetFilename(base._scope, this._path, name, this._extension);
            FileInfo file = base._scope.Context.GetResource(location).File;
            if (!file.Exists)
            {
                return null;
            }
            IPersistable persistable = obj;
            lock (base.SyncRoot)
            {
                using (FileStream stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    AMFReader reader = new AMFReader(stream);
                    string typeName = reader.ReadData() as string;
                    persistable = ObjectFactory.CreateInstance(typeName) as IPersistable;
                    persistable.Deserialize(reader);
                }
            }
            return persistable;
        }

        public override bool Remove(IPersistable obj)
        {
            return base.Remove(base.GetObjectId(obj));
        }

        public override bool Remove(string name)
        {
            base.Remove(name);
            string location = PersistenceUtils.GetFilename(base._scope, this._path, name, this._extension);
            FileInfo file = base._scope.Context.GetResource(location).File;
            if (file.Exists)
            {
                lock (base.SyncRoot)
                {
                    file.Delete();
                }
            }
            return true;
        }

        public override bool Save(IPersistable obj)
        {
            if (!base.Save(obj))
            {
                return false;
            }
            bool flag = this.SaveObject(obj);
            obj.IsPersistent = flag;
            return flag;
        }

        private bool SaveObject(IPersistable obj)
        {
            string objectFilename = this.GetObjectFilename(obj);
            FileInfo file = base._scope.Context.GetResource(objectFilename).File;
            string directoryName = file.DirectoryName;
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            lock (base.SyncRoot)
            {
                MemoryStream stream = new MemoryStream();
                AMFWriter writer = new AMFWriter(stream) {
                    UseLegacyCollection = false
                };
                writer.WriteString(obj.GetType().FullName);
                obj.Serialize(writer);
                writer.Flush();
                byte[] buffer = stream.ToArray();
                stream.Close();
                using (FileStream stream2 = new FileStream(file.FullName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    stream2.Write(buffer, 0, buffer.Length);
                    stream2.Close();
                }
            }
            return true;
        }

        public string Extension
        {
            get
            {
                return this._extension;
            }
            set
            {
                this._extension = value;
            }
        }
    }
}

