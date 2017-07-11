using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Globalization;
namespace MenuListCs
{
    /// <summary>
    /// 程序集入口
    /// </summary>
    public class AssemblyEntry
    {
        public Type[] GatherAssembly()
        {
            Type t = this.GetType();
            string filter = t.Name;//过滤掉当前的程序入口实体类
            Assembly ass = t.Assembly;
            //获取程序集下的其他实体类信息
            Type[] ts= ass.GetTypes();
            return ts;
        }
        public List<Form> GatherFormObject() 
        {
            List<Type> forms = new List<Type>();
            Type t = this.GetType();
            string filter = t.Name;//过滤掉当前的程序入口实体类
            Assembly ass = t.Assembly;
            //获取程序集下的其他实体类信息
            Type[] ts = ass.GetTypes();
            List<Form> fs = new List<Form>();
            foreach (Type item in ts)
            {
                if (item.BaseType.Name == typeof(System.Windows.Forms.Form).Name)
                {//继承于form
                    object obj = item.Assembly.CreateInstance(item.FullName);
                    Form f = obj as Form;
                    fs.Add(f);
                }
            }
            return fs;
        }
    }
}
