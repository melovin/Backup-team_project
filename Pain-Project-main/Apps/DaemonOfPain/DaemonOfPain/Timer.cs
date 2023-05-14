using DaemonOfPain.Encryption;
using DaemonOfPain.Services;
using DaemonOfPain.Services.APIClasses;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonOfPain
{
    public class Timer
    {
        public IScheduler scheduler { get; set; }

        public async Task SetUp(List<Config> configs)
        {
            if (scheduler != null)
                await scheduler.Clear();
            scheduler = await new StdSchedulerFactory().GetScheduler();
            await ClientPrepare();
            await StartTimer(configs);
        }

        private async Task ClientPrepare()
        {
            //update tabulky tasků
            IJobDetail updateTaskJob = JobBuilder.Create<TaskManager>().Build();
            ITrigger trigger = TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInHours(1).RepeatForever()).Build();
            await scheduler.ScheduleJob(updateTaskJob, trigger);

            //dotazování na server
            IJobDetail connectJob = JobBuilder.Create<APIService>().Build();
            ITrigger connectTrigger = TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever()).Build();
            await scheduler.ScheduleJob(connectJob, connectTrigger);

            //odeslání neodeslaných reportů
            IJobDetail reportJob = JobBuilder.Create<ReportHolder>().Build();
            ITrigger reportTrigger = TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever()).Build();
            await scheduler.ScheduleJob(reportJob, reportTrigger);

            ////šifrování - vygenerování nového klíče
            //IJobDetail encryptJob = JobBuilder.Create<EncryptionKeysManager>().Build();
            //ITrigger encryptTrigger = TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever()).Build();
            //await scheduler.ScheduleJob(encryptJob, encryptTrigger);

            //////Dotazování na public key serveru
            //IJobDetail keyServerJob = JobBuilder.Create<ServerKeyAsker>().Build();
            //ITrigger keyServerTrigger = TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInHours(1).RepeatForever()).Build();
            //await scheduler.ScheduleJob(keyServerJob, keyServerTrigger);
        }

        private async Task StartTimer(List<Config> configs)
        {
            if (configs == null)
                return;
            string cron = "";
            foreach (var item in configs)
            {
                IJobDetail job = JobBuilder.Create<BackupService>().Build();
                ITrigger trigger = TriggerBuilder.Create().WithCronSchedule(CronCorrector(item.Cron)).Build();
                cron = CronCorrector(item.Cron);
                await scheduler.ScheduleJob(job, trigger);
            }
            await scheduler.Start();
        }

        private string CronCorrector(string cron)
        {
            string[] parts = cron.Split(' ');
            if (parts[2] == "*" || parts[4] == "*") { parts[2] = "?"; }
            else if (parts[2] != "*" || parts[4] == "*") { parts[4] = "?"; }
            else if (parts[2] == "*" || parts[4] != "*") { parts[2] = "?"; }
            else if (parts[2] != "*" || parts[4] != "*") { parts[4] = "?"; }
            return "0 " + String.Join(' ', parts).Trim();
        }

    }
}
