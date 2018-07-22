using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DataHelpWinform
{
    public class BaseTextBox : TextBox
    {
        public void SetTextFontColor(System.Drawing.Color fontColor) 
        {
            base.ForeColor = fontColor;
        }
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr LoadLibrary(string lpFileName);
        protected override CreateParams CreateParams
        {
            get
            {
                base.ImeMode = ImeMode.On;//允许输入中文
                CreateParams prams = base.CreateParams;
                if (LoadLibrary("msftedit.dll") != IntPtr.Zero)
                {
                    prams.ExStyle |= 0x020; // transparent   
                    prams.ClassName = "RICHEDIT50W";
                }
                return prams;
            }
        }

    } 
}
