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
    public partial class TransparentPanel : UserControl
    {
        public TransparentPanel()
        {
            InitializeComponent();
            Init();
        }
        void Init() 
        {
            this.BackColor = Color.Transparent;//设置背景透明
        }
        /// <summary>
        /// 设置字体色
        /// </summary>
        /// <param name="fontColor"></param>
        public void SetTextFontColor(Color fontColor) 
        {
            baseTextBox.SetTextFontColor(fontColor);
        }
    }
}
