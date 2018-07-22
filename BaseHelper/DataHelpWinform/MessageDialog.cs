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
    public partial class MessageDialog : Form
    {
        public MessageDialog()
        {
            InitializeComponent();
        }

        private void MessageDialog_Load(object sender, EventArgs e)
        {

        }
        public delegate void SendMessageCallBack(object data);
        private SendMessageCallBack call; 
        private void DefaultCallBack(object data) { }
        public void InitMessage(object data, SendMessageCallBack callBack) 
        {
            call = callBack;
            string message = data as string;
            if (!string.IsNullOrEmpty(message))
            {
                rtbMessage.Text = message;
            }
            this.ShowDialog();
        }
        private void Button_Click(object sender,EventArgs e)
        {
            GetMessage();
        }
        private void GetMessage() 
        {
            if (call == null) { call = DefaultCallBack; }
            call(rtbMessage.Text);
            this.Close();
        }
    }
}
