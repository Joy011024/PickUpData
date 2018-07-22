using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataHelpWinform;
namespace ServiceSmallTool
{
    public partial class ParallelFrm : Form
    {
        public ParallelFrm()
        {
            InitializeComponent();
            InitUIEle();
        }
        void InitUIEle()
        {
            try
            {
                txtParallelNum.KeyPress += new TextBox().TextBox_KeyPress;
            }
            catch (Exception ex)
            { 
            
            }
        }
    }
}
