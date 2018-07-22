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
    public partial class DataGenerateHelper : UserControl
    {
        private string _GenerateData;
        public string GenerateData
        {
            get 
            {
                if (string.IsNullOrEmpty(_GenerateData)) 
                {
                    return Guid.Empty.ToString();
                }
                return _GenerateData;
            }
        }
        public DataGenerateHelper()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender,EventArgs e) 
        {
            _GenerateData = Guid.NewGuid().ToString().ToUpper();
            rtbData.Text = _GenerateData;
        }
    }
}
