using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.Services
{
    public class ApiService
    {
        public int SUCCESS_count { get; set; }
        public int NORUN_count { get; set; }
        public int ERROR_count { get; set; }
        public async Task<SettingsInfo> GetInfo()
        {
            HttpClient client = new HttpClient();

            string settings = await client.GetStringAsync(@"https://localhost:5001/email/getSettings");
            SettingsInfo emailSettings = JsonConvert.DeserializeObject<SettingsInfo>(settings);

            string emails = await client.GetStringAsync(@"https://localhost:5001/Email/getEmails");
            emailSettings.SendTo = JsonConvert.DeserializeObject<List<string>>(emails);

            if (JsonConvert.SerializeObject(Application.setinf) != JsonConvert.SerializeObject(emailSettings))
            {
                await Application.Timer.SetUp(emailSettings);
            }

            return emailSettings;
        }   
        public async Task<List<TasksInfo>> GetTasks()
        {
            HttpClient client = new HttpClient();
            string tasks = await client.GetStringAsync(@"https://localhost:5001/Email/getReports");
            List<TasksInfo> taskInfo = JsonConvert.DeserializeObject<List<TasksInfo>>(tasks);
            return taskInfo;
        }
        public void CountStates(List<TasksInfo> taskInfo)
        {
            foreach (var item in taskInfo)
            {
                if (item.State == State.SUCCESS)
                    this.SUCCESS_count++;
                else if (item.State == State.NORUN)
                    this.NORUN_count++;
                else
                    this.ERROR_count++;
            }
        }
    }
}
