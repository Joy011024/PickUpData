using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaptureWebData
{
    public class ActionTimeOut
    {
        long timeOut;//超时设定
        System.Action<Delegate> willDoProc;
        System.Action<Delegate> userDefineProc;//用户定义超时处理
        System.Action<Delegate> timeOutDoProc;//处理超时的动作
        System.Threading.ManualResetEvent doEvent = new System.Threading.ManualResetEvent(false);
        public void Dispose() 
        {
            if (doEvent != null)
                doEvent.Close();
            doEvent = null;
            willDoProc = null;
            timeOutDoProc = null;
            userDefineProc = null;
        }
        public void TimeOutVerify(long timeOutSec, System.Action<Delegate> proc, System.Action<Delegate> timeOutDo = null)
        {
            willDoProc = proc;
            userDefineProc = timeOutDo;
            timeOut = timeOutSec;
            timeOutDoProc = delegate { 
            //计算运行的时间
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                if (timeOutDo != null)
                    willDoProc(null);
                sw.Stop();
                if (sw.ElapsedMilliseconds < timeOutSec * 1000 && doEvent != null)
                {
                    doEvent.Set();
                }
            };
        }
        public bool WaitProc(long timeOutSec) 
        {
            timeOut = timeOutSec;
            timeOutDoProc.BeginInvoke(null, null, null);
            bool flag = doEvent.WaitOne((int)timeOut * 1000, false);
            if (!flag)
            {
                if (userDefineProc != null)
                    userDefineProc(null);
            }
            Dispose();
            return flag;
        }
    }
}
