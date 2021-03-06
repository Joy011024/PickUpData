﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PureMVC.Interfaces;
namespace CaptureWebData
{
    public  class FormMediatorHelper : Form,IMediator,INotifier
    {
        public FormMediatorHelper():this("")
        {

        }
        public FormMediatorHelper(string name)
        {
            InitializeComponent();
            this.mediatorName = name;
            this.compontent = null;
            FormBorderStyle = FormBorderStyle.None;
        }
        #region Constants

        #endregion

        #region Members

        protected string mediatorName;
        protected object compontent;
        #endregion

        #region Constructors




        #endregion

        #region Accessors

        public virtual string MediatorName
        {
            get { return mediatorName; }
        }


        public virtual object ViewComponent
        {
            get { return compontent; }
            set { compontent = value; }
        }


        protected IFacade Facade
        {
            get { return m_facade; }
        }
        #endregion

        #region Private Members


        private IFacade m_facade = CommonFacade.FacadeInstance;

        #endregion

        #region Public Methods
        #region IMediator Members

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
        #endregion

        #region INotifier Members




        public virtual void SendNotification(string notificationName, object body, string type)
        {

            m_facade.SendNotification(notificationName, body, type);
        }
        public virtual void InitializeNotifier(string key)
        {

        }
        #endregion

        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FormMediatorHelper
            // 
            this.ClientSize = new System.Drawing.Size(303, 309);
            this.Name = "FormMediatorHelper";
            this.ResumeLayout(false);

        }
    }


}
