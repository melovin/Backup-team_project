using DaemonOfPain;
using DatabaseTest.Controllers;
using Microsoft.AspNetCore.Mvc;
using NCrontab;
using Quartz;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DatabaseTest.DatabaseTables
{
    public class Tasker : IJob
    {
        private MyContext context = new MyContext();
        public System.Threading.Tasks.Task Execute(IJobExecutionContext context)
        {
            UpdateTaskDatabaseTable();
            return System.Threading.Tasks.Task.CompletedTask;
        }
        public void RegenerateTaskDatabaseTable()
        {
            try
            {
                DateTime today = DateTime.Now;
                var del = from t in context.Tasks
                          where t.Date > today
                          select t;
                foreach (Task item in del)
                {
                    context.Tasks.Remove(item);
                }
                context.SaveChanges();
                UpdateTaskDatabaseTable();
            }
            catch { }
        }
        public void UpdateTaskDatabaseTable()
        {
            try
            {
                //odtranění tasku, které jsou starší 8 dní
                DateTime today = DateTime.Now;
                var del = from t in context.Tasks
                          where t.Date <= today.AddDays(-8)
                          select t;
                foreach (Task item in del)
                {
                    context.Tasks.Remove(item);
                }
                

                //získání tasku z databáze, který je poslední
                Task newest = (from t in context.Tasks
                               orderby t.Date descending
                               where t.Date >= today
                               select t).FirstOrDefault();

                 //vytvoření listu tasků, které po posledním tasku následují(nevíce 30 hodin dopředu)
                 List<Tasks> TaskList = new List<Tasks>();
                List<Assignment> assignments = context.Assignments.ToList();//je nutné to dát takto do listu, protože ve foreach níže musím context použít na získání cronu(jinak by vznikla chyba v tom, že se snažím otevřít dvě spojení s databází najednou)
                foreach (var assignment in assignments)
                {
                    var schedule = CrontabSchedule.Parse(context.Configs.Where(x=> x.Id == assignment.IdConfig).FirstOrDefault().Cron);
                    DateTime startDate;
                    if (newest == null) { startDate = DateTime.Now; }
                    else { startDate = newest.Date; }
                    foreach (var date in schedule.GetNextOccurrences(startDate, DateTime.Now.AddHours(30)))
                    {
                        TaskList.Add(new Tasks() { Date = date,  IdAssignment = assignment.Id});
                    }
                }
                TaskList.Sort((x, y) => DateTime.Compare(x.Date, y.Date));


                //přidání nových tasků do databáze 
                foreach (var item in TaskList)
                {
                    context.Tasks.Add(new Task() { IdAssignment = item.IdAssignment, Date = item.Date, State = "NoRun" });
                }
                context.SaveChanges();
            }
            catch (Exception)
            { }
        }
    }
}
