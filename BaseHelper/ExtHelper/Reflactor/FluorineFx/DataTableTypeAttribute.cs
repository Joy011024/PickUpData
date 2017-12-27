namespace FluorineFx
{
    using FluorineFx.Invocation;
    using System;
    using System.Collections;
    using System.Data;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple=true)]
    public class DataTableTypeAttribute : Attribute, IInvocationCallback
    {
        private string _propertyName;
        private string _remoteClass;
        private string _tableName;

        public DataTableTypeAttribute(string remoteClass)
        {
            this._remoteClass = remoteClass;
        }

        public DataTableTypeAttribute(string tableName, string remoteClass)
        {
            this._remoteClass = remoteClass;
            this._tableName = tableName;
        }

        public DataTableTypeAttribute(string tableName, string propertyName, string remoteClass)
        {
            this._remoteClass = remoteClass;
            this._tableName = tableName;
            this._propertyName = propertyName;
        }

        private ArrayList ConvertDataTable(DataTable dataTable)
        {
            ArrayList list = new ArrayList(dataTable.Rows.Count);
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                ASObject obj2 = new ASObject(this._remoteClass);
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    DataColumn column = dataTable.Columns[j];
                    obj2.Add(column.ColumnName, row[column]);
                }
                list.Add(obj2);
            }
            return list;
        }

        public void OnInvoked(IInvocationManager invocationManager, MethodInfo methodInfo, object obj, object[] arguments, object result)
        {
            DataTable table;
            ArrayList list;
            if ((result is DataSet) && (this._tableName != null))
            {
                DataSet set = result as DataSet;
                if (set.Tables.Contains(this._tableName))
                {
                    table = set.Tables[this._tableName];
                    if (this._propertyName != null)
                    {
                        table.ExtendedProperties.Add("alias", this._propertyName);
                    }
                    list = this.ConvertDataTable(table);
                    invocationManager.Properties[table] = list;
                }
            }
            if (result is DataTable)
            {
                table = result as DataTable;
                list = this.ConvertDataTable(table);
                invocationManager.Result = list;
            }
        }

        public string RemoteClass
        {
            get
            {
                return this._remoteClass;
            }
        }
    }
}

