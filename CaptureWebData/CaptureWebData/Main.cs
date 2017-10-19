using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using CaptureManage.AppWin;
using Domain.GlobalModel;
using Infrastructure.ExtService;
using System.IO;
namespace CaptureWebData
{
    public partial class Main : Form
    {
        Assembly appAssembly;//当前程序运行的程序集
        string asseblyDir;
        public Main()
        {
            InitializeComponent();
            LoadOtherAssemblyWin();
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
        string GetAppDir()
        {
           string path=  this.GetType().Assembly.Location;
           DirectoryInfo di = new DirectoryInfo(path);
           return di.Parent.FullName;
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
        void LoadOtherAssemblyWin() 
        {
            //new System.Windows.Forms.ToolStripSplitButton()
            WinArray wins = new WinArray();
            foreach (KeyValuePair<string,List<ClassInfo>> item in wins.winGroup)
            {
                ToolStripSplitButton tsbSB = new ToolStripSplitButton()
                {
                    DisplayStyle=ToolStripItemDisplayStyle.Text,
                    Name=typeof(ToolStripSplitButton).Name+"_"+item.Key,
                    Text=item.Key
                };
                foreach (ClassInfo form in item.Value)
                {
                    ToolStripMenuItem li = new ToolStripMenuItem()
                    {
                        Text = (string.IsNullOrEmpty(form.Display)?form.ClassName:form.Display),
                        Tag=form.AssemblyName,//SystemConfig.CaptureWebDataWinAssembly,
                        Name=form.ClassName
                    };
                    li.Click += new EventHandler(OtherAssemblyToolStripItem_Click);
                    tsbSB.DropDownItems.Add(li);
                }
                tspHeadTool.Items.AddRange(new ToolStripItem[] { tsbSB });
            }
        }
        private void OtherAssemblyToolStripItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem li = sender as ToolStripMenuItem;
            if(string.IsNullOrEmpty(asseblyDir))
                asseblyDir=GetAppDir();
            Form frm = (new GeneratorClass()).AutoCreateType<Form>(asseblyDir, (string)li.Tag, li.Name, ".dll");
            //无法将顶级控件添加到控件。
            frm.TopLevel = false;
            frm.Parent = this;
            frm.Show();
        }
    }
}
