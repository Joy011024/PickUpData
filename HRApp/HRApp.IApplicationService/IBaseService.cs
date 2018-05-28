using Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRApp.IApplicationService
{
    public interface IBaseServiceWithSqlConnstring<T> : BaseService where T : class
    {
        JsonData Add(T model);
        List<T> QueryWhere(T model);
        T Get(object id);
        bool Update(T entity);
    }
    public interface IBaseAllWithSqlConnString<T> : IBaseServiceWithSqlConnstring<T> where T : class
    {
        List<T> QueryAll();
    }
    public interface BaseService
    {
        string SqlConnString { get; set; }
    }
}
