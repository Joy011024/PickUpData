namespace FluorineFx
{
    using FluorineFx.Invocation;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;

    public class DataSetTypeAttribute : Attribute, IInvocationResultHandler
    {
        private string _remoteClass;

        public DataSetTypeAttribute(string remoteClass)
        {
            this._remoteClass = remoteClass;
        }

        public void HandleResult(IInvocationManager invocationManager, MethodInfo methodInfo, object obj, object[] arguments, object result)
        {
            if (result is DataSet)
            {
                DataSet set = result as DataSet;
                ASObject obj2 = new ASObject(this._remoteClass);
                foreach (KeyValuePair<object, object> pair in invocationManager.Properties)
                {
                    if (pair.Key is DataTable)
                    {
                        DataTable key = pair.Key as DataTable;
                        if (set.Tables.IndexOf(key) != -1)
                        {
                            if (!key.ExtendedProperties.ContainsKey("alias"))
                            {
                                obj2[key.TableName] = pair.Value;
                            }
                            else
                            {
                                obj2[key.ExtendedProperties["alias"] as string] = pair.Value;
                            }
                        }
                    }
                }
                invocationManager.Result = obj2;
            }
        }
    }
}

