using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
namespace CaptureWebData
{
    public partial class Main : Form
    {
        Assembly appAssembly;//当前程序运行的程序集
        public Main()
        {
            InitializeComponent();
            BindToolItemClick();
           appAssembly= GetAppAssembly();
        }
        void BindToolItemClick()
        {
            foreach (ToolStripItem item in tspHeadBtnSplider.DropDownItems)
            {
                item.Click += new EventHandler(ToolStripItem_Click);
            }
        }
        Assembly GetAppAssembly() 
        {
           Assembly ass=  this.GetType().Assembly;
           return ass;
            //只提取到当前的程序集
          
        }
        Type[] GetAppOtherForm() 
        {
            if (appAssembly == null)
            {
                appAssembly = GetAppAssembly();
            }
            string name = this.Name;
            Type[] ts = appAssembly.GetTypes().Where(t =>
                    t.BaseType.Name == typeof(System.Windows.Forms.Form).Name && t.Name != name
                ).ToArray();
            return ts;
        }
        private void ToolStripItem_Click(object sender, EventArgs e)
        {
            //定义那些窗体只创建一次
            //定义那些窗体已经加载（单例使用）
            ToolStripItem li = sender as ToolStripItem;
            if (li == null)
            { }
            string frmName = li.Tag as string;//对应的窗体
            if (string.IsNullOrEmpty(frmName))
            { //窗体信息不是绑定在Tag上
                
            }
            Type targetForm = appAssembly.GetType(frmName);
            if (targetForm == null)
            {//该窗体不再当前程序集内 
            
            }
            Form frm= System.Activator.CreateInstance(targetForm) as Form;
            if (frm == null)
            {//不是目标类 
            
            }
            frm.Show();
            //Form frm = (Form)Convert.ChangeType(targetForm, typeof(Form));//其他信息: 对象必须实现 IConvertible。
            // Form frm = (Form)targetForm.Assembly.CreateInstance(frmName);//不能加载当前程序集内的窗体
        }
    }
}
