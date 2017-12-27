namespace FluorineFx.Scheduling
{
    using FluorineFx.Collections;
    using FluorineFx.Configuration;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Threading;
    using log4net;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Threading;

    internal class SchedulingService : ISchedulingService, IScopeService, IService
    {
        private readonly TreeSet _blockedJobs = new TreeSet();
        private bool _halted = false;
        private long _idleWaitTime = 0x7530L;
        private int _idleWaitVariablness = 0x1b58;
        private readonly object _jobLock = new object();
        private readonly IDictionary _jobsDictionary = new Hashtable(100);
        private long _misfireThreshold = 0x1388L;
        private bool _paused = false;
        private readonly TreeSet _pausedJobs = new TreeSet();
        private readonly TreeSet _pausedTriggers = new TreeSet();
        private readonly object _pauseLock = new object();
        private readonly Random _random = new Random((int) DateTime.Now.Ticks);
        private bool _signaled;
        private Thread _thread;
        private ThreadPoolEx _threadPool = new ThreadPoolEx(0xea60, 10);
        private readonly TreeSet _timeTriggers = new TreeSet(new TriggerComparator());
        private readonly object _triggerLock = new object();
        private readonly ArrayList _triggers = new ArrayList(100);
        private readonly IDictionary _triggersDictionary = new Hashtable(100);
        private const long DefaultIdleWaitTime = 0x7530L;
        private static readonly ILog log = LogManager.GetLogger(typeof(SchedulingService));

        public SchedulingService()
        {
            this._thread = new Thread(new ThreadStart(this.Run));
            this._thread.Start();
        }

        public Trigger AcquireNextTrigger(DateTime noLaterThan)
        {
            TriggerWrapper wrapper = null;
            lock (this._triggerLock)
            {
                while (wrapper == null)
                {
                    if (this._timeTriggers.Count > 0)
                    {
                        wrapper = this._timeTriggers[0] as TriggerWrapper;
                    }
                    if (wrapper == null)
                    {
                        return null;
                    }
                    if (!wrapper.Trigger.GetNextFireTimeUtc().HasValue)
                    {
                        this._timeTriggers.Remove(wrapper);
                        wrapper = null;
                    }
                    else
                    {
                        this._timeTriggers.Remove(wrapper);
                        if (this.ApplyMisfire(wrapper))
                        {
                            if (wrapper.Trigger.GetNextFireTimeUtc().HasValue)
                            {
                                this._timeTriggers.Add(wrapper);
                            }
                            wrapper = null;
                            continue;
                        }
                        if (wrapper.Trigger.GetNextFireTimeUtc().Value > noLaterThan)
                        {
                            this._timeTriggers.Add(wrapper);
                            return null;
                        }
                        wrapper.State = InternalTriggerState.Acquired;
                        return wrapper.Trigger;
                    }
                }
            }
            return null;
        }

        public string AddScheduledJob(int interval, IScheduledJob job)
        {
            return this.AddScheduledJob(interval, -1, job);
        }

        public string AddScheduledJob(int interval, int repeatCount, IScheduledJob job)
        {
            Trigger trigger = new Trigger("Trigger_" + job.Name, null, DateTime.UtcNow, null, repeatCount, (long) interval);
            this.ScheduleJob(job, trigger);
            return job.Name;
        }

        public string AddScheduledOnceJob(DateTime date, IScheduledJob job)
        {
            Trigger trigger = new Trigger("Trigger_" + job.Name, null, date);
            this.ScheduleJob(job, trigger);
            return job.Name;
        }

        public string AddScheduledOnceJob(long timeDelta, IScheduledJob job)
        {
            DateTime date = DateTime.UtcNow.AddMilliseconds((double) timeDelta);
            return this.AddScheduledOnceJob(date, job);
        }

        protected bool ApplyMisfire(TriggerWrapper tw)
        {
            DateTime utcNow = DateTime.UtcNow;
            if (this.MisfireThreshold > 0L)
            {
                utcNow = utcNow.AddMilliseconds((double) (-1L * this.MisfireThreshold));
            }
            DateTime? nextFireTimeUtc = tw.Trigger.GetNextFireTimeUtc();
            if (!(nextFireTimeUtc.HasValue && (nextFireTimeUtc.Value <= utcNow)))
            {
                return false;
            }
            tw.Trigger.UpdateAfterMisfire();
            if (!tw.Trigger.GetNextFireTimeUtc().HasValue)
            {
                tw.State = InternalTriggerState.Complete;
                lock (this._triggerLock)
                {
                    this._timeTriggers.Remove(tw);
                }
            }
            else if (nextFireTimeUtc.Equals(tw.Trigger.GetNextFireTimeUtc()))
            {
                return false;
            }
            return true;
        }

        private long GetRandomizedIdleWaitTime()
        {
            return (this._idleWaitTime - this._random.Next(this._idleWaitVariablness));
        }

        public string[] GetScheduledJobNames()
        {
            ArrayList list = new ArrayList();
            lock (this._jobLock)
            {
                foreach (string str in this._jobsDictionary.Keys)
                {
                    list.Add(str);
                }
            }
            return (string[]) list.ToArray(typeof(string));
        }

        public virtual Trigger[] GetTriggersForJob(string jobName)
        {
            ArrayList list = new ArrayList();
            lock (this._triggerLock)
            {
                for (int i = 0; i < this._triggers.Count; i++)
                {
                    TriggerWrapper wrapper = this._triggers[i] as TriggerWrapper;
                    if (wrapper.JobName.Equals(jobName))
                    {
                        list.Add(wrapper.Trigger);
                    }
                }
            }
            return (Trigger[]) list.ToArray(typeof(Trigger));
        }

        private ArrayList GetTriggerWrappersForJob(string jobName)
        {
            ArrayList list = new ArrayList();
            lock (this._triggerLock)
            {
                for (int i = 0; i < this._triggers.Count; i++)
                {
                    TriggerWrapper wrapper = this._triggers[i] as TriggerWrapper;
                    if (wrapper.JobName.Equals(jobName))
                    {
                        list.Add(wrapper);
                    }
                }
            }
            return list;
        }

        internal virtual void Halt()
        {
            lock (this._pauseLock)
            {
                this._halted = true;
                if (this._paused)
                {
                    Monitor.Pulse(this._pauseLock);
                }
                else
                {
                    this.SignalSchedulingChange();
                }
            }
        }

        protected internal virtual void NotifySchedulerThread()
        {
            this.SignalSchedulingChange();
        }

        public void ProcessJob(object state)
        {
            TriggerFiredBundle bundle = state as TriggerFiredBundle;
            Trigger trigger = bundle.Trigger;
            IScheduledJob job = bundle.Job;
            while (true)
            {
                JobExecutionException result = null;
                ScheduledJobContext context = new ScheduledJobContext();
                try
                {
                    job.Execute(context);
                }
                catch (JobExecutionException exception2)
                {
                    result = exception2;
                    log.Info(string.Format(CultureInfo.InvariantCulture, "Job {0} threw a JobExecutionException: ", new object[] { job.Name }), exception2);
                }
                catch (Exception exception3)
                {
                    log.Error(string.Format(CultureInfo.InvariantCulture, "Job {0} threw an unhandled Exception: ", new object[] { job.Name }), exception3);
                    SchedulerException cause = new SchedulerException("Job threw an unhandled exception.", exception3) {
                        ErrorCode = 800
                    };
                    result = new JobExecutionException(cause, false) {
                        ErrorCode = 800
                    };
                }
                SchedulerInstruction noInstruction = SchedulerInstruction.NoInstruction;
                try
                {
                    noInstruction = trigger.ExecutionComplete(context, result);
                }
                catch (Exception)
                {
                }
                if (noInstruction == SchedulerInstruction.ReExecuteJob)
                {
                    if (log.get_IsDebugEnabled())
                    {
                        log.Debug("Rescheduling trigger to reexecute");
                    }
                }
                else
                {
                    this.TriggeredJobComplete(trigger, job, noInstruction);
                    break;
                }
            }
            this.NotifySchedulerThread();
        }

        public void ReleaseAcquiredTrigger(Trigger trigger)
        {
            lock (this._triggerLock)
            {
                TriggerWrapper wrapper = this._triggersDictionary[trigger.Name] as TriggerWrapper;
                if ((wrapper != null) && (wrapper.State == InternalTriggerState.Acquired))
                {
                    wrapper.State = InternalTriggerState.Waiting;
                    this._timeTriggers.Add(wrapper);
                }
            }
        }

        public bool RemoveJob(string jobName)
        {
            bool flag = false;
            foreach (Trigger trigger in this.GetTriggersForJob(jobName))
            {
                this.RemoveTrigger(trigger.Name);
                flag = true;
            }
            lock (this._jobLock)
            {
                object obj2 = this._jobsDictionary[jobName];
                this._jobsDictionary.Remove(jobName);
                return ((obj2 != null) | flag);
            }
        }

        public bool RemoveScheduledJob(string jobName)
        {
            bool flag = false;
            foreach (Trigger trigger in this.GetTriggersForJob(jobName))
            {
                this.RemoveTrigger(trigger.Name);
                flag = true;
            }
            lock (this._jobLock)
            {
                object obj2 = this._jobsDictionary[jobName];
                this._jobsDictionary.Remove(jobName);
                return ((obj2 != null) | flag);
            }
        }

        public bool RemoveTrigger(string triggerName)
        {
            return this.RemoveTrigger(triggerName, true);
        }

        public bool RemoveTrigger(string triggerName, bool deleteOrphanedJob)
        {
            bool flag;
            lock (this._triggerLock)
            {
                object obj2 = this._triggersDictionary[triggerName];
                this._triggersDictionary.Remove(triggerName);
                flag = obj2 != null;
                if (!flag)
                {
                    return flag;
                }
                TriggerWrapper wrapper = null;
                for (int i = 0; i < this._triggers.Count; i++)
                {
                    wrapper = this._triggers[i] as TriggerWrapper;
                    if (triggerName.Equals(wrapper.Name))
                    {
                        this._triggers.RemoveAt(i);
                        break;
                    }
                }
                this._timeTriggers.Remove(wrapper);
                JobWrapper wrapper2 = this._jobsDictionary[wrapper.Trigger.JobName] as JobWrapper;
                Trigger[] triggersForJob = this.GetTriggersForJob(wrapper.Trigger.JobName);
                if (((triggersForJob == null) || (triggersForJob.Length == 0)) && deleteOrphanedJob)
                {
                    this.RemoveJob(wrapper.Trigger.JobName);
                }
            }
            return flag;
        }

        public IScheduledJob RetrieveJob(string jobName)
        {
            lock (this._jobsDictionary)
            {
                JobWrapper wrapper = this._jobsDictionary[jobName] as JobWrapper;
                return ((wrapper != null) ? wrapper.Job : null);
            }
        }

        public Trigger RetrieveTrigger(string triggerName)
        {
            lock (this._triggersDictionary)
            {
                TriggerWrapper wrapper = this._triggersDictionary[triggerName] as TriggerWrapper;
                return ((wrapper != null) ? wrapper.Trigger : null);
            }
        }

        public void Run()
        {
            bool flag = false;
            while (!this._halted)
            {
                Exception exception2;
                try
                {
                    int num2;
                    int num3;
                    object obj2;
                    TimeSpan span;
                    lock ((obj2 = this._pauseLock))
                    {
                        while (this._paused && !this._halted)
                        {
                            try
                            {
                                Monitor.Wait(this._pauseLock, 100);
                            }
                            catch (ThreadInterruptedException)
                            {
                            }
                        }
                        if (this._halted)
                        {
                            break;
                        }
                    }
                    if (this._threadPool.AvailableThreads <= 0)
                    {
                        continue;
                    }
                    Trigger trigger = null;
                    DateTime utcNow = DateTime.UtcNow;
                    this._signaled = false;
                    try
                    {
                        trigger = this.AcquireNextTrigger(utcNow.AddMilliseconds((double) this._idleWaitTime));
                        flag = false;
                    }
                    catch (Exception exception)
                    {
                        if (!flag)
                        {
                            log.Error("SchedulerThreadLoop: RuntimeException " + exception.Message, exception);
                        }
                        flag = true;
                    }
                    if (trigger != null)
                    {
                        utcNow = DateTime.UtcNow;
                        DateTime time2 = trigger.GetNextFireTimeUtc().Value;
                        span = (TimeSpan) (time2 - utcNow);
                        long num4 = (long) span.TotalMilliseconds;
                        num2 = 10;
                        num3 = (int) (num4 / ((long) num2));
                        while ((num3 >= 0) && !this._signaled)
                        {
                            try
                            {
                                Thread.Sleep(num2);
                            }
                            catch (ThreadInterruptedException)
                            {
                            }
                            utcNow = DateTime.UtcNow;
                            span = (TimeSpan) (time2 - utcNow);
                            num4 = (long) span.TotalMilliseconds;
                            num3 = (int) (num4 / ((long) num2));
                        }
                        if (this._signaled)
                        {
                            try
                            {
                                this.ReleaseAcquiredTrigger(trigger);
                            }
                            catch (Exception exception5)
                            {
                                exception2 = exception5;
                                log.Error("ReleaseAcquiredTrigger: RuntimeException " + exception2.Message, exception2);
                            }
                            this._signaled = false;
                        }
                        else
                        {
                            TriggerFiredBundle state = null;
                            lock ((obj2 = this._pauseLock))
                            {
                                if (!this._halted)
                                {
                                    try
                                    {
                                        state = this.TriggerFired(trigger);
                                    }
                                    catch (Exception exception6)
                                    {
                                        exception2 = exception6;
                                        log.Error(string.Format(CultureInfo.InvariantCulture, "RuntimeException while firing trigger {0}", new object[] { trigger.Name }), exception2);
                                    }
                                }
                                if (state == null)
                                {
                                    try
                                    {
                                        this.ReleaseAcquiredTrigger(trigger);
                                    }
                                    catch (SchedulerException)
                                    {
                                    }
                                }
                                else
                                {
                                    this._threadPool.QueueUserWorkItem(new WaitCallback(this.ProcessJob), state);
                                }
                            }
                        }
                        continue;
                    }
                    utcNow = DateTime.UtcNow;
                    DateTime time3 = utcNow.AddMilliseconds((double) this.GetRandomizedIdleWaitTime());
                    span = (TimeSpan) (time3 - utcNow);
                    long totalMilliseconds = (long) span.TotalMilliseconds;
                    num2 = 10;
                    for (num3 = (int) (totalMilliseconds / ((long) num2)); (num3 > 0) && !this._signaled; num3 = (int) (totalMilliseconds / ((long) num2)))
                    {
                        try
                        {
                            Thread.Sleep(10);
                        }
                        catch (ThreadInterruptedException)
                        {
                        }
                        utcNow = DateTime.UtcNow;
                        span = (TimeSpan) (time3 - utcNow);
                        totalMilliseconds = (long) span.TotalMilliseconds;
                    }
                }
                catch (Exception exception9)
                {
                    exception2 = exception9;
                    log.Error("Runtime error occured in main trigger firing loop.", exception2);
                }
            }
        }

        public DateTime ScheduleJob(IScheduledJob job, Trigger trigger)
        {
            if (job == null)
            {
                throw new SchedulerException("Job cannot be null", 100);
            }
            if (trigger == null)
            {
                throw new SchedulerException("Trigger cannot be null", 100);
            }
            if (trigger.JobName == null)
            {
                trigger.JobName = job.Name;
            }
            DateTime? nullable = trigger.ComputeFirstFireTimeUtc();
            if (!nullable.HasValue)
            {
                throw new SchedulerException("Based on configured schedule, the given trigger will never fire.", 100);
            }
            this.StoreJobAndTrigger(job, trigger);
            this.NotifySchedulerThread();
            return nullable.Value;
        }

        private void SetAllTriggersOfJobToState(string jobName, InternalTriggerState state)
        {
            ArrayList triggerWrappersForJob = this.GetTriggerWrappersForJob(jobName);
            foreach (TriggerWrapper wrapper in triggerWrappersForJob)
            {
                wrapper.State = state;
                if (state != InternalTriggerState.Waiting)
                {
                    this._timeTriggers.Remove(wrapper);
                }
            }
        }

        public void Shutdown()
        {
            lock (this._pauseLock)
            {
                this._halted = true;
                if (this._paused)
                {
                    Monitor.Pulse(this._pauseLock);
                }
                else
                {
                    this.SignalSchedulingChange();
                }
            }
            try
            {
                this._thread.Join();
            }
            catch (Exception)
            {
            }
        }

        internal virtual void SignalSchedulingChange()
        {
            this._signaled = true;
        }

        public void Start(ConfigurationSection configuration)
        {
        }

        public void StoreJob(IScheduledJob job, bool replaceExisting)
        {
            JobWrapper wrapper = new JobWrapper(job);
            bool flag = false;
            lock (this._jobLock)
            {
                if (this._jobsDictionary[wrapper.Name] != null)
                {
                    if (!replaceExisting)
                    {
                        throw new ArgumentException("job");
                    }
                    flag = true;
                }
                if (!flag)
                {
                    this._jobsDictionary[wrapper.Name] = wrapper;
                }
                else
                {
                    JobWrapper wrapper2 = this._jobsDictionary[wrapper.Name] as JobWrapper;
                    wrapper2.Job = wrapper.Job;
                }
            }
        }

        public void StoreJobAndTrigger(IScheduledJob job, Trigger trigger)
        {
            this.StoreJob(job, false);
            this.StoreTrigger(trigger, false);
        }

        public void StoreTrigger(Trigger trigger, bool replaceExisting)
        {
            TriggerWrapper wrapper = new TriggerWrapper(trigger);
            lock (this._triggerLock)
            {
                if (this._triggersDictionary.Contains(wrapper.Name))
                {
                    if (!replaceExisting)
                    {
                        throw new NotSupportedException("Object already exists " + trigger.Name);
                    }
                    this.RemoveTrigger(trigger.Name, false);
                }
                if (this.RetrieveJob(trigger.JobName) == null)
                {
                    throw new ApplicationException("The job (" + trigger.JobName + ") referenced by the trigger does not exist.");
                }
                this._triggers.Add(wrapper);
                this._triggersDictionary[wrapper.Name] = wrapper;
                lock (this._pausedTriggers)
                {
                    if (this._pausedTriggers.Contains(trigger.Name) || this._pausedJobs.Contains(trigger.JobName))
                    {
                        wrapper.State = InternalTriggerState.Paused;
                        if (this._blockedJobs.Contains(trigger.JobName))
                        {
                            wrapper.State = InternalTriggerState.PausedAndBlocked;
                        }
                    }
                    else if (this._blockedJobs.Contains(trigger.JobName))
                    {
                        wrapper.State = InternalTriggerState.Blocked;
                    }
                    else
                    {
                        this._timeTriggers.Add(wrapper);
                    }
                }
            }
        }

        internal virtual void TogglePause(bool pause)
        {
            lock (this._pauseLock)
            {
                this._paused = pause;
                if (this._paused)
                {
                    this.SignalSchedulingChange();
                }
                else
                {
                    Monitor.Pulse(this._pauseLock);
                }
            }
        }

        public void TriggeredJobComplete(Trigger trigger, IScheduledJob job, SchedulerInstruction triggerInstCode)
        {
            lock (this._triggerLock)
            {
                JobWrapper wrapper = this._jobsDictionary[job.Name] as JobWrapper;
                TriggerWrapper wrapper2 = this._triggersDictionary[trigger.Name] as TriggerWrapper;
                this._blockedJobs.Remove(job.Name);
                if (wrapper2 != null)
                {
                    if (triggerInstCode == SchedulerInstruction.DeleteTrigger)
                    {
                        if (!trigger.GetNextFireTimeUtc().HasValue)
                        {
                            if (!wrapper2.Trigger.GetNextFireTimeUtc().HasValue)
                            {
                                this.RemoveTrigger(trigger.Name);
                            }
                            else
                            {
                                log.Debug("Deleting cancelled - trigger still active");
                            }
                        }
                        else
                        {
                            this.RemoveTrigger(trigger.Name);
                        }
                    }
                    else if (triggerInstCode == SchedulerInstruction.SetTriggerComplete)
                    {
                        wrapper2.State = InternalTriggerState.Complete;
                        this._timeTriggers.Remove(wrapper2);
                    }
                    else if (triggerInstCode == SchedulerInstruction.SetTriggerError)
                    {
                        log.Info(string.Format(CultureInfo.InvariantCulture, "Trigger {0} set to ERROR state.", new object[] { trigger.Name }));
                        wrapper2.State = InternalTriggerState.Error;
                    }
                    else if (triggerInstCode == SchedulerInstruction.SetAllJobTriggersError)
                    {
                        log.Info(string.Format(CultureInfo.InvariantCulture, "All triggers of Job {0} set to ERROR state.", new object[] { trigger.Name }));
                        this.SetAllTriggersOfJobToState(trigger.Name, InternalTriggerState.Error);
                    }
                    else if (triggerInstCode == SchedulerInstruction.SetAllJobTriggersComplete)
                    {
                        this.SetAllTriggersOfJobToState(trigger.Name, InternalTriggerState.Complete);
                    }
                }
            }
        }

        public TriggerFiredBundle TriggerFired(Trigger trigger)
        {
            lock (this._triggerLock)
            {
                TriggerWrapper wrapper = this._triggersDictionary[trigger.Name] as TriggerWrapper;
                if ((wrapper == null) || (wrapper.Trigger == null))
                {
                    return null;
                }
                if (wrapper.State == InternalTriggerState.Complete)
                {
                    return null;
                }
                if (wrapper.State == InternalTriggerState.Paused)
                {
                    return null;
                }
                if (wrapper.State == InternalTriggerState.Blocked)
                {
                    return null;
                }
                if (wrapper.State == InternalTriggerState.PausedAndBlocked)
                {
                    return null;
                }
                DateTime? previousFireTimeUtc = trigger.GetPreviousFireTimeUtc();
                this._timeTriggers.Remove(wrapper);
                trigger.Triggered();
                wrapper.State = InternalTriggerState.Waiting;
                TriggerFiredBundle bundle = new TriggerFiredBundle(this.RetrieveJob(trigger.JobName), trigger, false, new DateTime?(DateTime.UtcNow), trigger.GetPreviousFireTimeUtc(), previousFireTimeUtc, trigger.GetNextFireTimeUtc());
                if (wrapper.Trigger.GetNextFireTimeUtc().HasValue)
                {
                    lock (this._triggerLock)
                    {
                        this._timeTriggers.Add(wrapper);
                    }
                }
                return bundle;
            }
        }

        public virtual long MisfireThreshold
        {
            get
            {
                return this._misfireThreshold;
            }
            set
            {
                if (value < 1L)
                {
                    throw new ArgumentException("Misfirethreashold must be larger than 0");
                }
                this._misfireThreshold = value;
            }
        }
    }
}

