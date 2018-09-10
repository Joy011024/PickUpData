using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using System.Windows.Forms; 
namespace CefSharpWin
{
    public class  FacadeFactory : Facade
    {
        private static FacadeFactory mInstance = null;

        public static FacadeFactory Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new FacadeFactory();
                }

                return mInstance;
            }
        }

        protected override void InitializeController()
        {
            base.InitializeController();

             
        }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            try
            {
                
            }
            catch (Exception ex)
            {
                
            }
        }
         
    }

    #region Extend
    public class MediatorService :   IMediator, INotifier
    {
        #region Constants
        /// <summary>
        /// The name of the <c>Mediator</c>
        /// </summary>
        public const string NAME = "Mediator";
        #endregion

        #region Members
        /// <summary>
        /// The mediator name
        /// </summary>
        protected string m_mediatorName;

        /// <summary>
        /// The view component being mediated
        /// </summary>
        protected object m_viewComponent;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a new mediator with the default name and no view component
        /// </summary>
        public MediatorService()
            : this(NAME, null)
        {
        }

        /// <summary>
        /// Constructs a new mediator with the specified name and no view component
        /// </summary>
        /// <param name="mediatorName">The name of the mediator</param>
        public MediatorService(string mediatorName)
            : this(mediatorName, null)
        {
        }

        /// <summary>
        /// Constructs a new mediator with the specified name and view component
        /// </summary>
        /// <param name="mediatorName">The name of the mediator</param>
        /// <param name="viewComponent">The view component to be mediated</param>
		public MediatorService(string mediatorName, object viewComponent)
        {
            m_mediatorName = (mediatorName != null) ? mediatorName : NAME;
            m_viewComponent = viewComponent;
            //  FormBorderStyle = FormBorderStyle.None;//移除标题栏（包含了min，max，close按钮）
        }
        #endregion

        #region Accessors
        /// <summary>
        /// The name of the <c>Mediator</c>
        /// </summary>
        /// <remarks><para>You should override this in your subclass</para></remarks>
        public virtual string MediatorName
        {
            get { return m_mediatorName; }
        }

        /// <summary>
        /// The <code>IMediator</code>'s view component.
        /// </summary>
        /// <remarks>
        ///     <para>Additionally, an implicit getter will usually be defined in the subclass that casts the view object to a type, like this:</para>
        ///     <example>
        ///         <code>
        ///             private System.Windows.Form.ComboBox comboBox {
        ///                 get { return viewComponent as ComboBox; }
        ///             }
        ///         </code>
        ///     </example>
        /// </remarks>
        public virtual object ViewComponent
        {
            get { return m_viewComponent; }
            set { m_viewComponent = value; }
        }

        /// <summary>
        /// Local reference to the Facade Singleton
        /// </summary>
        protected IFacade Facade
        {
            get { return m_facade; }
        }
        #endregion

        #region Private Members

        /// <summary>
        /// Local reference to the Facade Singleton
        /// </summary>
        private IFacade m_facade = FacadeFactory.Instance;

        #endregion

        #region Public Methods
        #region IMediator Members
        /// <summary>
        /// List the <c>INotification</c> names this <c>Mediator</c> is interested in being notified of
        /// </summary>
        /// <returns>The list of <c>INotification</c> names </returns>
        public virtual IList<string> ListNotificationInterests()
        {
            return new List<string>();
        }

        /// <summary>
        /// Handle <c>INotification</c>s
        /// </summary>
        /// <param name="notification">The <c>INotification</c> instance to handle</param>
        /// <remarks>
        ///     <para>
        ///        Typically this will be handled in a switch statement, with one 'case' entry per <c>INotification</c> the <c>Mediator</c> is interested in. 
        ///     </para>
        /// </remarks>
        public virtual void HandleNotification(INotification notification)
        {
        }

        /// <summary>
        /// Called by the View when the Mediator is registered
        /// </summary>
        public virtual void OnRegister()
        {
        }

        /// <summary>
        /// Called by the View when the Mediator is removed
        /// </summary>
        public virtual void OnRemove()
        {
        }
        #endregion

        #region INotifier Members

        /// <summary>
        /// Send an <c>INotification</c>
        /// </summary>
        /// <param name="notificationName">The name of the notiification to send</param>
        /// <remarks>Keeps us from having to construct new notification instances in our implementation code</remarks>
        /// <remarks>This method is thread safe</remarks>
        public virtual void SendNotification(string notificationName)
        {
            // The Facade SendNotification is thread safe, therefore this method is thread safe.
            m_facade.SendNotification(notificationName);
        }

        /// <summary>
        /// Send an <c>INotification</c>
        /// </summary>
        /// <param name="notificationName">The name of the notification to send</param>
        /// <param name="body">The body of the notification</param>
        /// <remarks>Keeps us from having to construct new notification instances in our implementation code</remarks>
		/// <remarks>This method is thread safe</remarks>
		public virtual void SendNotification(string notificationName, object body)
        {
            // The Facade SendNotification is thread safe, therefore this method is thread safe.
            m_facade.SendNotification(notificationName, body);
        }

        /// <summary>
        /// Send an <c>INotification</c>
        /// </summary>
        /// <param name="notificationName">The name of the notification to send</param>
        /// <param name="body">The body of the notification</param>
        /// <param name="type">The type of the notification</param>
        /// <remarks>Keeps us from having to construct new notification instances in our implementation code</remarks>
		/// <remarks>This method is thread safe</remarks>
		public virtual void SendNotification(string notificationName, object body, string type)
        {
            // The Facade SendNotification is thread safe, therefore this method is thread safe.
            m_facade.SendNotification(notificationName, body, type);
        }

        #endregion

        #region 控制窗体顶部按钮显示开关
        public void HideSystemButtons()
        {
             
        }
        #endregion
        #endregion
    }

    public class FormMediatorService :Form,  IMediator
    {
        public string  MediatorName { get;  }

        public object  ViewComponent
        { get; set; }
        public FormMediatorService()
        {
            string mediatoir = GetType().Name;
            FacadeFactory.Instance.RegisterMediator(this);
        }
        public virtual  void  HandleNotification(INotification notification)
        {
             
        }

        public virtual IList<string>  ListNotificationInterests()
        {
            return new string[0] { };
        }

        void IMediator.OnRegister()
        {
           
        }

        void IMediator.OnRemove()
        {
            
        }
    }

    public class CommandService : SimpleCommand
    {
        public virtual void  Execute(INotification notification)
        {
            try
            {
                SendNotification(notification.Name, notification.Body, notification.Type);//作为桥梁进行消息发送
            }
            catch (Exception ex)
            {

            }
        }
    }
    #endregion
}
