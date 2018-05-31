﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHRApp.Infrastructure
{
    public interface IBaseSampleRepository
    {
        string SqlConnString { set; get; }
    }
    /// <summary>
    /// sample insert,delete ,update,select
    /// </summary>
    public interface IBaseCRUDRepository : IBaseSampleRepository
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
        bool Edit(string sql,Dictionary<string,object> param);
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
        T Get<T>(object key) where T:class;
        /// <summary>
        /// like query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        List<T> Query<T>(string cmd,Dictionary<string,object> param) where T:class;
        /// <summary>
        /// logic delete
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool LogicDelete(object key);
    }
    /// <summary>
    /// 数据简单操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T> : IBaseSampleRepository where T : class
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Add(T entity);
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Edit(T entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Delete(object key);
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool LogicDel(object key);
        /// <summary>
        /// 主键查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get(object key);
        /// <summary>
        /// SQL语句查询
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        IList<T> Query(string cmd);
    }
    /// <summary>
    /// 扩展查询列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseListRepository<T> : IBaseRepository<T> where T : class
    {
        /// <summary>
        /// 参数化查询
        /// </summary>
        /// <param name="cmd">带参数的SQL语句</param>
        /// <param name="param">key为参数名称，value为参数值</param>
        /// <returns></returns>
        IList<T> QueryList(string cmd, Dictionary<string, object> param);
        IList<T> QueryAll();
    }
    /// <summary>
    /// 含有批量操作的扩展功能
    /// </summary>
    /// <typeparam name="T">实体</typeparam>
    public interface IBatchExtRepository<T> : IBaseListRepository<T> where T : class
    {
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        List<bool> BatchAdd(List<T> entities);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        List<bool> BatchDelete(List<object> keys);
        /// <summary>
        /// 批量逻辑删除
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        List<bool> BatchLogicDelete(List<object> keys);
    }
}
