using DaemonOfPain.Components;
using DaemonOfPain.Encryption;
using DaemonOfPain.Services;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DaemonOfPain
{
    public class Application
    {
        public Timer Timer { get; set; }
        public static DaemonDataService DataService { get; set; }
        public TaskManager TaskManager { get; set; }
        public static string IdOfThisClient { get; set; }
        public Application()
        {
            DataService = new DaemonDataService();
            Timer = new Timer();
            TaskManager = new TaskManager();
        }
        public async Task StartApplication()
        {
            Settings set = DataService.GetSettings();
            if (set == null)
                Console.WriteLine("První spuštění!");
            else
            {
                EncryptionKeysManager.NewKeys();
                EncryptionKeysManager.ServerKey = await APIService.GetServerPublicKey();
            }
            while (set == null)
            {
                try
                {
                    Console.WriteLine("Navazuji spojení s databází...");
                    EncryptionKeysManager.NewKeys();
                    EncryptionKeysManager.ServerKey = await APIService.GetServerPublicKey();
                    string newId = await APIService.LoginToServer();
                    DataService.WriteSettings(new Settings() { Id = newId });
                    IdOfThisClient = newId;
                    set = DataService.GetSettings();
                    Console.WriteLine("Připojeno.");
                }
                catch
                {
                    Console.WriteLine("Navázání spojení se serverem bylo neúspěšné!");
                    Console.WriteLine("Další pokus za 15 sekund.");
                    Thread.Sleep(15000);
                }
            }
            IdOfThisClient = set.Id;

            await APIService.GetConfigs();

            TaskManager.UpdateTaskList(DataService.GetAllConfigs());
            await Timer.SetUp(DataService.GetAllConfigs());

            while (true)
            {
                if (DataService.ConfigsWasChanged)
                {
                    string newId = await APIService.LoginToServer();
                    DataService.WriteSettings(new Settings() { Id = newId });
                    IdOfThisClient = newId;
                    if (newId == null)
                        throw new Exception();//nelze se spojit s databází
                }
                else
                {
                    IdOfThisClient = set.Id;
                }

                await APIService.GetConfigs();

                TaskManager.UpdateTaskList(DataService.GetAllConfigs());
                await Timer.SetUp(DataService.GetAllConfigs());

                while (true)
                {
                    if (DataService.ConfigsWasChanged)
                    {
                        DataService.ConfigsWasChanged = false;
                        await Timer.SetUp(DataService.GetAllConfigs());
                    }

                    Thread.Sleep(5000);
                }
            }
            catch (Exception)
            {
                Console.Clear();
                Console.WriteLine("Couldn't connect to API service");
                Console.WriteLine("Trying again in 5 seconds!");
                Thread.Sleep(5000);
                await StartApplication();
            }

        }
    }
}
