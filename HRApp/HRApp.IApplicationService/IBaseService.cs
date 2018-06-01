using Common.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRApp.IApplicationService
{
    public interface IBaseServiceWithSqlConnstring<T> : IBaseService where T : class
    {
        JsonData Add(T model);
        List<T> QueryWhere(T model);
        T Get(object id);
        bool Update(T entity);
    }
    public interface ICountServiceWithConnString<T> : IBaseServiceWithSqlConnstring<T> where T : class
    {
        [Description("统计数目")]
        int Count(object entity);
    }
    public interface IBaseAllWithSqlConnString<T> : IBaseServiceWithSqlConnstring<T> where T : class
    {
        List<T> QueryAll();
    }
    public interface IBaseService
    {
        string SqlConnString { get; set; }
    }
    /// <summary>
    /// sample insert,delete ,update,select
    /// </summary>
    public interface IBaseCRUDRepository : IBaseService
    {
        /// <summary>
        /// insert
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Add<T>(T entity) where T : class;
        /// <summary>
        /// update
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        bool Edit(string sql, Dictionary<string, object> param);
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Delete(object key);
        /// <summary>
        /// primary key query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(object key) where T : class;
        /// <summary>
        /// like query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        List<T> Query<T>(string cmd, Dictionary<string, object> param) where T : class;
        /// <summary>
        /// logic delete
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool LogicDelete(object key);
    }
}
