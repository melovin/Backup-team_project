using DatabaseTest.Controllers;
using DatabaseTest.DatabaseTables;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseTest.Encryption;

namespace DatabaseTest
{
    public class Program
    {
        public static async System.Threading.Tasks.Task Main(string[] args)
        {
            new Tasker().RegenerateTaskDatabaseTable();
            Timer timer = new Timer();
            await timer.SetUp();
            EncryptionKeysManager.NewKeys();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
