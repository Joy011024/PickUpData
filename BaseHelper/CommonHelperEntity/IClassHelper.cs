using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace CommonHelperEntity
{
     public interface IClassHelper
    {
    }
     public static class ClassHelperExtend
     {
        /// <summary>
         ///  获取对象全部声明为public的公共属性
        /// </summary>
        /// <typeparam name="T">查询公共属性的对象类型</typeparam>
        /// <param name="help">对象实例【实际上不需要提供，只是接口中应用】</param>
        /// <returns></returns>
 
        public static PropertyInfo[] GetClassProperties<T>(this T help) where T : IClassHelper
        {
            Type type = help.GetType();
            PropertyInfo[] pis = type.GetProperties();
            return pis;
        }
     }
}
