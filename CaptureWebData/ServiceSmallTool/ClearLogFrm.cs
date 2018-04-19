using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infrastructure.ExtService;
using System.IO;
namespace ServiceSmallTool
{
    public partial class ClearLogFrm : Form
    {
        public ClearLogFrm()
        {
            InitializeComponent();
            txtLogDir.Text = ExeDir;
            btnClearLog.Tag = ButtonTag.ClearLog;
            btnClearNote.Tag = ButtonTag.ClearTip;
            btnClearLog.Click += new EventHandler(Button_Click);
            txtLogDir.MouseDoubleClick += new MouseEventHandler(TextDir_DoubleClick);
            lstNote.View = View.Details;
            lstNote.Columns.Add(new ColumnHeader() {  Text="Tip",Width=400});
            lstNote.GridLines = true;
            btnClearNote.Click += new EventHandler(Button_Click);
        }
        enum ButtonTag 
        {
            ClearTip=1,
            ClearLog=2
        }
        OpenFileDialog of = new OpenFileDialog();
        FolderBrowserDialog file = new FolderBrowserDialog();
        string ExeDir
        {
            get 
            {
                return new AppDirHelper().GetAppDir(AppCategory.WinApp);
            }
        }
        public void Button_Click(object sender,EventArgs e)
        {
            Button btn = sender as Button;
            ButtonTag tag;
            object bt = btn.Tag;
            if (bt == null)
            {
                return;
            }
            Enum.TryParse(bt.ToString(), out tag);
            switch (tag)
            {
                case ButtonTag.ClearTip:
                    lstNote.Items.Clear();
                    break;
                case ButtonTag.ClearLog: ClearLog(); break;
            }
            
        }
        void ClearLog() 
        {
            string day = txtDayBefore.Text;
            string dir = txtLogDir.Text;
            int DayBefore = 0;
            int.TryParse(day, out DayBefore);
            if (DayBefore < 0)
            {
                DayBefore = 1;
            }
            ClearLogHelp help = new ClearLogHelp();
            help.ClearLogOfDayBefore(dir, DayBefore);
        }
        public void TextDir_DoubleClick(object sender, EventArgs e)
        {//鼠标左键双击加载url 
            MouseEventArgs mouse = e as MouseEventArgs;
            TextBox txt=sender as TextBox;
            lstNote.Items.Add(mouse.Button.ToString());
            if (file.ShowDialog() == DialogResult.OK)
            {
                txt.Text = file.SelectedPath;
            }
        }
    }
    public class ClearLogHelp 
    {
        public void ClearLogOfDayBefore(string logDir, int dayBefore) 
        {
            /*
             清理指定日期前的日志
             * 步骤A
             * 1.查找目录下的文件
             * 2.文件创建日期是在指定日期前的进行删除操作
            */
            DirectoryInfo di = new DirectoryInfo(logDir);
            FileInfo[] fis = di.GetFiles();//目录下的文件列表
            if (fis.Length == 0)
            {//空文件夹 
            
            }
            DateTime time = DateTime.Now.AddDays(-dayBefore);
            for (int i = 0; i < fis.Length; i++)
            {
                FileInfo item = fis[i];
                if (item.CreationTime <= time)
                {
                    item.Delete();
                }
            }
            /*
             * 步骤B
             * 子目录 遍历
             * 重复步骤A
             */
            DirectoryInfo[] dis= di.GetDirectories();
            if (dis.Length == 0)
            {
                return;
            }
            for (int i = 0; i < dis.Length; i++)
            {
                ClearLogOfDayBefore(dis[i].FullName, dayBefore);
            }
        }
    }
}
