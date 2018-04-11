using Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRApp.IApplicationService
{
    public interface IBaseServiceWithSqlConnstring<T> where T:class
    {
        string SqlConnString { get; set; }
        JsonData Add(T model);
        List<T> QueryWhere(T model);
    }
    public interface IBaseAllWithSqlConnString<T> : IBaseServiceWithSqlConnstring<T> where T : class
    {
        List<T> QueryAll();
    }
}
