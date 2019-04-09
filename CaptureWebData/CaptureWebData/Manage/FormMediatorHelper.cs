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
        protected string mediatorName;
        protected object compontent;
        #endregion
       
        public FormMediatorHelper(string mediatorName)
        {
            this.mediatorName = mediatorName;
            this.compontent = null;
            FormBorderStyle = FormBorderStyle.None;
        }
        #region override
        public virtual string MediatorName
        {
            get
            {
                return this.mediatorName;

            }
        }
        public virtual object ViewComponent
        {
            get => compontent;
            set => compontent = value;
        }
        protected IFacade Facade
        {
            get => facade;
        }
        private IFacade facade = CommonFacade.FacadeInstance;

        public virtual string[] ListNotificationInterests()
        {
            return new string[] { };
        }
        public virtual void HandleNotification(INotification notification)
        {
            
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

        public virtual void InitializeNotifier(string key)
        {

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
