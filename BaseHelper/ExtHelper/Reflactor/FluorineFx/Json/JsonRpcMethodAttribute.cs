namespace FluorineFx.Json
{
    using FluorineFx;
    using FluorineFx.Invocation;
    using FluorineFx.Util;
    using System;
    using System.Data;
    using System.Reflection;

    [Serializable, AttributeUsage(AttributeTargets.Method)]
    public sealed class JsonRpcMethodAttribute : Attribute, IInvocationResultHandler
    {
        private string _name;

        public JsonRpcMethodAttribute()
        {
        }

        public JsonRpcMethodAttribute(string name)
        {
            this._name = name;
        }

        public void HandleResult(IInvocationManager invocationManager, MethodInfo methodInfo, object obj, object[] arguments, object result)
        {
            if (invocationManager.Result is DataSet)
            {
                DataSet dataSet = result as DataSet;
                invocationManager.Result = TypeHelper.ConvertDataSetToASO(dataSet, false);
            }
            if (invocationManager.Result is DataTable)
            {
                DataTable dataTable = result as DataTable;
                invocationManager.Result = TypeHelper.ConvertDataTableToASO(dataTable, false);
            }
        }

        public string Name
        {
            get
            {
                return StringUtils.MaskNullString(this._name);
            }
            set
            {
                this._name = value;
            }
        }
    }
}

