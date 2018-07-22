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
    public partial class SetTimeIntrevalWin : Form
    {
        private void DefaultCallBack(object data) { }
        public DelegateEvent.CallBack SetIntervalEventCallBack { get; set; }
        public SetTimeIntrevalWin(DelegateEvent.CallBack callback)
        {
            InitializeComponent();
            SetIntervalEventCallBack = callback;
            Init();
        }
        
        void Init() 
        {
            if (SetIntervalEventCallBack == null) 
            {
                SetIntervalEventCallBack = DefaultCallBack;
            }
            setTimeInterval.SetIntervalCallBack = SetIntervalEventCallBack;
        }

        private void setTimeInterval_Load(object sender, EventArgs e)
        {

        }
    }
}
