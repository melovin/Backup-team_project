using DaemonOfPain.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonOfPain.Services
{
    public class DaemonDataService //Zápis a získávání DaemonDat ze souborů.txt
    {
        private string configPath = @"..\..\..\DaemonData\Configs.json";
        private string DaemonSettingsPath = @"..\..\..\DaemonData\DaemonSettings.json";
        private string SnapshotsPath = @"..\..\..\DaemonData\Snapshots.json";


        private List<Config> Configs = new List<Config>();
        public bool ConfigsWasChanged { get; set; }

        public DaemonDataService()
        {
            ConfigsWasChanged = false;
            StreamReader sr = new StreamReader(configPath);
            string data = sr.ReadToEnd();
            sr.Close();

            if (JsonConvert.DeserializeObject<List<Config>>(data) != null)
                this.Configs = JsonConvert.DeserializeObject<List<Config>>(data);
        }

        public void WriteSnapshot(Snapshot snap)
        {
            StreamReader sr = new StreamReader(SnapshotsPath);
            string data = sr.ReadToEnd();
            sr.Close();
            List<Snapshot> snapshots = new List<Snapshot>();
            if (data != "")
                snapshots = JsonConvert.DeserializeObject<List<Snapshot>>(data);
            if (snapshots.Exists(x => x.ConfigID == snap.ConfigID))
                snapshots.Remove(snapshots.Find(x => x.ConfigID == snap.ConfigID));
            snapshots.Add(snap);

            StreamWriter sw = new StreamWriter(SnapshotsPath);
            sw.Write(JsonConvert.SerializeObject(snapshots));
            sw.Close();
        }
        public Snapshot GetSnapshotByID(int id)
        {
            StreamReader sr = new StreamReader(SnapshotsPath);
            string data = sr.ReadToEnd();
            sr.Close();
            List<Snapshot> snapshots = JsonConvert.DeserializeObject<List<Snapshot>>(data);

            return snapshots.Find(x => x.ConfigID == id);
        }

        public List<Config> GetAllConfigs()
        {
            return this.Configs;
        }
        public Config GetConfigByID(int id)
        {
            return this.Configs.Find(x => x.Id == id);
        }


        public void WriteAllConfigs(List<Config> data)
        {
            if(JsonConvert.SerializeObject(Configs) != JsonConvert.SerializeObject(data))
            {
                Configs.Clear();
                foreach (Config newConfig in data)
                {
                    this.Configs.Add(newConfig);
                }
                StreamWriter sw = new StreamWriter(configPath);
                sw.Write(JsonConvert.SerializeObject(this.Configs));
                sw.Close();
                Application.DataService.ConfigsWasChanged = true;
            }
        }

        public void WriteSettings(Settings set)
        {
            StreamWriter sw = new StreamWriter(DaemonSettingsPath);
            sw.Write(JsonConvert.SerializeObject(set));
            sw.Close();
        }
        public Settings GetSettings()
        {
            StreamReader sr = new StreamReader(DaemonSettingsPath);
            string data = sr.ReadToEnd();
            sr.Close();
            return JsonConvert.DeserializeObject<Settings>(data);
        }
    }
}
