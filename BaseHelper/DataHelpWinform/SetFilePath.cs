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
    public partial class SetFilePath : UserControl
    {
        public string FilePath { get; set; }
        public SetFilePath()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 设置默认显示的文件路径
        /// </summary>
        public void DefaultFilePath() 
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;//默认路径在程序集下
            InitFilePath(path);
        }
        public void InitFilePath(string path) 
        {
            if (string.IsNullOrEmpty(path)) { return; }
            FilePath = path;
            txtFilePath.Text = FilePath;
        }
        private void Button_Click(object sender,EventArgs e) 
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) 
            {
                FilePath = folderBrowserDialog.SelectedPath;
                txtFilePath.Text = FilePath;
            }
        }
    }
}
