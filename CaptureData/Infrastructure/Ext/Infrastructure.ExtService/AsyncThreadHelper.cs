using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace Infrastructure.ExtService
{
    public class AsyncRequestThreadHelper:IAsyncResult
    {
        public AsyncCallback AsyncEvent { get; private set; }
        public bool IsCompleted { get; set; }
        public AsyncRequestThreadHelper(AsyncCallback callEvent)
        {
            AsyncEvent = callEvent;
        }
        public void SetComplate()
        {
            IsCompleted = true;
            if (AsyncEvent == null)
            {
                return;
            }
            AsyncEvent(this);
        }

        public object AsyncState
        {
            get { throw new NotImplementedException(); }
        }

        public System.Threading.WaitHandle AsyncWaitHandle
        {
            get { throw new NotImplementedException(); }
        }

        public bool CompletedSynchronously
        {
            get { throw new NotImplementedException(); }
        }
    }
    /// <summary>
    /// 异步进程执行动作
    /// </summary>
    public class AsyncThreadDoEvent
    {
        public delegate void CallEvent(object eventParam);
        public static AsyncRequestThreadHelper th{get;private set;}
        public void OpenNewThreadWithRun(CallEvent callBack,object param) 
        {
            AsyncCallback call = new AsyncCallback(t =>
            {
                callBack(param);
            });
            if (th == null)
            {
                th = new AsyncRequestThreadHelper(call);
            }
            new Thread(() =>
            {
               
                th.SetComplate();
            }).Start();
            
        }
    }
}
