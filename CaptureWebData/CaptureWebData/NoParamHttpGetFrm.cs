using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HttpClientHelper;
using System.IO;
namespace CaptureWebData
{
    public partial class NoParamHttpGetFrm : Form
    {
        public NoParamHttpGetFrm()
        {
            InitializeComponent();
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            string url = rtbUrl.Text;
            rtbResponse.Text= HttpClientExtend.HttpClientGet(url);
        }

        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                rtbPath.Text = folderBrowserDialog.SelectedPath; 
            }
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            string text = rtbResponse.Text;
            DateTime now = DateTime.Now;
            string file = rtbPath.Text+"/"+ now.ToString("yyyyMMddHHmmss") + ".txt";
            FileStream fs=new FileStream(file,FileMode.Create,FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs,Encoding.UTF8);
            sw.Write(text);
            sw.Close();
            fs.Close();
        }
    }
}
