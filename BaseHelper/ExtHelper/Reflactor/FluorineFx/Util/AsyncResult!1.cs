namespace FluorineFx.Util
{
    using System;

    internal class AsyncResult<TResult> : AsyncResultNoResult
    {
        private TResult m_result;

        public AsyncResult(AsyncCallback asyncCallback, object state) : base(asyncCallback, state)
        {
            this.m_result = default(TResult);
        }

        public TResult EndInvoke()
        {
            base.EndInvoke();
            return this.m_result;
        }

        public void SetAsCompleted(TResult result, bool completedSynchronously)
        {
            this.m_result = result;
            base.SetAsCompleted(null, completedSynchronously);
        }
    }
}

