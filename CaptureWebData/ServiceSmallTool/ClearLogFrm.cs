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
using CommonHelperEntity;
using Common.Data;
using FeatureFrmList;
using System.Threading;
using Domain.CommonData;
namespace ServiceSmallTool
{
    public partial class ClearLogFrm : Form
    {
        public ClearLogFrm()
        {
            InitializeComponent();
            InitUI();
        }
        enum ButtonTag 
        {
            ClearTip=1,
            ClearLog=2,
            OutputDir=3
        }
        enum UiConfig
        {
            ClearLogDir=1,
            ClearBeforeDay=2
        }
        [Description("公用节点")]
        enum ConfigCommonNode
        {
            [Description("节点修改时间")]
            ConfigLastUpdateTime=1
        }
        void InitUI() 
        {
            txtLogDir.Text = ExeDir;
            btnClearLog.Tag = ButtonTag.ClearLog;
            btnClearNote.Tag = ButtonTag.ClearTip;
            btnClearLog.Click += new EventHandler(Button_Click);
            txtLogDir.MouseDoubleClick += new MouseEventHandler(TextDir_DoubleClick);
            lstNote.View = View.Details;
            lstNote.Columns.Add(new ColumnHeader() { Text = "Tip", Width = 400 });
            lstNote.GridLines = true;
            btnClearNote.Click += new EventHandler(Button_Click);
            btnDirOutput.Tag = ButtonTag.OutputDir;
            btnDirOutput.Click += new EventHandler(Button_Click);
            BindSelect();
            bg.DoWork += new DoWorkEventHandler(Background_DoWork);
            bg.RunWorkerAsync();
            SaveConfig();
        }
        void SaveConfig() 
        {
            string dir = new AppDirHelper().GetAppDir(AppCategory.WinApp) + @"\Config\AppConfig.xml";
            Dictionary<string, string> setting = new Dictionary<string, string>();
            setting.Add(UiConfig.ClearLogDir.ToString(), txtLogDir.Text);
            setting.Add(UiConfig.ClearBeforeDay.ToString(), txtDayBefore.Text);
            setting.Add(ConfigCommonNode.ConfigLastUpdateTime.ToString(), DateTime.Now.ToString(CommonFormat.DateTimeFormat));
            dir.UpdateConfigNode(setting, new XmlNodeDataAttribute()
            {//字典所属上级节点信息
                NodeName = "configuration/appSettings",
                NodeKeyName = "name",
                NodeKeyValue = "UiConfig"
            }, new XmlNodeDataAttribute()
            {//字典中每一项节点配置项
                NodeName = "add",
                NodeKeyName = "key",
                NodeKeyValue = "value"
            });
        }
        void BindSelect() 
        {
           // Dictionary<string,string> dict= PickupFolderType.FullName.EnumFieldDescDict<DescriptionAttribute>("Description");
            cmbFolderNameType.BindDataSource(PickupFolderType.FolderName);
        }
        OpenFileDialog of = new OpenFileDialog();
        ClearLogHelp logHelp = new ClearLogHelp();
        FolderBrowserDialog file = new FolderBrowserDialog();
        BackgroundWorker bg = new BackgroundWorker();
        Dictionary<string, ThreadStart> workList = new Dictionary<string, ThreadStart>();
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
                case ButtonTag.ClearLog: DoWork(); break;
                case ButtonTag.OutputDir://日志输出
                    KeyValuePair<string, string> pickFolder = (KeyValuePair<string, string>)cmbFolderNameType.SelectedItem;
                    PickupFolderType pickFolderE;
                    Enum.TryParse(pickFolder.Key,out pickFolderE);
                    OutputDirNames(txtLogDir.Text, ckContainerNode.Checked, pickFolderE); 
                    break;
            }
            
        }
        void OutputDirNames(string fatherDir, bool foreachNode, PickupFolderType folder) 
        {
            List<string> outputDirNames = new List<string>();
            logHelp.OriginDir = fatherDir;
            logHelp.OutputDirInLog(fatherDir, foreachNode, folder, outputDirNames);
            LogHelperExtend.WriteDocument(ExeDir,DateTime.Now.ToString( CommonFormat.DateToHourIntFormat)+FileSuffix.Log, string.Join("\r\n", outputDirNames));
        }
        void DoClearLogEvent() 
        {
            DoClearLog(null);
        }
        void DoClearLog(object data) 
        {//跨进程调度要实现UI联动需要进程回调
            if (this.InvokeRequired)
            {
                this.Invoke(new BaseDelegate(DoClearLog), data);
                return;
            }
            ClearLog();
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
            DateTime begin = DateTime.Now;
            logHelp.ClearLogOfDayBefore(dir, DayBefore, ckDeleteDir.Checked);
            DateTime end = DateTime.Now;
            //其他信息: 线程间操作无效: 从不是创建控件“lstNote”的线程访问它。
            
            //lstNote.Items.Add("Run time: "+begin.ToString(CommonFormat.DateTimeFormat) + " ~ " + end.ToString(CommonFormat.DateTimeFormat));
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
        void Background_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {//轮询调度 
                if (workList.Count == 0)
                {
                    return;
                }
                //检测是否为设置中断的轮询
                foreach (var item in workList)
                {
                    Thread th = new Thread(item.Value);
                    th.Start();
                }
                Thread.Sleep(300 * 1000);//休眠30秒
            }
        }
        void DoWork() 
        {
            bool needRunBackGround = false;
            if (workList.Count==0)
            {
                needRunBackGround = true;
            }
            string tag = ButtonTag.ClearLog.ToString();
            if (!workList.ContainsKey(tag)) 
            {//增加日志清除进程
                ThreadStart ts = new ThreadStart(DoClearLogEvent);//清除日志
                workList.Add(tag, ts);
            }
            if (needRunBackGround)
            {//重新启动
                bg.RunWorkerAsync();
            }
        }
        void StopWork() 
        {
            string tag = ButtonTag.ClearLog.ToString();
            if (!workList.ContainsKey(tag))
            { //停止日志清除进程
                workList.Remove(tag);
            }
        }
    }
    [Description("提取文件夹名称枚举")]
    public enum PickupFolderType
    {
        [Description("文件夹名")]
        FolderName = 1,
        [Description("相对路径名（作用于子文件夹）")]
        RelativeDir = 2,
        [Description("全路径")]
        FullName = 3
    }
    [Description("执行事件")]
    public class DoEventTag
    {
        [Description("上次执行的事件")]
        public string LastEvent { get; set; }
        [Description("现在执行的事件")]
        public string NowEvent { get; set; }
        [Description("是否继续执行")]
        public bool ContinueDo { get; set; }
        [Description("总共执行次数")]
        public int DoNumber { get; set; }
        [Description("中断次数")]
        public int StopNumber { get; set; }
    }
    public class ClearLogHelp 
    {
        public void ClearLogOfDayBefore(string logDir, int dayBefore,bool deleteNodeDir) 
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
                if (deleteNodeDir)
                { //进行子集目录删除[只能删除空目录]
                    //Directory.Delete(logDir);
                }
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
                ClearLogOfDayBefore(dis[i].FullName, dayBefore, deleteNodeDir);
            }
        }
        public string OriginDir;
        [Description("将子目录进行日志输入，foreachNode是否遍历多级子目录，folderType选择输出文件夹信息,outputDirNames输出的子目录名称列表")]
        public void OutputDirInLog(string fatherDir, bool foreachNode, PickupFolderType folderType, List<string> outputDirNames)
        {
           // string originDir=fatherDir;
            DirectoryInfo di = new DirectoryInfo(fatherDir);
            DirectoryInfo[] nodes = di.GetDirectories();
           
            //是否需要继续遍历子目录
            foreach (var item in nodes)
            {
                string dir = item.FullName;
                string outName =item.Name;// justOutputFolderName ? item.Name : dir;
                switch (folderType)
                {
                    case PickupFolderType.FolderName:
                        outName = item.Name;
                        break;
                    case PickupFolderType.RelativeDir:
                        //相对路径，需要剔除原始目录
                        outName = item.FullName;
                        if(!string.IsNullOrEmpty(OriginDir))
                        {
                            outName = outName.Replace(OriginDir, string.Empty);
                        }
                        break;
                    case PickupFolderType.FullName:
                        outName = item.FullName;
                        break;
                    default:
                        break;
                }
                outputDirNames.Add(outName);//日志追加
                //输出到日志
                if (foreachNode)
                {
                    OutputDirInLog(dir, foreachNode, folderType, outputDirNames);
                }
            }
        }
    }
}
