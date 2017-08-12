using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FeatureFrmList
{
    
    public partial class UserContact : UserControl
    {
        public UserContactData user = new UserContactData();
        public UserContact()
        {
            InitializeComponent();
        }
        public UserContactData GetUserContact() 
        {
            PageDataHelp page = new PageDataHelp();
            page.GetClassFromControl<UserContactData>(this.Controls, user);
            return user;
        }
    }
    public class UserContactData
    {
        public string Uin { get; set; }
        public string UinNick { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public string Webchat { get; set; }
        public string WebchatNick { get; set; }
    }
}
