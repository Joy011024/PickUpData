namespace FluorineFx.Messaging.Rtmp.Persistence
{
    using FluorineFx.Messaging.Api;
    using System;
    using System.IO;
    using System.Text;

    internal class PersistenceUtils
    {
        public const string PersistencePath = "persistence";

        public static string GetFilename(IScope scope, string folder, string name, string extension)
        {
            return Path.Combine(GetPath(scope, folder), name + extension);
        }

        public static string GetPath(IScope scope, string folder)
        {
            StringBuilder builder = new StringBuilder();
            IScope scope2 = ScopeUtils.FindApplication(scope);
            if (scope2 != null)
            {
                do
                {
                    builder.Insert(0, scope.Name + Path.DirectorySeparatorChar);
                    scope = scope.Parent;
                }
                while (scope.Depth >= scope2.Depth);
                builder.Insert(0, "apps" + Path.DirectorySeparatorChar);
            }
            builder.Insert(0, "~");
            builder.Append(folder);
            builder.Append(Path.DirectorySeparatorChar);
            return builder.ToString();
        }
    }
}

