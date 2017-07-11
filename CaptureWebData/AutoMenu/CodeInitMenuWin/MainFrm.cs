using MenuListCs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CodeInitMenuWin
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }
        void InitMenu()
        {
            AssemblyEntry entre = new AssemblyEntry();
            List<Form> fs= entre.GatherFormObject();
            List<string> items = new List<string>();
            List<ToolStripItem> ul = new List<ToolStripItem>();
            foreach (Form item in fs)
            {
                //ToolStripButton li = new ToolStripButton();//单独一项的菜单
                //li.Text = item.Text;
                ToolStripMenuItem li = new ToolStripMenuItem();
                li.Name = item.Name;
                li.Text = item.Text;
                string menuItem = item.Tag as string;
                if (!items.Contains(menuItem))
                {//菜单项
                    items.Add(menuItem);
                }
                //这是第几项
                int index = items.IndexOf(menuItem);
                ToolStripItem ts = ul[index];
            }
            tsMenu.Items.AddRange(ul.ToArray());
        }
    }
}
