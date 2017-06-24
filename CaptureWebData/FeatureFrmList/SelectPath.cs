using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FeatureFrmList
{
    public partial class SelectPath : UserControl
    {
        public string SelectDir { get; private set; }
        public BaseDelegate CallBack { get; set; }
        public SelectPath()
        {
            InitializeComponent();
            btnSelectPath.Click += new EventHandler(Button_Click);
        }
        public void SetPath(string dir) 
        {
            SelectDir = dir;
            rtbDir.Text = SelectDir;
        }
        private void Button_Click(object sender, EventArgs e) 
        {
            if (folderBrowser.ShowDialog() == DialogResult.OK) 
            {
                string dir = folderBrowser.SelectedPath;
                SetPath(dir);
                if (CallBack != null) 
                {
                    CallBack(SelectDir);
                }
            }
        }
    }
}
