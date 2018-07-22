using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DataHelpWinform
{
    public static class TextBoxHelper
    {
        public static void TextBox_KeyPress(this TextBox txt, object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0X20) e.KeyChar = (char)0;//禁止输入空格
            if (e.KeyChar == 0X2D) e.KeyChar = (char)0;//处理负号
            if (e.KeyChar <= 0X20)
            {
                return;
            }
            try
            {
                double.Parse(((TextBox)sender).Text + e.KeyChar.ToString());
            }
            catch (Exception ex)
            {
                e.KeyChar = (char)0;//处理非法字符
            }
        }
        public static int ConvertIntFrom16(this TextBox txt) 
        {
            string text = txt.Text;
            if (string.IsNullOrEmpty(text))
            {
                text = "00";
            }
            return Convert.ToInt32(text, 16);
        }
    }
    
}
