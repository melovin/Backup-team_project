using EmailSender.Services;
using FluentEmail.Core;
using FluentEmail.Smtp;
using Quartz;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmailSender
{
    public class SenderService :IJob
    {
        private string tableTitleStyle = "border: 2px solid;";
        private string rowStyle = "border: 2px solid; padding: 7px";
        public async Task EmailSender(SettingsInfo setInfo, ApiService settingsGetter, List<TasksInfo> taskInfo)
        {
            //smtp client creation
            SmtpClient client = new SmtpClient()
            {
                Host = setInfo.SMTP,
                Port = setInfo.Port,
                EnableSsl = setInfo.SSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential()
                {
                    UserName = setInfo.Sender,
                    Password = setInfo.Password
                }
            } ;

            //template creating
            StringBuilder messageTemplate = new StringBuilder();
            messageTemplate.AppendLine($"<p>Hi Admin!</p>");
            messageTemplate.AppendLine($"<p>This is your<strong> {setInfo.Freq}</strong> report from <i>LePain a.s.</i></p>");
            messageTemplate.AppendLine("<br>");
            messageTemplate.AppendLine("<strong>HERE IS YOUR REPORT: </strong>");
            messageTemplate.AppendLine($"<ul><li>ALL TASKS: {taskInfo.Count}</li>");
            messageTemplate.AppendLine($"<li style='color:green;'>SUCCESS: {settingsGetter.SUCCESS_count}</li>");
            messageTemplate.AppendLine($"<li style='color:orange;'>NORUN: {settingsGetter.NORUN_count}</li>");
            messageTemplate.AppendLine($"<li style='color:red;'>ERROR: {settingsGetter.ERROR_count}</li></ul>");

            if(settingsGetter.ERROR_count != 0)
            {
                messageTemplate.AppendLine("<br>");
                messageTemplate.AppendLine("<p><strong>PROBLEMS: </strong></p>");
                messageTemplate.AppendLine("<br>");
                messageTemplate.AppendLine($"<table style='{this.tableTitleStyle}'><tr><th style='{this.tableTitleStyle}'>ID</th><th style='{this.tableTitleStyle}'>CONFIG</th><th style='{this.tableTitleStyle}'>CLIENT</th><th style='{this.tableTitleStyle}'>DATE</th><th style='{this.tableTitleStyle}'>MESSAGE</th></tr>");
                foreach (var item in taskInfo)
                {
                    if (item.State == State.ERROR)
                        messageTemplate.AppendLine($"<tr><td style='{this.rowStyle}'>{item.TaskId}</td><td style='{this.rowStyle}'>{item.ConfigName}</td><td style='{this.rowStyle}'>{item.ClientName}</td><td style='{this.rowStyle}'>{item.Date}</td><td style='{this.rowStyle}'>{item.Message}<td></tr>");
                }
                messageTemplate.AppendLine("</table>");
            }
            messageTemplate.AppendLine("<br>");
            messageTemplate.AppendLine("<p>Have a nice day! :)</p>");
            messageTemplate.AppendLine("<i>  -LePain a.s.</i>");

            //message creating
            MailAddress FromEmail = new MailAddress(setInfo.Sender, "Your report sender");
            MailMessage Message = new MailMessage()
            {
                IsBodyHtml = true,
                From = FromEmail,
                Subject = $"Your {setInfo.Freq} report!",
                Body = messageTemplate.ToString()
            };
            //email validation and sending
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            foreach (var item in setInfo.SendTo)
            {
                if (regex.IsMatch(item))
                    Message.To.Add(item);
            }
                client.SendCompleted += Client_SendCompleted;
                await client.SendMailAsync(Message);
        }

        private static void Client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if(e.Error != null)
            {
                Console.WriteLine("Something went wrong!");
                return;
            }
            Console.WriteLine("Email send succesfully!");
        }

        public async Task Execute(IJobExecutionContext context)
        {
            ApiService settingsGetter = new ApiService();
            SettingsInfo setinf = Application.setinf; //get email settings
            List<TasksInfo> taskInfo = await settingsGetter.GetTasks(); //get list of tasks
            settingsGetter.CountStates(taskInfo); //count number of tasks with specific state
            await this.EmailSender(setinf, settingsGetter, taskInfo); //send email
        }
    }
}
