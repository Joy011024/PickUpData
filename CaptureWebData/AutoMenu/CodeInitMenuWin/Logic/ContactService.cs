using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.EFMsSQL;
using CodeInitMenuWin.Model;
namespace CodeInitMenuWin.Logic
{
    public class ContactService
    {
        public string ConnString{ get;private set; }
        public ContactService(string connString)
        {
            ConnString = connString;
        }
        public bool InsertContact() 
        {
            MainRespority<FullContact> mr = new MainRespority<FullContact>(ConnString);
            return false;
        }
    }
}
