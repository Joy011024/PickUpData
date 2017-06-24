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
    public partial class SelectFile : UserControl
    {
        public string SelectFileFullName { get; private set; }
        public BaseDelegate CallBack { get; set; }
        public SelectFile()
        {
            InitializeComponent();
            btnSelectPath.Click += new EventHandler(Button_Click);
        }
        public void SetPath(string fileName)
        {
            SelectFileFullName = fileName;
            rtbFileName.Text = SelectFileFullName;
        }
        private void Button_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string dir = openFile.FileName;
                SetPath(dir);
                if (CallBack != null)
                {
                    CallBack(SelectFileFullName);
                }
            }
        }
    }
}
