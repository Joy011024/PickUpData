using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace DataHelpWinform
{
    public static class ListViewExtend
    {
        /// <summary>
        /// ListView控件添加列头
        /// </summary>
        /// <param name="helper">组件</param>
        /// <param name="columnHeadName">列头名称</param>
        /// <param name="clearColumn">是否清除之前的列名</param>
        public static void ListViewAddColumns(this ListView helper,string[] columnHeadName, bool clearColumn) 
        {
            if (clearColumn) 
            {
                int count = helper.Columns.Count;
                for (int i = count-1; i >=0; i--)
                {
                    helper.Columns.RemoveAt(i);
                }
            }
            foreach (string item in columnHeadName)
            {
                helper.Columns.Add(item);
            }
        }
        /// <summary>
        /// 设置Listview的显示列名
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="columnDisplay"></param>
        public static void ListViewSetColumn(this ListView helper, Dictionary<string, string> columnDisplay) 
        {
            helper.View = View.Details;
            helper.Columns.Clear();
            foreach (KeyValuePair<string,string> item in columnDisplay)
            {
                ColumnHeader head = new ColumnHeader();
                head.Text = item.Value;
                head.Name = item.Key;
                head.Tag = item.Key;
                helper.Columns.Add(head);
            }
        }
        /// <summary>
        /// 获取ListView组件选择列的值
        /// </summary>
        /// <param name="help"></param>
        /// <param name="targetColumn"></param>
        /// <returns></returns>
        public static string[] GetTargetColumnValue(this ListView help, string targetColumn) 
        {
            List<string> data = new List<string>();
            if (help.Items.Count == 0) 
            {
                return null;
            }
            ColumnHeader head=help.Columns[targetColumn];
            if (head == null) 
            {//没有该列
                return null;
            }
            int index = head.Index;
            foreach (ListViewItem item in help.Items)
            {
                ListViewItem.ListViewSubItem sub = item.SubItems[index];
                data.Add(sub.Text);
            }
            return data.ToArray();
        }
    }
}
