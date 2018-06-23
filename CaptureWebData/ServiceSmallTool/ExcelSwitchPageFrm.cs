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
using CommonHelperEntity;
namespace ServiceSmallTool
{
    public partial class ExcelSwitchPageFrm : Form
    {
        public ExcelSwitchPageFrm()
        {
            InitializeComponent();
            btnCut.Click += new EventHandler(Button_Click);
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
            //参数提取
            int size = 0;
            if (!int.TryParse(txtNumber.Text, out size))
            {
                size = 30000;
            }
            CSVHelper.ReadCSVFile(selectFile.SelectFileFullName,size);
            //ExcelHelper.ExcelCuttingPage(selectFile.SelectFileFullName, 0, size, CallBack);
            //进行excel页分割动作
        }
        void CallBack(object data)
        { 
        
        }

    }
}
