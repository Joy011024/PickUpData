using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace DataHelp
{
    public static class EntityReflectionValidate
    {
        /// <summary>
        /// 验证实体数据中指定字符类型字段的值是否为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static bool ValidateEmptyField<T>(this T entity,string[] fields) where T:class
        {
            if (fields == null || fields.Length == 0) { return true; }
            Type t = typeof(T);
            foreach (string p in fields)
            {
                PropertyInfo pi = t.GetProperty(p);
                if (pi == null) { continue; }
                
                object value =pi.GetValue(entity, null);
                if (value.GetType() != typeof(string)) { continue; }
                if (string.IsNullOrEmpty(value as string))
                {
                    return true;
                }
            }
            return false ;
        }
    }
}
