using Domain.CommonData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infrastructure.ExtService;
namespace GatherImage
{
    public partial class GatherFrm : Form
    {
        public GatherFrm()
        {
            InitializeComponent();
           
        }
        public void GatherImage()
        {
            if (lsbProcess.InvokeRequired)
            {
                DelegateData.SampleDelegate sam = new DelegateData.SampleDelegate(GatherImage);
                this.Invoke(sam);
                return;
            }
            int gather = DownLoadImageSize();
            lsbProcess.Items.Add("成功采集数目" + gather);
            //guin.GatherImage("http://q2.qlogo.cn/g?b=qq&k=gPpshyzPCdw5WsH3BUB5xg&s=100&t=1483286794");
        }
        int DownLoadImageSize() 
        {
            GatherUinImage guin = new GatherUinImage();
            bool isZip = true;
            if (rbOrigin.Checked)
            {
                isZip = false;
            }
            List<string> dirs=guin.DownLoadImage(isZip);
            if(dirs.Count>0)
            LoggerWriter.CreateLogFile("Rows:"+dirs.Count+"\r\n"+ string.Join("\r\n", dirs), 
                NowAppDirHelper.GetNowAppDir(AppCategory.WinApp) + "/" + AppCategory.WinApp.ToString(), ELogType.DebugData);
            int gather = dirs.Count;
            if (gather > 0)
            {//如果本次操作有图片进行下载，则删除轮询调度作业，直接使用while去查找，避免出现锁死的现象
                (new QuartzJob()).DeleteJob<JobDelegateFunction>();
                DownLoadImageSize();
            }
            return gather;
        }
        void QuartRun(object obj) 
        {
            GatherImage();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            InitDownload(-1);
        }
        void InitDownload(int lastSize)
        {
            //图片下载轮询方式进行更改
            //首先进行库遍历【1 查到数据之后 库轮询，没查到数据则job作业轮询直到查询到数据】
            QuartzJob job = new QuartzJob();
            if (lastSize !=0)
            {
                job.DeleteJob<JobDelegateFunction>();
                lastSize = DownLoadImageSize();
            }
            else
            {
                DelegateData.BaseDelegate del = QuartRun;
                job.CreateJobWithParam<JobDelegateFunction>(new object[] { del,lastSize }, DateTime.Now, 60, 0);
            }
        }
        private void BtnClearPRocess_Click(object sender, EventArgs e)
        {
            lsbProcess.Items.Clear();
        }
    }
}
