using EmailSender.Services;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender
{
    public class Timer
    {
        public IScheduler Scheduler { get; set; }

        public async Task SetUp(SettingsInfo setInf)
        {
            if (Scheduler != null)
                await Scheduler.Clear();
            Scheduler = await new StdSchedulerFactory().GetScheduler();
            await this.StartTimer(setInf);

        }
        private async Task StartTimer(SettingsInfo setInf)
        {
            if (setInf.Freq == Freq.DAILY)
            {
                IJobDetail job = JobBuilder.Create<SenderService>().Build();
                ITrigger trigger = TriggerBuilder.Create().WithCronSchedule("0 0 0 ? * *").Build();
                //ITrigger trigger = TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInSeconds(30).RepeatForever()).Build();
                await Scheduler.ScheduleJob(job, trigger);
            }
            else if (setInf.Freq == Freq.WEEKLY)
            {
                IJobDetail job = JobBuilder.Create<SenderService>().Build();
                ITrigger trigger = TriggerBuilder.Create().WithCronSchedule("0 0 0 ? * 1").Build();
                //ITrigger trigger = TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInSeconds(30).RepeatForever()).Build();
                await Scheduler.ScheduleJob(job, trigger);
            }
            else if (setInf.Freq == Freq.MONTHLY)
            {
                IJobDetail job = JobBuilder.Create<SenderService>().Build();
                ITrigger trigger = TriggerBuilder.Create().WithCronSchedule("0 0 0 1 ? *").Build();
                await Scheduler.ScheduleJob(job, trigger);
            }
            IJobDetail reqJob = JobBuilder.Create<Application>().Build();
            ITrigger reqTrigger = TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInHours(1).RepeatForever()).Build();
            await Scheduler.ScheduleJob(reqJob, reqTrigger);

            await Scheduler.Start();
        }
     }
}
