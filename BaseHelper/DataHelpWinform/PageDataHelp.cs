using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using System.Windows.Forms;
using System.Reflection;
namespace DataHelpWinform
{
    public class PageDataHelp
    {
        /// <summary>
        /// 获取控件元素的内容
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public List<DataItem> GetControlData(System.Windows.Forms.Control.ControlCollection control)
        {
            List<DataItem> datas = new List<DataItem>();
            foreach (Control item in control)
            {
                if (item is Label || item is Button) 
                {
                    continue;
                }
                if (item is Panel || item is Form || item is TabControl || item is TabPage) 
                {
                   List<DataItem> children= GetControlData(item.Controls);
                   datas.AddRange(children);
                   continue;
                }
                DataItem di = new DataItem();
                if (item is TextBox) 
                {
                    di.ValueType = DataType.String;
                    di.Name = item.Name;
                    di.Value = item.Text;
                    di.Tag = item.Tag;
                }
                else if (item is ComboBox) 
                {
                    ComboBox cmb= item as ComboBox;
                    di.ValueType = DataType.Object;
                    di.Name = cmb.Name;
                    di.Value = cmb.SelectedItem;
                    di.Tag = cmb.Tag;
                }
                else if (item is CheckBox) 
                {
                    CheckBox cb = item as CheckBox;
                    di.Value = cb.Checked.ToString();
                    di.ValueType = DataType.Bool;
                    di.Name = cb.Name;
                    di.Tag = cb.Tag;
                }
                else if (item is RichTextBox) 
                {
                    RichTextBox rct = item as RichTextBox;
                    di.Tag = rct.Tag.ToString();
                    di.Value = rct.Text;
                    di.ValueType = DataType.String;
                    di.Name = rct.Name;
                }
                datas.Add(di);
            }
            return datas;
        }
        public List<Control> ForeachPanel(Control element, string elementType)
        {
            List<Control> controls = new List<Control>();
            if (element == null) { return controls; }
            Control.ControlCollection coll = element.Controls;
            if (coll == null || coll.Count == 0) { return controls; }
            foreach (Control item in coll)
            {
                Type type = item.GetType();
                if (type.Name == elementType)
                {
                    controls.Add(item);
                }
            }
            return controls;
        }
        public List<Control> ForachControls(Control element, string elementType, string tag) 
        {
            List<Control> controls = new List<Control>();
            if (element == null) { return controls; }
            if (element.GetType().Name == elementType && (element.Tag as string)==tag) 
            {
                controls.Add(element);
            }
            foreach (Control item in element.Controls)
            {
                controls.AddRange(ForachControls(item, elementType, tag));
            }
            return controls;
        }
        public List<Control> ForeachControl(Control element, string tag)
        {
            List<Control> controls = new List<Control>();
            if (element == null) { return controls; }
            Control.ControlCollection coll = element.Controls;
            if (coll == null || coll.Count == 0) { return controls; }
            foreach (Control item in coll)
            {
                if ((item.Tag as string) == tag)
                {
                    controls.Add(item);
                }
                List<Control> childs= ForeachControl(item, tag);
                if (childs.Count > 0) 
                {
                    controls.AddRange(childs);
                }
            }
            return controls;
        }
        public Control GetControlByName(Control.ControlCollection elements, string elementName)
        {
            Control ele = null;
            if (elements == null || elements.Count == 0) { return ele; }
            foreach (Control item in elements)
            {
                if (item.Name == elementName)
                {
                    return item;
                }
                Control.ControlCollection items=item.Controls;
                Control aim= GetControlByName(items, elementName);
                if (aim != null) 
                {
                    return aim;
                }
            }
            return ele;
        }
        /// <summary>
        /// 获取页面上对应的实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <returns></returns>
        public T GetClassFromControl<T>(System.Windows.Forms.Control.ControlCollection control,T entity) where T : class
        {
            GetControlData<T>(control, ref entity);
            return entity;
        }
         
        private void GetControlData<T>(System.Windows.Forms.Control.ControlCollection control,ref T entity) where T:class
        {
            foreach (Control item in control)
            {
                if (item is Label || item is Button)
                {
                    continue;
                }
                if (item is Panel||item is Form||item is TabControl||item is TabPage)
                {
                    GetControlData(item.Controls, ref entity);
                    continue;
                }
                string property = item.Tag as string;
                if (string.IsNullOrEmpty(property))
                {
                    continue;
                }
                PropertyInfo pi = typeof(T).GetProperty(property);
                if (pi == null) 
                {//没有该属性
                    continue;
                }
                if (item is TextBox)
                {
                    string text = item.Text;
                    if (!string.IsNullOrEmpty(text)) 
                    {//注意：如果输入的是数据库连接字符串需要进行处理(\\sql=》 \sql)
                  
                    }
                    pi.SetValue(entity, text, null);
                }
                else if (item is ComboBox)
                {
                    ComboBox cmb = item as ComboBox;
                    object obj = cmb.SelectedItem;
                    if (obj != null)
                    {
                        Type t = obj.GetType();
                        PropertyInfo p = t.GetProperty(cmb.ValueMember);
                        if (p != null)
                        {
                            pi.SetValue(entity, p.GetValue(obj, null), null);
                        }
                    }
                    else 
                    {
                        pi.SetValue(entity, cmb.Text, null);
                    }
                }
                else if (item is CheckBox)
                {
                    CheckBox cb = item as CheckBox;
                    pi.SetValue(entity, cb.Checked.ToString(),null);
                }
                else if (item is RichTextBox)
                {
                    RichTextBox rct = item as RichTextBox;
                    pi.SetValue(entity,rct.Text,null);
                }
            }
        }
        public RadioButton GetCheckedRadioButtonInPanel(Control panelEle) 
        {
            Control.ControlCollection  list= panelEle.Controls;
            foreach (Control item in list)
            {
                RadioButton rbt = item as RadioButton;
                if (rbt!=null&&rbt.Checked) 
                {
                    return rbt;
                }
            }
            return null;
        }
    }
}
