using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace DataHelp
{
    public static class EnumExtend
    {
        /// <summary>
        /// 获取枚举中的指定特性的内容（键值：Key 枚举成员字符，value 指定特性的值）
        /// </summary>
        /// <param name="enumObj">枚举类型</param>
        /// <param name="attributeName">特性名称</param>
        /// <param name="attributePropertyName">需要获取特性的指定属性名称</param>
        /// <returns>查询结果【null表示传递的参数特性名或者特性指定的属性名为空】</returns>
        public static Dictionary<string, object> GetEnumFieldAttributeDict(this Enum enumObj, string attributeName, string attributePropertyName) 
        {
            if (string.IsNullOrEmpty(attributeName) || string.IsNullOrEmpty(attributePropertyName)) 
            {
                return null;
            }
            Dictionary<string, object> fieldAttribute = new Dictionary<string, object>();
            Type type = enumObj.GetType();
            string[] names = Enum.GetNames(type);//全部枚举成员名称
            //MemberInfo[] mis = type.GetMembers();//将会含有多一个枚举成员 int只为0
            List<FieldInfo> fils = new List<FieldInfo>();
            foreach (string item in names)
            {
                FieldInfo fi = type.GetField(item);
                object[] atts= fi.GetCustomAttributes(true);
                foreach (var att in atts)
                {
                    Type t = att.GetType();
                    string name = t.Name;
                    if (name != attributeName) { continue; }
                    if (t.GetProperty(attributePropertyName) == null) { continue; }
                    PropertyInfo pi = t.GetProperty(attributePropertyName);
                    if (pi == null) 
                    {
                        continue;
                    }
                    object value = pi.GetValue(att, null);
                    fieldAttribute.Add(item, value);
                    break;
                }
            }
            return fieldAttribute;
        }
        public static string[] GetEnumMembers(this Enum enumObj)
        {
            Type type = enumObj.GetType();
            string[] names = Enum.GetNames(type);//全部枚举成员名称
            return names;
        }
        /// <summary>
        /// 获取枚举成员的指定特性
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="attributetype">修饰枚举成员的特性类型</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetEnumFieldAttribute(this Enum enumType, Type attributetype = null)
        {
            if (attributetype == null)
            {
                return new Dictionary<string,object>();
            }
            FieldInfo[] fis = GetEnumFields(enumType);
            Dictionary<string,object> atts = new Dictionary<string, object>();
            foreach (FieldInfo item in fis)
            {
                object obj = item.GetCustomAttributes(attributetype, true).FirstOrDefault();
                atts.Add(item.Name, obj);
            }
            return atts;
        }
        public static FieldInfo[] GetEnumFields(this Enum entity)
        {
            //获取每一个枚举成员
            Type enumType = entity.GetType();
            FieldInfo[] fis = enumType.GetFields();
            List<FieldInfo> members = new List<FieldInfo>();
            Array arr = Enum.GetNames(enumType);//获取枚举中全部声明的成员
            foreach (string item in arr)
            {
                foreach (FieldInfo fi in fis)
                {
                    if (item == fi.Name)
                    {
                        members.Add(fi);
                    }
                }
            }
            return members.ToArray();
        }
    }
}
