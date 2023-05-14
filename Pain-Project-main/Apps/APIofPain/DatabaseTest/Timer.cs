using DatabaseTest.Encryption;
using Quartz;
using Quartz.Impl;
using System;


namespace DatabaseTest.DatabaseTables
{
    public class Timer
    {
        public IScheduler scheduler { get; set; }

        public async System.Threading.Tasks.Task SetUp()
        {
            scheduler = await new StdSchedulerFactory().GetScheduler();

            IJobDetail job = JobBuilder.Create<Tasker>().Build();
            ITrigger trigger = TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInHours(1).RepeatForever()).Build();
            await scheduler.ScheduleJob(job, trigger);

            //zmìna klíèe
            //IJobDetail jobEncrypt = JobBuilder.Create<EncryptionKeysManager>().Build();
            //ITrigger triggerEncrypt = TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever()).Build();
            //await scheduler.ScheduleJob(jobEncrypt, triggerEncrypt);

            await scheduler.Start();
        }

    }
}
