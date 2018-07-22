using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace DataHelpWinform
{
    public class FileDialogHelp
    {
        public static string SelectFile(OpenFileDialog open) 
        {
            string fileFullName = string.Empty;
            if (open.ShowDialog() == DialogResult.OK) 
            {
                fileFullName = open.FileName;
            }
            return fileFullName;
        }
    }
}
