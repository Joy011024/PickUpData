using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;
using System.Text.RegularExpressions;
namespace Infrastructure.ExtService
{
    public static class EntityReflection
    {
        public static PropertyInfo[] GetEntityProperty<T>(this T help) where T : class
        {
            return help.GetType().GetProperties();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="help"></param>
        /// <param name="propertyFalg">属性继承</param>
        /// <returns></returns>
        public static PropertyInfo[] GetEntityProperty<T>(this T help , BindingFlags propertyFalg) where T : class
        {
            return help.GetType().GetProperties(propertyFalg);
        }
        public static PropertyInfo[] GetEntityProperty<T>() where T : class
        {
            Type t = typeof(T);
            PropertyInfo[] pis = t.GetProperties();
            return pis;
        }
        /// <summary>
        /// 根据属性名获取属性信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="help"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo<T>(this T help, string property) where T : class
        {
            Type t = typeof(T);
            PropertyInfo pi = t.GetProperty(property);
            return pi;
        }
        /// <summary>
        /// 提供属性获取实体类中属性的值【没有检测属性是否存在】
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="help"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static object GetPropertyValue<T>(this T help, PropertyInfo p) where T : class
        {
            if (p == null) { return null; }
            return p.GetValue(help,null);
        }
        public static string[] GetEntityProperties<T>(this T help) where T:class 
        {
            return typeof(T).GetProperties().Select(p => p.Name).ToArray();
        }
        public static string GetPropertyValue<T>(this T helper,string property) where T:class 
        {
            Type type = typeof(T);
            PropertyInfo pi= type.GetProperty(property);
            object obj= pi.GetValue(helper, null);
            if (obj == null)
            {
                return string.Empty;
            }
            return obj.ToString();
        }
        public static void SetPropertyValue<T>(this T helper,string propertyName,object value) where T:class
        {
            Type st = helper.GetType();
            PropertyInfo pi = st.GetProperty(propertyName);
            if (pi == null)
            {//没有该属性
                return;
            }
            if (value == null) { return; }
            //如果是字符串，需要对空串进行过滤
            if (pi.PropertyType.Name == typeof(string).Name && (string.IsNullOrEmpty(value as string)))
            {
                return;
            }
            //如果类型不一致需要强制类型转换【如果类型不一致且可空】
            if (pi.PropertyType.Name == typeof(Nullable<>).Name)
            {
                NullableConverter nullableConverter = new NullableConverter(pi.PropertyType);//如何获取可空类型属性非空时的数据类型
                Type nt = nullableConverter.UnderlyingType;
                pi.SetValue(helper, Convert.ChangeType(value, nt), null);
                return;
            }
            pi.SetValue(helper, Convert.ChangeType(value, pi.PropertyType), null);
        }
        /// <summary>
        /// 使用正则表达式提取xml格式字符串中的内容到实体内【实体类中的属性名称需要保持待提取文本中key形式】
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="obj">待填充数据的实体</param>
        /// <param name="text">待提取的文本</param>
        public static void FromXmlStringGetEntity<T>(this T obj, string text) where T : class
        {
            Type t = obj.GetType();
            foreach (PropertyInfo item in t.GetProperties())
            {
                string pn = item.Name;
                Regex reg = new Regex(string.Format(@"<{0}>(.*)<\/{0}>", pn));//<{skey}>(.*)<\/{skey}> //提取出格式内的文本
                MatchCollection mc = reg.Matches(text);
                if (mc.Count == 0)
                {
                    continue;
                }
                Match m = mc[0];
                GroupCollection gc = m.Groups;
                if (gc.Count < 2)
                {
                    continue;
                }
                string propertyValue = gc[1].Value;
                obj.SetPropertyValue(pn, propertyValue);
            }
        }
        public static string FillStringFromObject<T>(this T helper, string waitFillString) where T : class
        {
            Type t = helper.GetType();
            foreach (PropertyInfo item in t.GetProperties())
            {
                string name = item.Name;
                object value = item.GetValue(helper, null);
                string vString = value == null ? string.Empty : value.ToString();
                waitFillString = waitFillString.Replace(string.Format("{0}", name), vString);
            }
            return waitFillString;
        }
        public static Type GetPropertyType<T>(this T helper,string property) where T:class
        {
            PropertyInfo pi = helper.GetType().GetProperty(property);
            return pi.GetType();
        }
        /// <summary>
        /// 获取字段列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static FieldInfo[] GetFieldList<T>(this T helper) where T:class
        {
            Type t = helper.GetType();
            return t.GetFields();
        }
    }
    public static class DataConvert
    {
        public static R MapObject<T, R>(this T source)
            where T : class
            where R : class
        {
            R result = System.Activator.CreateInstance<R>();
            List<string> commonProperty = new List<string>();
            Type st = source.GetType();
            PropertyInfo[] sp = st.GetProperties();
            Type rt = result.GetType();
            PropertyInfo[] rp = rt.GetProperties();
            foreach (PropertyInfo item in sp)
            {
                PropertyInfo pi = rp.Where(p => p.Name == item.Name).FirstOrDefault();
                if (pi == null)
                {
                    continue;
                }
                object obj = item.GetValue(source, null);
                //如果是字符串，需要对空串进行过滤
                if (item.PropertyType.Name == typeof(string).Name && (string.IsNullOrEmpty(obj as string)))
                {
                    continue;
                }
                if (obj == null) { continue; }
                //如果类型不一致需要强制类型转换【如果类型不一致且可空】
                if (pi.PropertyType.Name == typeof(Nullable<>).Name)
                {
                    NullableConverter nullableConverter = new NullableConverter(pi.PropertyType);//如何获取可空类型属性非空时的数据类型
                    Type nt = nullableConverter.UnderlyingType;
                    pi.SetValue(result, Convert.ChangeType(obj, nt), null);
                    continue;

                }
                pi.SetValue(result, Convert.ChangeType(obj, pi.PropertyType), null);
            }
            return result;
        }
    }
}
