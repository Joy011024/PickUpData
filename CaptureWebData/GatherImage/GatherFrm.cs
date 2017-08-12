using Domain.CommonData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
            GatherUinImage guin = new GatherUinImage();
            bool isZip = true;
            if (rbOrigin.Checked)
            {
                isZip = false;
            }
           int gather=  guin.DownLoadImage(isZip).Count;
           lsbProcess.Items.Add("成功采集数目"+gather);
          
            //guin.GatherImage("http://q2.qlogo.cn/g?b=qq&k=gPpshyzPCdw5WsH3BUB5xg&s=100&t=1483286794");
        }
        void QuartRun(object obj) 
        {
            GatherImage();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            QuartzJob job = new QuartzJob();
            DelegateData.BaseDelegate del = QuartRun;
            job.CreateJobWithParam<JobDelegateFunction>(new object[] {  del }, DateTime.Now, 15, 0);
        }

        private void BtnClearPRocess_Click(object sender, EventArgs e)
        {
            lsbProcess.Items.Clear();
        }
    }
}
