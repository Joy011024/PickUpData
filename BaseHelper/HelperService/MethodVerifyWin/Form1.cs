using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataHelp;
namespace MethodVerifyWin
{
    public partial class Form1 : Form
    {
        public enum EConvertInt 
        {
            Convert10=1,
            Convert16=2
        }
        public Form1()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender,EventArgs e) 
        {
            Button btn = sender as Button;
            string t10 = rtbInt.Text;
            string t16 = rtbInt16.Text;
            EConvertInt cint;
            string tag = btn.Tag as string;
            Enum.TryParse(tag, out cint);
            switch (cint)
            {
                case EConvertInt.Convert10:
                    rtbInt.Text = t16.ConvertIntFrom16String();
                    break;
                case EConvertInt.Convert16:
                    rtbInt16.Text = t10.Convert16ToString();
                    break;
            }
        }
    }
}
