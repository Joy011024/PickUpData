using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System.Reflection;
namespace CaptureWebData
{
    /*
     必备程序集
     Common.Logging, Version=3.3.1.0
     Common.Logging.Core, Version=3.3.1.0
     */
    public class QuartzJob
    {
        static IScheduler scheduler { get; set; }
        public static IScheduler GetScheduler()
        {
            if (scheduler != null)
            {
                return scheduler;
            }
            else
            {
                ISchedulerFactory schedf = new StdSchedulerFactory();
                IScheduler sched = schedf.GetScheduler();
                return sched;
            }
        }
        public void CreateJob<T>(int interval, int repeactCount) where T : IJob //调用的实体需要继承IJob
        {
            // Console.WriteLine(DateTime.Now.ToString());
            ISchedulerFactory schedf = new StdSchedulerFactory();//pool
            scheduler = schedf.GetScheduler();
            string jobName = typeof(T).Name;
            string group = new ConfigurationItems().AppNameEng;
            IJobDetail job = JobBuilder.Create<T>().WithIdentity(jobName, group).Build();//job
            ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create().
                WithSimpleSchedule(x => x.WithIntervalInSeconds(interval).
                    WithRepeatCount(repeactCount)).Build();
            //如果存在该作业，进行替换
            JobKey jy = new JobKey(jobName, group);
            if (scheduler.GetJobDetail(jy) == null)
            {
                scheduler.ScheduleJob(job, trigger);//in pool
                scheduler.Start();
                scheduler.PauseJob(jy);
            }
            //执行多少次之后结束
            else
            {
                
               // scheduler.DeleteJob(jy);
            }
        }
        public void CreateJobWithParam<T>(object param, DateTime start, int interval, int repeat) where T : IJob
        {
            CreateJobWithParam<T>(param, start, interval, repeat, new ConfigurationItems().AppNameEng);
        }
        public void CreateJobWithParam<T>(object param, DateTime start, int interval, int repeat, string jobGroup) where T : IJob
        {
            ISchedulerFactory schedf = new StdSchedulerFactory();//pool
            scheduler = schedf.GetScheduler();
            string jobName = typeof(T).FullName;
            JobKey key;
            if (string.IsNullOrEmpty(jobGroup))
            {
                key = new JobKey(jobName);
            }
            else 
            {
                key = new JobKey(jobName, jobGroup);
            }
            IJobDetail job = JobBuilder.Create<T>().WithIdentity(key).Build();
            job.JobDataMap.Put("QuarztParam", param);
            //IJobListener jobListence = new DefineJobListener();
            //加上监听之后无效
            //IMatcher<JobKey> match = KeyMatcher<JobKey>.KeyEquals(job.Key); // Quartz.Impl.Matchers.KeyMatcher<TKey>
            //scheduler.ListenerManager.AddJobListener(jobListence, match);
            DateTimeOffset startSet=DateTime.SpecifyKind(start,DateTimeKind.Utc);
            if (repeat == 0)
            {
                repeat = int.MaxValue;
            }
            if (scheduler.GetJobDetail(key)!=null) 
            {
                scheduler.DeleteJob(key);
            }
            ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create().
                //StartAt(startSet).
                WithSimpleSchedule(x=>x.WithIntervalInSeconds(interval).WithRepeatCount(repeat)).//触发事件间隔，触发次数
                Build();
            //.WithSimpleSchedule(x => x.WithIntervalInSeconds(5).WithRepeatCount(100)) .Build();
            scheduler.ScheduleJob(job, trigger);
            scheduler.Start();
        }
        public void RunJob<T>() where T:class
        {
            object job= scheduler.GetJobDetail(new JobKey(typeof(T).Name));
            scheduler.ResumeJob(new JobKey(typeof(T).Name));
        } 
        public void StopAllJob() 
        {
            if (scheduler == null) { }
            scheduler.PauseAll();
        }
        public void StartJob(string jobName) 
        {
            scheduler.ResumeJob(new JobKey(jobName));
        }
        private void GetAllJobName()
        {
            List<string> group = scheduler.GetJobGroupNames().ToList();//默认为default
            
        }
    }
    /// <summary>
    /// 调用的对象T必须具备一个带有object类型参数的构造函数
    /// 在对象中处理具体的逻辑
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JobAction<T> : IJob where T:class
    {
        public void Execute(IJobExecutionContext context)
        {
            JobKey jobKey = context.JobDetail.Key;
            // 获取传递过来的参数            
            JobDataMap data = context.JobDetail.JobDataMap;
            object objParam = data.Get("QuarztParam");
            Type t = typeof(T);
            ConstructorInfo havaCon = t.GetConstructor(new Type[] { typeof(object) });
            object obj = (T)havaCon.Invoke(new object[] { objParam });
        }
    }
}
