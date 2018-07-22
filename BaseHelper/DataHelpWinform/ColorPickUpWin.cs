using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataHelpWinform
{
    public partial class ColorPickUpWin : Form
    {
        Form Frm { get; set; }
        public ColorRgbData Rgb = new ColorRgbData();
        public DelegateEvent.CallBack PickUpColorSuccessCallEvent { get; set; }
        public ColorPickUpWin()
        {
            InitializeComponent();
            Init();
        }
        public ColorPickUpWin(Form frm) :this()
        {
            this.Frm = frm;
        }
        public ColorPickUpWin(DelegateEvent.CallBack pickUpSucc)
            : this() 
        {
            PickUpColorSuccessCallEvent = pickUpSucc;
        }
        private void ColorPickUpWin_Load(object sender, EventArgs e)
        {

        }
        void Init() 
        {
            colorPickUp.RealTimeRgbCall = RealTimeRgbData;
            colorPickUp.PickUpColorSuccessCall = PickUpColorSuccessCall;//提取色彩执行完毕的事件
        }
        private void RealTimeRgbData(object data)
        {
            Rgb = data as ColorRgbData;
        }
        void PickUpColorSuccessCall(object data)
        {
            if (Frm != null) 
            {
                Frm.Close();
            }
            if (PickUpColorSuccessCallEvent != null) 
            {
                PickUpColorSuccessCallEvent(data);
            }
            this.Close();
        }
    }
}
