using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.GlobalModel;
using System.ComponentModel;
namespace HRApp.Model
{
    [TableField(TableName = "Account")]
    public class UserAccount : UserBaseField
    {
       
        [Description("账号是否激活")]
        public bool IsActive { get; set; }
        public string Psw { get; set; }
    }
    public class UserBaseField 
    {
        public string UserName { get; set; }
        [Description("昵称")]
        public string Nick { get; set; }
    }
    public class SignInAccountParam : UserBaseField
    {
        public string Psw { get; set; }
        public string PswConfirm { get; set; }
    }
}
