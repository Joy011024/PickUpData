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
    public partial class ColorPickUp : UserControl
    {
        public ColorRgbData Rgb = new ColorRgbData();
        /// <summary>
        /// 实时获取选择的rgb色彩值
        /// </summary>
        public DelegateEvent.CallBack RealTimeRgbCall;
        public DelegateEvent.CallBack PickUpColorSuccessCall;
        enum ERgb 
        {
            R=1,
            G=2,
            B=3
        }
        int MaxSize = 255;
        public ColorPickUp()
        {
            InitializeComponent();
            InitPage();
        }
        private void InitPage() 
        {
            txtR.KeyPress += (new TextBox()).TextBox_KeyPress;
            txtG.KeyPress += (new TextBox()).TextBox_KeyPress;
            txtB.KeyPress += (new TextBox()).TextBox_KeyPress;
            BindRGB(trackBarR);
            BindRGB(trackBarG);
            BindRGB(trackBarB);
            trackBarR.Value = trackBarR.Maximum / 2;
            BindTextBox();
        }
        private void BindRGB(TrackBar bar) 
        {
            bar.Maximum = MaxSize;
            bar.Minimum = 0;
        }
        private void TrackBar_KeyPress(object sender, KeyPressEventArgs e) 
        {
            TrackBar bar = sender as TrackBar;
            int select = bar.Value;
            txtR.Text = select.ToString();
        }
        void BindTextBox() 
        {
            txtRGB.Text = PickUpValue(txtR) + PickUpValue(txtG) + PickUpValue(txtB);
            Rgb.Red = txtR.ConvertIntFrom16();
            Rgb.Green = txtG.ConvertIntFrom16();
            Rgb.Blue = txtB.ConvertIntFrom16();
            panelRGB.BackColor = Color.FromArgb(Rgb.Red, Rgb.Green, Rgb.Blue);
            if (RealTimeRgbCall != null) 
            {
                RealTimeRgbCall(Rgb);
            }
        }
        string PickUpValue(TextBox txt)
        {
            return txt.Text.PadLeft(txt.MaxLength - txt.Text.Length, '0');
        }
        private void TrackBar_Scroll(object sender, EventArgs e)
        {
            TrackBar bar = sender as TrackBar;
            string select = bar.Value.ToString("x8").Replace("000000","");
            string tag = bar.Tag as string;
            ERgb rgb;
            Enum.TryParse(tag, out rgb);
            switch (rgb) 
            {
                case ERgb.R: txtR.Text = select; break;
                case ERgb.G: txtG.Text = select; break;
                case ERgb.B: txtB.Text = select; break;
            }
            BindTextBox();
        }
        private void Button_Click(object sender,EventArgs e)
        {
            if (PickUpColorSuccessCall != null) 
            {
                PickUpColorSuccessCall(Rgb);
            }
        }
    }
}
