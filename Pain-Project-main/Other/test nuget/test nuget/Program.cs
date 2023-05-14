using System;
using Quartz;
using NCrontab;
using System.Linq;
using Quartz.Impl;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Cronos;

namespace test_nuget
{
    class Program
    {
        static async Task Main(string[] args)
        {

            // get a scheduler
            IScheduler scheduler = await new StdSchedulerFactory().GetScheduler();
            //await scheduler.Start();

            // define the job and tie it to our HelloJob class
            IJobDetail cronJob = JobBuilder.Create<MyJob>()
                //.WithIdentity("cron")
                .Build();

            IJobDetail job = JobBuilder.Create<HelloJob>()
                //.WithIdentity("Job")
                .Build();

            //List<ITrigger> triggers = new List<ITrigger>();

            //triggers.Add(TriggerBuilder.Create().WithCronSchedule("40 * * * * ?").Build());
            //triggers.Add(TriggerBuilder.Create().WithCronSchedule("10 * * * * ?").Build());



            // Trigger the job to run now, and then every 40 seconds
            ITrigger trigger2 = TriggerBuilder.Create().WithCronSchedule("20,25,30,35 * * ? * *").Build();


            ITrigger trigger3 = TriggerBuilder.Create()
    //.WithIdentity("myTrigger")
    //.StartNow()
    .WithSimpleSchedule(x => x
        .WithIntervalInSeconds(10)
        .RepeatForever())
.Build();

            await scheduler.ScheduleJob(cronJob, trigger2);
            await scheduler.ScheduleJob(job, trigger3);





            //   var sched = await SchedulerBuilder.Create()
            //    // default max concurrency is 10
            //    .UseDefaultThreadPool(x => x.MaxConcurrency = 5)
            //    // this is the default 
            //    // .WithMisfireThreshold(TimeSpan.FromSeconds(60))
            //    .UsePersistentStore(x =>
            //    {
            //      // force job data map values to be considered as strings
            //     // prevents nasty surprises if object is accidentally serialized and then 
            //     // serialization format breaks, defaults to false
            //     x.UseProperties = true;
            //        x.UseClustering();
            //        x.UseSqlServer("my connection string");
            //        // this requires Quartz.Serialization.Json NuGet package
            //     x.UseJsonSerializer();
            //    })
            //    // job initialization plugin handles our xml reading, without it defaults are used
            //    // requires Quartz.Plugins NuGet package
            //    .UseXmlSchedulingConfiguration(x =>
            //    {
            //     x.Files = new[] { "~/quartz_jobs.xml" };
            //     // this is the default
            //     x.FailOnFileNotFound = true;
            //     // this is not the default
            //     x.FailOnSchedulingError = true;
            //    })
            //    .BuildScheduler();

            await scheduler.Start();
            while(true)
                Thread.Sleep(10000);
            //await scheduler.Shutdown();



            //var s = CrontabSchedule.Parse("0 5 * * *");
            //var start = new DateTime(2020, 2, 5);
            //var end = start.AddMonths(1);
            //var occurrences = s.GetNextOccurrences(start, end);
            //foreach (var item in occurrences)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine(string.Join(Environment.NewLine,from t in occurrences select $"{t:ddd, dd MMM yyyy HH:mm}"));
        }
    }
}
