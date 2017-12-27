using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
namespace SqlDBSyncTool
{
    public class BaseForm:Form
    {
        protected string assemblyPath = AppDomain.CurrentDomain.BaseDirectory;
        public string GetSearchColumnNameInTablePath()
        {
           return assemblyPath + ConfigurationManager.AppSettings["SearchColumnNameInTable"];
        }
        protected string DateTimeUnitlf =  ConfigurationManager.AppSettings["DateTimeUnitlf"];
    }
}
