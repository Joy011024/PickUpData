using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfrastructureRepository.IMainContext
{
    public interface IMainRepository<T>
    {
        /// <summary>
        /// 执行动作出现异常时的反馈内容
        /// </summary>
        string Message { get; }
        bool Insert(T entity);
        bool InsertList(List<T> list);
        IEnumerable<T> Query(Func<T, bool> lambda);
        /// <summary>
        /// 执行增删改操作的存储过程
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        int ExecuteSPNoQuery(string cmd, params object[] param);
        /// <summary>
        ///执行查询操作的存储过程
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="cmd"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        IEnumerable<R> ExecuteSPSelect<R>(string cmd, params object[] param);
    }
}
