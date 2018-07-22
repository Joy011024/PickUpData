using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataHelpWinform
{
    public partial class SelectFile : UserControl
    {
        public delegate void GetFilePathCallBack(string path);
        public GetFilePathCallBack CallBack { get; set; }
        void SelfCallBack(string path) { }
        public string FilePath { get; set; }
        public SelectFile()
        {
            InitializeComponent();
        }
        public void RunCallBack() 
        {
            if (CallBack == null) 
            {
                CallBack = SelfCallBack;
            }
            CallBack(FilePath);
        }
        private void Button_Click(object sender, EventArgs e) 
        {
            if (SelectFileDialog.ShowDialog() == DialogResult.OK) 
            {
                FilePath = SelectFileDialog.FileName;
                txtFilePath.Text = FilePath;
                RunCallBack();
            }
        }
    }
}
