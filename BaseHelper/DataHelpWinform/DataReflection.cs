using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using System.Reflection;
namespace DataHelpWinform
{
    public static class DataReflection
    {
        /// <summary>
        /// 将页面元素数据转换为实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T DataConvertObject<T>(this T entity, List<DataItem> data) where T :class
        {
           return ConvertObject<T>(entity, data);
        }
        /// <summary>
        /// 通过反射将数据赋值给对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T ConvertObject<T>(T entity, List<DataItem> data)
        {
            Type type = entity.GetType();
            PropertyInfo[] pis = type.GetProperties();
            foreach (DataItem item in data)
            {
                string property = item.Tag as string;
                if (property == null || !pis.Where(p => p.Name == (item.Tag as string)).Any())
                {//含有元素则进行赋值
                    continue;
                }
                PropertyInfo pi = type.GetProperty(property);
                //if (item.ValueType != DataType.Object)
                //{
                  
                //}
                pi.SetValue(entity, item.Value, null);
            }
            return entity;
        }
    }
}
