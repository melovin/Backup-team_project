using DaemonOfPain.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonOfPain.Services
{
    public class HeapManager
    {
        private string MetadataPackagePath = @"..\..\..\DaemonData\BackupMetadata.json";
        public List<HeapItem> meta { get; set; }
        public HeapManager()
        {
            GetHeap();
            if (meta == null)
                meta = new List<HeapItem>();
        }
        public void SaveChanges()
        {
            StreamWriter sw = new StreamWriter(MetadataPackagePath, false);
            sw.Write(JsonConvert.SerializeObject(meta));
            sw.Close();
        }
        private void GetHeap()
        {
            try
            {
                StreamReader sr = new StreamReader(MetadataPackagePath);
                string data = sr.ReadToEnd();
                sr.Close();
                meta = JsonConvert.DeserializeObject<List<HeapItem>>(data);
            }
            catch
            { }
        }

        public List<HeapItem> GetListOfBackups(string destination, int configId)
        {
            Config config = Application.DataService.GetConfigByID(configId);
            if (config.BackupType == _BackupType.FB)
            {
                return meta.Where(x => x.Destination == destination && x.ConfigID == configId).ToList();
            }
            else
            {
                int l = 0;//nejnižší číslo retence
                foreach (var item in meta.Where(x => x.Destination == destination && x.ConfigID == configId))
                {
                    int temp = item.PackageRetentionNumber;
                    if (temp > l)
                        l = temp;
                }
                return meta.Where(x => x.Destination == destination && x.ConfigID == configId && x.PackageRetentionNumber == l).ToList();
            }
        }
        public Metadata GetLastBackup(string destination, int configId)
        {
            int l = 0;//nejnižší číslo retence
            foreach (var item in meta.Where(x => x.Destination == destination && x.ConfigID == configId))
            {
                int temp = item.PackageRetentionNumber;
                if (temp > l)
                    l = temp;
            }
            int ll = 0;
            foreach (var item in meta.Where(x => x.Destination == destination && x.ConfigID == configId && x.PackageRetentionNumber == l))
            {
                int temp = item.BackupRetentionNumber;
                if (temp > ll)
                    ll = temp;
            }
            try
            {
                return meta.First(x => x.Destination == destination && x.ConfigID == configId && x.PackageRetentionNumber == l && x.BackupRetentionNumber == ll).Metadata;

            }
            catch (Exception)
            {
                return null;
            }
        }
        public Metadata GetFirstBackup(string destination, int configId)
        {
            int l = int.MaxValue;//nevyšší číslo retence
            foreach (var item in meta.Where(x => x.Destination == destination && x.ConfigID == configId))
            {
                int temp = item.PackageRetentionNumber;
                if (temp < l)
                    l = temp;
            }
            int ll = int.MaxValue;
            foreach (var item in meta.Where(x => x.Destination == destination && x.ConfigID == configId && x.PackageRetentionNumber == l))
            {
                int temp = item.BackupRetentionNumber;
                if (temp < ll)
                    ll = temp;
                if (temp == 1)
                    break;
            }
            return meta.First(x => x.Destination == destination && x.ConfigID == configId && x.PackageRetentionNumber == l && x.BackupRetentionNumber == ll).Metadata;
        }
        public int GetPackageCount(string destination, int configId)
        {
            List<int> retentions = new List<int>();
            foreach (var item in meta.Where(x => x.Destination == destination && x.ConfigID == configId))
            {
                int temp = item.PackageRetentionNumber;
                if (!retentions.Contains(temp))
                    retentions.Add(temp);
            }
            return retentions.Count;
        }
        public void DeleteOldestPackage(string destination, int configId)
        {
            int l = int.MaxValue;//nejnižší číslo retence
            foreach (var item in meta.Where(x => x.Destination == destination && x.ConfigID == configId))
            {
                int temp = item.PackageRetentionNumber;
                if (temp < l)
                    l = temp;
            }
            List<HeapItem> ItemsToDelete = meta.Where(x => x.PackageRetentionNumber == l && x.Destination == destination && x.ConfigID == configId).ToList();
            while (ItemsToDelete.Count > 0)
            {
                meta.Remove(ItemsToDelete[0]);
                ItemsToDelete.RemoveAt(0);
            }
        }
        public bool ExistConfig(int configID, string dest )
        {
            return meta.FirstOrDefault(x => x.ConfigID == configID && x.Destination == dest) != null;
        }







        //nefunguje :(
        public void Synchronization()
        {
            SynchronizationRemoveUseless();
            SynchronizationAddUnknown();
            SaveChanges();
        }
        public void SynchronizationRemoveUseless()
        {
            List<Config> configs = Application.DataService.GetAllConfigs();
            List<string> destinations = new List<string>();
            Dictionary<string, HeapItem> d = new Dictionary<string, HeapItem>();
            List<Metadata> metadata = new List<Metadata>();
            foreach (var item in meta)//převedení lokálních metadat(heapItemů) na dictionary
            {
                d.Add(item.Destination, item);
            }
            foreach (var conf in configs)//vytvoření listu jedinečných destinací
            {
                foreach (var dest in conf.Destinations)
                {
                    if (!destinations.Contains(dest.DestinationPath))
                        destinations.Add(dest.DestinationPath);
                }
            }
            foreach (var item in destinations)//prochází jedinečný list destinací a získává z nich všechny metadata
            {
                metadata.AddRange(new MetadataService().MetaSearcher(item, false));
            }
            foreach (var item in metadata)//
            {
                string path = PathReturner(item.BackupPath, 2);
                if (d.ContainsKey(path))
                    d.Remove(path);
            }
            foreach (var item in d)
            {
                meta.Remove(item.Value);
            }
        }
        public void SynchronizationAddUnknown()
        {

        }
        private string PathReturner(string path, int steps)//již funkční//vrátí se o určitý počet složek zpět. př. PathReturner(@"C:\Users\František\Desktop\",2) se vrátí o dvě složky zpět - vrátí => C:\Users
        {
            string[] parts = path.Split("\\");
            for (int i = 0; i < steps; i++)
            {
                parts[parts.Length - i - 1] = "";
            }
            string st = string.Join("\\", parts);
            return st.Trim('\\');
        }
    }
}
