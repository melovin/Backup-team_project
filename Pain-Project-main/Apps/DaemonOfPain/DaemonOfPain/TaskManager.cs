using DaemonOfPain.Services;
using NCrontab;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonOfPain
{
    public class TaskManager : IJob
    {
        public static List<Tasks> TaskList { get; set; } = new List<Tasks>();

        public TaskManager()
        {
            TaskList = new List<Tasks>();
        }

        public void UpdateTaskList (List<Config> configs)
        {
            if (configs == null)
                return;
            TaskList.Clear();
            //return schedule.GetNextOccurrence(DateTime.Now);
            foreach (var config in configs)
            {
                int id = config.Id;
                var schedule = CrontabSchedule.Parse(config.Cron);
                foreach (var date in schedule.GetNextOccurrences(DateTime.Now, DateTime.Now.AddHours(3)))
                {
                    TaskList.Add(new Tasks() { Date = date, IdConfig = id });
                }
            }
            TaskList.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
        }

        public Task Execute(IJobExecutionContext context)
        {
            this.UpdateTaskList(Application.DataService.GetAllConfigs());
            Console.WriteLine("task");
            return Task.CompletedTask;
        }
    }
}
