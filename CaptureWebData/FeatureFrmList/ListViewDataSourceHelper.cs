using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

using Infrastructure.ExtService;
namespace FeatureFrmList
{
    public static class ListViewDataSourceHelper
    {
        static string CodeInsertColumn = "CodeInsertColumn";
        /// <summary>
        /// 绑定object类型的数据源[并将上数据绑定对应行的tag上]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <param name="dataSource"></param>
        public static void BindDataSource<T>(this ListView lst, List<T> dataSource, bool addIndex = false) where T : class
        {
            if (lst.Columns.Count == 0)
            {//控件自身限制：没有定义列不会显示数据
                return;
            }
           
            lst.Items.Clear();
            //提取列
            List<string> columns = new List<string>();
            foreach (ColumnHeader item in lst.Columns)
            {
                columns.Add(item.Name);
            }
            PropertyInfo[] pis = EntityReflection.GetEntityProperty<T>();
            for (int i = 0; i < dataSource.Count; i++)
            {
                T item = dataSource[i];
                List<ListViewItem.ListViewSubItem> row = new List<ListViewItem.ListViewSubItem>();
                if (addIndex)
                {
                    row.Add(new ListViewItem.ListViewSubItem() { Text = (i + 1).ToString() });
                }
                foreach (var cell in columns)
                {
                    ListViewItem.ListViewSubItem subs = new ListViewItem.ListViewSubItem();
                    subs.Name = cell;
                    object value = item.GetPropertyValue(cell);
                    if (value != null)
                    {
                        subs.Text = value.ToString();
                    }
                    row.Add(subs);
                }
                ListViewItem rowitem = new ListViewItem(row.ToArray(), 0);
                // rowitem.SubItems.AddRange(row.ToArray());
                rowitem.Tag = item;
                lst.Items.Add(rowitem);
            }
            if (addIndex)
            {//在listview中插入一列
                ColumnHeader first = lst.Columns[0];
                if ((first.Tag as string) != CodeInsertColumn)
                {
                    lst.Columns.Insert(0, new ColumnHeader() { Name = CodeInsertColumn, Text = CodeInsertColumn, Tag = CodeInsertColumn });
                }
            }
        }
        public static void BindDataSourceRelyColumnName<T>(this  ListView lst, List<T> dataSource, bool addIndex = false) where T : class
        {
            ListView.ColumnHeaderCollection heads = lst.Columns;
            List<string> columns = new List<string>();
            lst.Items.Clear();
            //按照列头的顺序增加显示的行数据
            foreach (ColumnHeader item in heads)
            {
                columns.Add(item.Name);
            }
            if (columns.Count == 0)
            {//没有设置listView的匹配列
                return;
            }
            for (int i = 0; i < dataSource.Count; i++)
            {
                T item = dataSource[i];
                List<ListViewItem.ListViewSubItem> row = new List<ListViewItem.ListViewSubItem>();
                if (addIndex)
                {
                    row.Add(new ListViewItem.ListViewSubItem() { Text = (i + 1).ToString() });
                }
                foreach (string column in columns)
                {
                    ListViewItem.ListViewSubItem subs = new ListViewItem.ListViewSubItem();
                    subs.Name = column;
                    PropertyInfo pi = item.GetPropertyInfo(column);
                    object value = item.GetPropertyValue(pi);
                    subs.Text = value == null ? string.Empty : value.ToString();
                    row.Add(subs);
                }
                ListViewItem rowitem = new ListViewItem();
                rowitem = new ListViewItem(row.ToArray(), 0);
                rowitem.Tag = item;
                lst.Items.Add(rowitem);
            }
            if (addIndex)
            {//在listview中插入一列
                ColumnHeader first = lst.Columns[0];
                if ((first.Tag as string) != CodeInsertColumn)
                {
                    lst.Columns.Insert(0, new ColumnHeader() { Name = CodeInsertColumn, Text = CodeInsertColumn, Tag = CodeInsertColumn });
                }
            }
        }
        public static void BindHead(this ListView lst, Dictionary<string,string> head)
        {
            if (lst.Columns.Count > 0)
            {
                lst.Columns.Clear();
            }
            lst.View = View.Details;
            foreach (var item in head)
            {
                lst.Columns.Add(new ColumnHeader() { Text=item.Value, Name=item.Key });
            }
        }
    }
}
