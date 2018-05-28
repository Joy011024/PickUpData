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
        /// <summary>
        /// 去除首位空格
        /// </summary>
        public void Trim() 
        {
            Psw = Psw.Trim();
            PswConfirm = PswConfirm.Trim();
            UserName = UserName.Trim();
            Nick = Nick.Trim();
        }
        /// <summary>
        /// 账户的常规校验【用户名不能含有中文】，密码不能为空
        /// </summary>
        /// <returns></returns>
        public int AccountValid() 
        {
            return 0;
        }
    }
    public enum ErrorCode
    { 
        [Description("用户名含有中文")]//只允许字母数字
        UserNameContainerChinese=1,
        [Description("密码不能为空")]
        PswIsRequired=2
    }
}
