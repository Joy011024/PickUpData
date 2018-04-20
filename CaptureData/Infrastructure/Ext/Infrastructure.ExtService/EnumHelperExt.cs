using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace Infrastructure.ExtService
{
    public static  class EnumHelperExt
    {
        public static Dictionary<string,string> EnumFieldDescDict<FieldAttribute>(this Enum e, string attProper) where FieldAttribute : Attribute
        {
            //提取枚举成员列表
            Type enumType = e.GetType();
            //此处涉及到枚举的原理
            FieldInfo[] fis = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);//GetFields()如果直接使用Enum作为枚举对象，将会多增加一条记录
            Type att = typeof(FieldAttribute);
            Dictionary<string, string> fieldDesc = new Dictionary<string, string>();
            foreach (FieldInfo item in fis)
            {
                //提取枚举成员的修饰特性
                object[] atts = item.GetCustomAttributes(att, false);
                if (atts == null || atts.Length == 0)
                {
                    continue;
                }
                Type mt = atts[0].GetType();
                PropertyInfo pi = mt.GetProperty(attProper);
                if (pi == null)
                {//没有改特性属性
                    continue;
                }
                object obj = pi.GetValue(atts[0], null);
                if (obj == null)
                {//特性属性未赋值
                    fieldDesc.Add(item.Name, string.Empty);
                    continue;
                }
                fieldDesc.Add(item.Name, obj.ToString());
            }
            return fieldDesc;
        }
    }
}
