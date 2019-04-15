using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PureMVC.Interfaces;
namespace PureMvcExt.AppFactory
{
    public class FrmBase:Form//,IMediator,INotifier
    {
        public FrmBase()
        {
            FormBorderStyle = FormBorderStyle.None;
        }
        public FrmBase(string key) : base()
        {

        }
    }
}
