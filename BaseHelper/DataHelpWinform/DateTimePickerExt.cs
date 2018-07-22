using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataHelpWinform
{
    public static  class DateTimePickerExt
    {
        public static void DateTimePickerMinTime(this DateTimePicker dtp) 
        {
            DateTime now = DateTime.Now;
            dtp.MinDate = now;
            dtp.Value = now;
        }
    }
}
