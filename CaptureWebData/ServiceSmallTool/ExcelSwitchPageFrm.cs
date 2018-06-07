using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppLanguage;
namespace ServiceSmallTool
{
    public partial class ExcelSwitchPageFrm : Form
    {
        public ExcelSwitchPageFrm()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender,EventArgs e) 
        {
            string file = selectFile.SelectFileFullName;
            SwitchExcel(file);
        }
        void SwitchExcel(string file) 
        {
            if (string.IsNullOrEmpty(file))
            {
                rtbProcess.Text += Lang.LblClearData;
                return;
            }
        }
    }
}
