using EmailSender.Services;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmailSender
{
    public class Application : IJob
    {
        public static ApiService apiService { get; set; }
        public static SettingsInfo setinf { get; set; }
        public static Timer Timer { get; set; }
        public Application()
        {
            if (apiService == null)
                apiService = new ApiService();
            if (Timer == null)
                Timer = new Timer();
        }
        public async Task Start()
        {
            setinf = await apiService.GetInfo();
            await Timer.SetUp(setinf);
            while (true)
            {
                Thread.Sleep(5000);
            }
        }
        public async Task Execute(IJobExecutionContext context)
        {
            setinf = await apiService.GetInfo();
        }
    }
}
