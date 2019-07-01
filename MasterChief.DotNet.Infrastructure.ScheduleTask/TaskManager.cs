using System;
using System.Collections.Generic;
using MasterChief.DotNet4.Utilities.Operator;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace MasterChief.DotNet.Infrastructure.ScheduleTask
{
    /// <summary>
    /// 基于Quartz的计划任务实现
    /// </summary>
    public static class TaskManager
    {
        #region Constructors

        static TaskManager()
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            Scheduler = schedulerFactory.GetScheduler();
            Scheduler.Start();
        }

        #endregion Constructors

        #region Fields

        private const string Crontriggerprefix = "_CronTrigger";
        private const string Groupprefix = "_Group";
        private const string Simpletriggerprefix = "_SimpleTrigger";
        private const string Triggergroupprefix = "_TriggerGroup";

        /// <summary>
        ///     作业调度器
        /// </summary>
        public static readonly IScheduler Scheduler;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     添加任务类型作业
        /// </summary>
        /// <param name="jobName">作业名称</param>
        /// <param name="cronTime">Cron 表达式</param>
        /// <param name="jobDataKey">作业数据Key</param>
        /// <param name="jobDataValue">作业数据Value</param>
        public static void AddCron<T>(string jobName, string cronTime, string jobDataKey, string jobDataValue)
            where T : IJob
        {
            ValidateOperator.Begin()
                .NotNullOrEmpty(jobName, "作业名称")
                .NotNullOrEmpty(cronTime, "Cron 表达式");
            var jobDetail = JobBuilder.Create<T>().WithIdentity(jobName, string.Concat(jobName, Groupprefix))
                .UsingJobData(jobDataKey, jobDataValue)
                .Build(); //创建任务实例
            ICronTrigger cronTrigger = new CronTriggerImpl(string.Concat(jobName, Crontriggerprefix),
                string.Concat(jobName, Triggergroupprefix), cronTime); //创建触发器实例
            Scheduler.ScheduleJob(jobDetail, cronTrigger); //绑定触发器和任务
        }

        /// <summary>
        ///     添加任务类型作业
        /// </summary>
        /// <param name="jobName">作业名称</param>
        /// <param name="cronTime">Cron 表达式</param>
        /// <param name="jobDataValue">作业数据</param>
        public static void AddCron<T>(string jobName, string cronTime, string jobDataValue)
            where T : IJob
        {
            AddCron<T>(jobName, cronTime, "jobData", jobDataValue);
        }

        /// <summary>
        ///     添加任务类型作业
        /// </summary>
        /// <param name="jobName">作业名称</param>
        /// <param name="cronTime">Cron 表达式</param>
        public static void AddCron<T>(string jobName, string cronTime)
            where T : IJob
        {
            AddCron<T>(jobName, cronTime, null);
        }

        /// <summary>
        ///     添加周期类型作业
        /// </summary>
        /// <param name="jobName">作业名称</param>
        /// <param name="simpleTime">作业间隔【毫秒】</param>
        public static void AddSimple<T>(string jobName, int simpleTime)
            where T : IJob
        {
            AddSimple<T>(jobName, DateTime.UtcNow.AddMilliseconds(1), TimeSpan.FromMilliseconds(simpleTime));
        }

        /// <summary>
        ///     添加周期类型作业
        /// </summary>
        /// <param name="jobName">作业名称</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="simpleTime">作业间隔</param>
        public static void AddSimple<T>(string jobName, DateTimeOffset startTime, int simpleTime)
            where T : IJob
        {
            AddSimple<T>(jobName, startTime, TimeSpan.FromMilliseconds(simpleTime));
        }

        /// <summary>
        ///     添加周期类型作业
        /// </summary>
        /// <param name="jobName">作业名称</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="simpleTime">作业间隔</param>
        public static void AddSimple<T>(string jobName, DateTimeOffset startTime, TimeSpan simpleTime)
            where T : IJob
        {
            AddSimple<T>(jobName, startTime, simpleTime, new Dictionary<string, object>());
        }

        /// <summary>
        ///     添加周期类型作业
        /// </summary>
        /// <param name="jobName">作业名称</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="simpleTime">作业间隔</param>
        /// <param name="jobDataKey">作业数据key.</param>
        /// <param name="jobDataValue">作业数据value.</param>
        public static void AddSimple<T>(string jobName, DateTimeOffset startTime, int simpleTime, string jobDataKey,
            object jobDataValue)
            where T : IJob
        {
            var jobDataMap = new Dictionary<string, object>
            {
                {jobDataKey, jobDataValue}
            };
            AddSimple<T>(jobName, startTime, TimeSpan.FromMilliseconds(simpleTime), jobDataMap);
        }

        /// <summary>
        ///     添加周期类型作业
        /// </summary>
        /// <param name="jobName">作业名称</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="simpleTime">作业间隔</param>
        /// <param name="jobmapData">作业数据</param>
        public static void AddSimple<T>(string jobName, DateTimeOffset startTime, TimeSpan simpleTime,
            Dictionary<string, object> jobmapData)
            where T : IJob
        {
            ValidateOperator.Begin()
                .NotNullOrEmpty(jobName, "作业名称");
            var jobDetail = JobBuilder.Create<T>().WithIdentity(jobName, string.Concat(jobName, Groupprefix)).Build();
            jobDetail.JobDataMap.PutAll(jobmapData);
            ISimpleTrigger triggerCheck = new SimpleTriggerImpl(string.Concat(jobName, Simpletriggerprefix),
                string.Concat(jobName, Triggergroupprefix),
                startTime,
                null,
                SimpleTriggerImpl.RepeatIndefinitely,
                simpleTime);
            Scheduler.ScheduleJob(jobDetail, triggerCheck);
        }

        /// <summary>
        ///     删除Job
        /// </summary>
        /// <param name="jobName">作业名称</param>
        public static void Delete(string jobName)
        {
            ValidateOperator.Begin()
                .NotNullOrEmpty(jobName, "作业名称");
            var jobKey = new JobKey(jobName, jobName + Groupprefix);
            if (!Scheduler.CheckExists(jobKey)) throw new ArgumentException($"未找到：{jobName}作业.");
            Scheduler.DeleteJob(jobKey);
        }

        /// <summary>
        ///     修改任务周期类型的作业
        /// </summary>
        /// <param name="jobName">作业名称</param>
        /// <param name="cronTime">cron表达式</param>
        /// <exception cref="System.ArgumentException">未找到：{jobName}</exception>
        public static void ModifyCronTrigger(string jobName, string cronTime)
        {
            ValidateOperator.Begin()
                .NotNullOrEmpty(jobName, "作业名称")
                .NotNullOrEmpty(cronTime, "cron表达式");
            var triggerKey = new TriggerKey(string.Concat(jobName, Crontriggerprefix),
                string.Concat(jobName, Triggergroupprefix));
            var cronTrigger = Scheduler.GetTrigger(triggerKey) as CronTriggerImpl;
            if (cronTrigger == null) throw new ArgumentException($"未找到：{jobName}对应CronTriggerImpl类型作业.");

            cronTrigger.CronExpression = new CronExpression(cronTime);
            Scheduler.RescheduleJob(triggerKey, cronTrigger);
        }

        /// <summary>
        ///     修改周期性类型的作业
        /// </summary>
        /// <param name="jobName">作业名称</param>
        /// <param name="simpleTime">时间间隔[单位秒]</param>
        public static void ModifySimpleTrigger(string jobName, int simpleTime)
        {
            ModifySimpleTrigger(jobName, TimeSpan.FromMinutes(simpleTime));
        }

        /// <summary>
        ///     修改 周期性类型的作业
        /// </summary>
        /// <param name="jobName">作业名称</param>
        /// <param name="simpleTime">时间间隔</param>
        public static void ModifySimpleTrigger(string jobName, TimeSpan simpleTime)
        {
            ValidateOperator.Begin()
                .NotNullOrEmpty(jobName, "作业名称");
            var trigger = new TriggerKey(string.Concat(jobName, Simpletriggerprefix),
                string.Concat(jobName, Triggergroupprefix));
            var simpleTrigger = Scheduler.GetTrigger(trigger) as SimpleTriggerImpl;
            if (simpleTrigger == null) throw new ArgumentException($"未找到：{jobName}对应SimpleTriggerImpl类型作业.");

            simpleTrigger.RepeatInterval = simpleTime;
            Scheduler.RescheduleJob(trigger, simpleTrigger);
        }

        /// <summary>
        ///     暂停所有Job
        /// </summary>
        public static void PauseAll()
        {
            Scheduler.PauseAll();
        }

        /// <summary>
        ///     恢复所有Job
        /// </summary>
        public static void ResumeAll()
        {
            Scheduler.ResumeAll();
        }

        /// <summary>
        ///     卸载定时器
        /// </summary>
        /// <param name="waitForJobsToComplete">是否等待job执行完成</param>
        public static void Shutdown(bool waitForJobsToComplete)
        {
            Scheduler.Shutdown(waitForJobsToComplete);
        }

        #endregion Methods
    }
}