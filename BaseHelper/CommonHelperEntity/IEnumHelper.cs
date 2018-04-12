using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
namespace CommonHelperEntity
{
    public static class EnumHelper
    {
        public static FieldInfo[] GetEnumFields(Type enumType) 
        {
            //获取每一个枚举成员
            FieldInfo[] fis = enumType.GetFields();
            List<FieldInfo> members = new List<FieldInfo>();
            Array arr= Enum.GetNames(enumType);//获取枚举中全部声明的成员
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
        /// <summary>
        /// 获取枚举成员的指定特性
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="attributetype">修饰枚举成员的特性类型</param>
        /// <returns></returns>
        public static object[] GetEnumFieldAttribute(Type enumType , Type attributetype=null) 
        {
            if (attributetype == null)
            {
                return new object[0];
            }
            FieldInfo[] fis = GetEnumFields(enumType);
            List<object> atts=new List<object>();
            foreach (FieldInfo item in fis)
            {
               object obj=  item.GetCustomAttributes(attributetype,true).FirstOrDefault();
               atts.Add(obj);
            }
            return atts.ToArray();
        }
    }
}
