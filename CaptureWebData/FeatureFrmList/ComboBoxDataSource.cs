using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataHelp;
using System.ComponentModel;
namespace FeatureFrmList
{
    public static class ComboBoxHelp
    {
        static bool isFirstBind = true;
        public delegate void SelectIndexCallBack(object data);
        public static SelectIndexCallBack CallBack;
        public static void SelfCallBack(object data) { }
        /// <summary>
        /// 为ComboBox组件执行选择项触发事件
        /// </summary>
        /// <param name="help">组件</param>
        /// <param name="selectIndexdCallBack">获取选择数据处理的回调函数</param>
        public static void RunSelectIndexed(this ComboBox help, SelectIndexCallBack selectIndexdCallBack)
        {

            if (selectIndexdCallBack != null)
            {
                CallBack = selectIndexdCallBack;
            }
            ComboBox_SelectedIndexChanged(help);
        }
        /// <summary>
        /// 选择数据后触发的回调函数
        /// </summary>
        /// <param name="data"></param>
        private static void ComboBox_SelectedIndexChanged(ComboBox cb)
        {
            if (cb == null) return;
            object select = cb.SelectedItem;
            if (CallBack == null)
            {
                CallBack = new SelectIndexCallBack(SelfCallBack);
            }
            CallBack(select);
        }
        /// <summary>
        /// 绑定数据源
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="dataSource"></param>
        public static void BindDataSource(this ComboBox cmb, Enum dataSource)
        {
            Dictionary<string, object> desc = dataSource.GetEnumFieldAttribute(typeof(DescriptionAttribute));
            cmb.Items.Clear();
            IEnumerable<KeyValuePair<string, string>> ds = desc.Select(kv => new KeyValuePair<string, string>(kv.Key, ((DescriptionAttribute)kv.Value).Description));
            foreach (KeyValuePair<string, string> item in ds)
            {
                cmb.Items.Add(item);
            }
            cmb.DisplayMember = "Value";
            cmb.ValueMember = "Key";
        }
        /// <summary>
        /// 为combobox绑定数据源，并设置第一项自动选中
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="dataSource"></param>
        public static void BindDataSourceInitSelect(this ComboBox cmb, Enum dataSource)
        {
            BindDataSource(cmb, dataSource);
            if (cmb.Items.Count > 0)
            {
                cmb.SelectedIndex = 0;
            }
        }
    }
}
