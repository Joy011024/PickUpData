using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PureMVC.Interfaces;
using System.Windows.Forms;
namespace CaptureWebData
{
    public class FormMediatorHelper : Form, IMediator, INotifier
    {
        #region private member
        protected object compontent;
        protected string mediatorName;
        #endregion
        public FormMediatorHelper() : this("mediator")
        {

        }
        public FormMediatorHelper(string mediatorName)
        {
            this.mediatorName = mediatorName;
            this.compontent = null;
            FormBorderStyle = FormBorderStyle.None;
        }
        #region override
        private IFacade facade = CommonFacade.FacadeInstance;
        public virtual string MediatorName => this.mediatorName;
        protected IFacade Facade { get => facade; }

        public virtual object ViewComponent { get => compontent; set => compontent=value; }

        public virtual void HandleNotification(INotification notification)
        {
            
        }
        public virtual void InitializeNotifier(string key)
        {

        }

        public virtual string[] ListNotificationInterests()
        {
            return new string[] { };
        }

        public virtual void OnRegister()
        {
            
        }

        public virtual void OnRemove()
        {
            
        }

        public virtual void SendNotification(string notificationName, object body = null, string type = null)
        {
            facade.SendNotification(notificationName, body, type);
        }
        #endregion



        #region Constants
        #endregion
 

        #region Constructors
       

       
        #endregion

        #region Accessors
       

        

        
        #endregion

        

        #region Public Methods
        #region IMediator Members
         

        

       

        
        #endregion

        #region INotifier Members

       

       

       
       
        #endregion
        #endregion

    }
}
