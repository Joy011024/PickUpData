using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataHelp;
namespace DataHelpWinform
{
    public partial class SetTimeInterval : UserControl
    {
        public TimeIntervalParam param = new TimeIntervalParam();
        /// <summary>
        /// 设置好定时功能触发的回调函数
        /// </summary>
        public DelegateEvent.CallBack SetIntervalCallBack { get; set; }
        private void DefaultCallBack(object data) { }
        List<Control> cs = new List<Control>();
       
        public SetTimeInterval()
        {
            InitializeComponent();
            InitPage();
        }
        void InitTimeCategory()
        {
            Dictionary<string, object> data = TimeIntervalCategory.CustomTime.GetEnumFieldAttribute(typeof(DescriptionAttribute));
            cmbTimeCategory.Items.Clear();
            foreach (KeyValuePair<string, string> item in data.Select(s => new KeyValuePair<string, string>(s.Key, ((DescriptionAttribute)s.Value).Description)))
            {
                cmbTimeCategory.Items.Add(item);
            }
            cmbTimeCategory.DisplayMember = ComboBoxItem.Value.ToString();
            cmbTimeCategory.ValueMember = ComboBoxItem.Key.ToString();
            Dictionary<string, object> timeUnit = TimeSpanCategory.Second.GetEnumFieldAttribute(typeof(DescriptionAttribute));
            cmbTimeSpan.Items.Clear();
            foreach (KeyValuePair<string, string> item in timeUnit.Select(s => new KeyValuePair<string, string>(s.Key, ((DescriptionAttribute)s.Value).Description)))
            {
                cmbTimeSpan.Items.Add(item);
            }
            cmbTimeSpan.DisplayMember = ComboBoxItem.Value.ToString();
            cmbTimeSpan.ValueMember = ComboBoxItem.Key.ToString();
            cmbTimeSpan.SelectedIndex = 0;
            DateTime now = DateTime.Now;
            dtpSetTimeInterval.DateTimePickerMinTime();
            dtpAfter.DateTimePickerMinTime();
            //dtpSetTimeInterval.MinDate =now;
            //dtpSetTimeInterval.Value = now;
        }
        void InitPage() 
        {
            PageDataHelp page = new PageDataHelp();
            cs=page.ForeachPanel(this, typeof(Panel).Name);
            foreach (Control item in cs)
            {
                Panel p = item as Panel;
                p.Hide();
            }
            InitTimeCategory();
            cmbTimeCategory.SelectedIndex = 0;
        }
        private void ComboBox_SelectIndex(object sender,EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            KeyValuePair<string,string> data =(KeyValuePair<string,string>) cmb.SelectedItem;
            foreach (var item in cs)
            {
                string tag=item.Tag==null?string.Empty:item.Tag.ToString();
                if (tag== data.Key)
                {
                    item.Show(); 
                    continue;
                }
                item.Hide();
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0X20) e.KeyChar = (char)0;//禁止输入空格
            if (e.KeyChar == 0X2D) e.KeyChar=(char)0;//处理负号
            if (e.KeyChar <=0X20) 
            {
                return;
            }
            try 
            {
                double.Parse(((TextBox)sender).Text + e.KeyChar.ToString());
            }
            catch(Exception ex)
            {
                e.KeyChar = (char)0;//处理非法字符
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (SetIntervalCallBack == null) { SetIntervalCallBack = DefaultCallBack; }
            KeyValuePair<string,string> value =(KeyValuePair<string,string>) cmbTimeCategory.SelectedItem;
            TimeIntervalCategory type;
            Enum.TryParse(value.Key,out type);
            param.SetIntervalCategory = type;
            if (value.Key == TimeIntervalCategory.Quit.ToString()) 
            {
                SetIntervalCallBack(param);
                return;
            }
            if (value.Key == TimeIntervalCategory.FixedTime.ToString()) 
            {
                param.SetDateTime = dtpSetTimeInterval.Value;
                SetIntervalCallBack(param);
                return;
            }
            int size = 0;
            int.TryParse(txtSize.Text, out size);
            param.TriggerCount = size;
            int.TryParse(txtTimeSpan.Text, out size);
            KeyValuePair<string, string> span = (KeyValuePair<string, string>)cmbTimeSpan.SelectedItem;
            if (span.Key == TimeSpanCategory.Minute.ToString()) 
            {
                size = size * 60;
            }
            param.DateTimeSpan = size*1000;//1000=1秒
            if (cbAfter.Checked) 
            {
                param.AfterDateTimeRun = dtpAfter.Value;
            }
            SetIntervalCallBack(param);
        }
    }
}
