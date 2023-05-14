using DaemonOfPain.Components;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DaemonOfPain.Services
{
    [DisallowConcurrentExecution]
    public class BackupService : IJob
    {
        private MetadataService MdataService = new MetadataService();
        private List<Metadata> SubCompleteMlist = new List<Metadata>();
        private HeapManager HeapManager = new HeapManager();

        private int GetConfigId(List<Tasks> tasks)
        {
            int id = tasks[0].IdConfig;
            return id;
        }
        private void ConfigCheck(Config config)
        {
            string[] parts = config.Cron.Split(' ');
            if (parts.Length != 5)
                throw new Exception("Invalid cron!");

            if (!Regex.IsMatch(parts[0], @"(^\*$)|(^[0-5]?[0-9](-[0-5]?[0-9])?$)|(^[0-5]?[0-9]/[0-9]*$)|(^[0-5]?[0-9](,[0-5]?[0-9])+$)"))
                throw new Exception("Invalid cron!");
            if (!Regex.IsMatch(parts[1], @"(^\*$)|(^(([0-1]?[0-9])|(2[0-3]))(-(([0-1]?[0-9])|(2[0-3])))?$)|(^(([0-1]?[0-9])|(2[0-3]))/[0-9]*$)|(^(([0-1]?[0-9])|(2[0-3]))(,(([0-1]?[0-9])|(2[0-3])))+$)"))
                throw new Exception("Invalid cron!");
            if (!Regex.IsMatch(parts[2], @"(^\*$)|(^(([0-2]?[0-9])|(3[0-1]))(-(([0-2]?[0-9])|(3[0-1])))?$)|(^(([0-2]?[0-9])|(3[0-1]))/[0-9]*$)|(^(([0-2]?[0-9])|(3[0-1]))(,(([0-2]?[0-9])|(3[0-1])))+$)"))
                throw new Exception("Invalid cron!");
            if (!Regex.IsMatch(parts[3], @"(^\*$)|(^(([0-9])|(1[0-2]))(-(([0-9])|(1[0-2])))?$)|(^(([0-9])|(1[0-2]))/[0-9]*$)|(^(([0-9])|(1[0-2]))(,(([0-9])|(1[0-2])))+$)"))
                throw new Exception("Invalid cron!");
            if (!Regex.IsMatch(parts[4], @"(^\*$)|(^[0-6](-[0-6])?$)|(^[0-6]/[0-9]*$)|(^[0-6](,[0-6])+$)"))
                throw new Exception("Invalid cron!");

            if (config.Retention[0] <= 0 || config.Retention[1] <= 0)
                throw new Exception("Invalid retention!");

            foreach (var item in config.Sources)
            {
                if (!Directory.Exists(item))
                    throw new Exception("Source: " + item + " does not exist!");
            }

            List<string> checkList = new List<string>();
            foreach (var item in config.Sources)
            {
                string[] dirs = item.Split('\\');
                if (checkList.Contains(dirs[dirs.Length - 1]))
                    throw new Exception("A duplicate source folder name: " + item);
                checkList.Add(item);
            }
        }
        public void BackupSetup(List<Tasks> tasks)
        {
            Config config = Application.DataService.GetConfigByID(GetConfigId(tasks));
            ConfigCheck(config);


            foreach (var item in config.Destinations)
            {
                Service service = new Service(item);
                Prepare(service, config, service._destPath);
            }

            HeapManager.SaveChanges();

        }
        private void Prepare(Service sr, Config config, string path)
        {
            string configPath = path + '/' + config.Name;
            string backupPath = "";

            if (HeapManager.ExistConfig(config.Id, path))
            {
                Metadata firstMdata = HeapManager.GetFirstBackup(path, config.Id);
                Metadata lastMdata = HeapManager.GetLastBackup(path, config.Id);

                if (config.Retention[1] <= HeapManager.GetListOfBackups(path, config.Id).Count)//Je počet získaných metadat roven hodnotě v "Retention[1]" ?
                {
                    if (config.Retention[0] <= HeapManager.GetPackageCount(path, config.Id))//Je počet získaných metadat roven hodnotě v "Retention[0]" ?
                    {
                        sr.DeleteDir(firstMdata.BackupPath);
                        HeapManager.DeleteOldestPackage(path, config.Id);
                    }
                }
                else//místo je, není nutné nic odstraňovat, v diagramu => "Toto je nyní "Aktuální složka" pro zálohu"
                {
                    if (firstMdata.BackupType == _BackupType.FB)
                        throw new Exception();
                    backupPath = lastMdata.BackupPath;
                }
            }
            else
            {
                if (!sr.DirExists(configPath))
                    sr.CreateDir(configPath);
            }

            if (backupPath == "")
            {
                Snapshot snapshot = new Snapshot() { ConfigID = config.Id };
                Application.DataService.WriteSnapshot(snapshot);

                int packageRetention = 1;
                if (HeapManager.ExistConfig(config.Id, path))
                {
                    Metadata last = HeapManager.GetLastBackup(path, config.Id);
                    packageRetention = last.RetentionStats[0];
                    packageRetention++;
                }

                backupPath = configPath + "/FB_" + DateTime.Now.ToString("d") + "_" + packageRetention;
                if (config.BackupType == _BackupType.IN)
                    backupPath = configPath + "/IN_" + DateTime.Now.ToString("d") + "_" + packageRetention;
                else if (config.BackupType == _BackupType.DI)
                    backupPath = configPath + "/DI_" + DateTime.Now.ToString("d") + "_" + packageRetention;

                this.SubCompleteMlist.Clear();

                sr.CreateDir(backupPath);
            }
            int[] retention = new int[2];
            {
                Metadata last = HeapManager.GetLastBackup(path, config.Id);
                if (last != null)
                    retention = last.RetentionStats;
            }
            Backup(sr, config, backupPath, retention, path);
        }
        private void Backup(Service service, Config config, string path, int[] lastRetention, string actualDestination)
        {
            Metadata meta;//vytvoření metadat
            if (lastRetention[1] == 0)
                meta = new Metadata(config.Id, config.Name, path, DateTime.Now, config.BackupType, new int[2] { 1, 1 });
            else if (config.Retention[1] == lastRetention[1])
                meta = new Metadata(config.Id, config.Name, path, DateTime.Now, config.BackupType, new int[2] { lastRetention[0] + 1, 1 });
            else
                meta = new Metadata(config.Id, config.Name, path, DateTime.Now, config.BackupType, new int[2] { lastRetention[0], lastRetention[1] + 1 });

            if (config.BackupType != _BackupType.FB)
            {//přidání cesty - vytváření složek pro konkrétní zálohy. FB nepotřebuje, jelikož jeho záloha je brána jako balíček záloh.

                path = path + "/" + DateTime.Now.ToString("d") + DateTime.Now.ToString("_H.mm.ss");
            }
            Console.WriteLine(path);


            Dictionary<string, Source> changesDictionary = new Dictionary<string, Source>();

            Snapshot lastSnapshot = Application.DataService.GetSnapshotByID(config.Id);


            foreach (var item in config.Sources)
            {
                //vytváří chngesList a sbírá medadata
                List<SnapshotItem> newSnapshotList = new List<SnapshotItem>();
                newSnapshotList = GetSnapshot(item);
                newSnapshotList = SnapshotItemFilter(newSnapshotList);
                ChangeReport report;
                try
                {
                    report = GetChanges(lastSnapshot.Sources[item].Items, newSnapshotList);
                }
                catch
                {
                    report = GetChanges(new List<SnapshotItem>(), newSnapshotList);
                }
                List<SnapshotItem> changesList = report.SnapshotItem;
                changesList = SnapshotItemFilter(changesList);
                meta.Items.AddRange(report.MetadataItem);

                if (!changesDictionary.ContainsKey(item))
                {
                    Source src = new Source() { Path = item, Items = changesList };
                    changesDictionary[item] = src;
                }

                //nutno k cestě přidat název zdoje//asi už ne

                string[] parts = item.Split("\\");
                string newPath = parts[parts.Length - 1];
                while (service.DirExists(newPath))
                    newPath += "_1";

                if (!service.DirExists(path))
                    service.CreateDir(path);
                if (config.BackupFormat == _BackupFormat.PT)
                    DoBackup(service, changesList, path + "/" + newPath);
                else
                {
                    if (Directory.Exists(@"..\..\..\temp"))
                        Directory.Delete(@"..\..\..\temp", true);

                    Service tempServ = new Service(new Destination(@"..\..\..\temp", DestType.DRIVE));
                    if (!tempServ.DirExists(tempServ._destPath))
                        tempServ.CreateDir(tempServ._destPath);

                    DoBackup(tempServ, changesList, tempServ._destPath + @"\backup");
                    ZipFile.CreateFromDirectory(@"..\..\..\temp\backup", @"..\..\..\temp\" + newPath + ".zip");
                    service.CopyFile(@"..\..\..\temp\" + newPath + ".zip", path + "/" + newPath + ".zip");
                    Directory.Delete(@"..\..\..\temp", true);

                }
            }


            MdataService.FtpWriteMetadata(service, path, meta);
            HeapManager.meta.Add(new HeapItem(config.Id, actualDestination, meta));

            if (SubCompleteMlist.Count == 0 && (config.BackupType == _BackupType.DI || config.BackupType == _BackupType.IN))
            {
                Snapshot snapshot = new Snapshot() { ConfigID = config.Id, Sources = changesDictionary };
                Application.DataService.WriteSnapshot(snapshot);
            }
            else if (config.BackupType == _BackupType.IN && SubCompleteMlist.Count > 0)
            {
                Dictionary<string, Source> lastChangesDictionary = lastSnapshot.Sources;
                foreach (KeyValuePair<string, Source> item in changesDictionary)
                {
                    for (int i = 0; i < item.Value.Items.Count; i++)
                    {
                        if (lastChangesDictionary[item.Key].Items.Contains(item.Value.Items[i]))
                        {
                            int index = lastChangesDictionary[item.Key].Items.IndexOf(item.Value.Items[i]);
                            lastChangesDictionary[item.Key].Items[index] = item.Value.Items[i];
                        }
                        else
                        {
                            lastChangesDictionary[item.Key].Items.Add(item.Value.Items[i]);
                        }
                    }

                    //lastChangesDictionary[item.Key].Items = item.Value.Items;
                }

                Snapshot snapshot = new Snapshot() { ConfigID = config.Id, Sources = lastChangesDictionary };
                Application.DataService.WriteSnapshot(snapshot);
            }
        }
        public void DoBackup(Service service, List<SnapshotItem> changesList, string path)
        {
            if (!service.DirExists(path))
                service.CreateDir(path);
            foreach (var item in changesList)
            {
                if (item.ItemType == _ItemType.FILE)
                {
                    if (!Directory.Exists(path + PathReturner(item.ItemPath, 1).Replace(item.Root, "")))
                        service.CreateDir(path + PathReturner(item.ItemPath, 1).Replace(item.Root, ""));
                    service.CopyFile(item.ItemPath, path + item.ItemPath.Replace(item.Root, ""));
                }
                else
                {
                    if (!service.DirExists(path + item.ItemPath.Replace(item.Root, "")))
                        service.CreateDir(path + item.ItemPath.Replace(item.Root, ""));
                }
            }
        }

        private void CreateDir(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            if (!info.Exists)
            {
                Directory.CreateDirectory(info.FullName);
            }
        }
        private void CopyDir(string source, string destination)
        {
            DirectoryInfo sourceDir = new DirectoryInfo(source);
            CreateDir(destination);
            foreach (var item in sourceDir.GetFiles())
            {
                item.CopyTo(destination + "\\" + item.Name);
            }
            foreach (var item in sourceDir.GetDirectories())
            {
                CopyDir(item.FullName, destination + "\\" + item.Name);
            }
        }
        private List<SnapshotItem> SnapshotItemFilter(List<SnapshotItem> items)
        {
            Dictionary<string, SnapshotItem> itemsDic = new Dictionary<string, SnapshotItem>();
            foreach (var item in items)
            {
                itemsDic.Add(item.ItemPath, item);
            }
            for (int i = 0; i < itemsDic.Count; i++)
            {
                var key = itemsDic.ElementAt(i);
                string[] parts = key.Value.ItemPath.Split('\\');
                string path = key.Value.ItemPath.Replace("\\" + parts[parts.Length - 1], "");
                if (itemsDic.ContainsKey(path))
                {
                    i--;
                    itemsDic.Remove(path);
                }
            }
            List<SnapshotItem> newItems = new List<SnapshotItem>();
            foreach (var item in itemsDic)
            {
                newItems.Add(item.Value);
            }
            return newItems;
        }

        public List<SnapshotItem> GetSnapshot(string path)
        {
            return GetSnapshotCycle(path, path);
        }
        private List<SnapshotItem> GetSnapshotCycle(string path, string root, List<SnapshotItem> dirEntry = null)
        {
            if (dirEntry == null)
            {
                dirEntry = new List<SnapshotItem>();
            }
            DirectoryInfo dir = new DirectoryInfo(path);
            DirectoryInfo[] subDirs = dir.GetDirectories();
            FileInfo[] files = dir.GetFiles();
            foreach (var item in subDirs)
            {
                dirEntry.Add(new SnapshotItem(item.FullName, _ItemType.FOLDER, Convert.ToString(File.GetLastWriteTime(item.FullName)), root));
                GetSnapshotCycle(item.FullName, root, dirEntry);
            }
            foreach (var item in files)
            {
                dirEntry.Add(new SnapshotItem(item.FullName, _ItemType.FILE, Convert.ToString(File.GetLastWriteTime(item.FullName)), root));
            }
            return dirEntry;
        }
        public ChangeReport GetChanges(List<SnapshotItem> source, List<SnapshotItem> snapshotToCompare)
        {
            Dictionary<string, SnapshotItem> sourceDic = new Dictionary<string, SnapshotItem>();
            Dictionary<string, SnapshotItem> compareDic = new Dictionary<string, SnapshotItem>();
            ChangeReport report = new ChangeReport();
            foreach (var item in source)
            {
                sourceDic.Add(item.ItemPath, item);
            }
            foreach (var item in snapshotToCompare)
            {
                compareDic.Add(item.ItemPath, item);
            }
            foreach (var item in source)
            {
                try
                {
                    SnapshotItem s = compareDic[item.ItemPath];
                    if (item.ItemType != _ItemType.FOLDER && item.Date != s.Date)
                    {
                        report.SnapshotItem.Add(item);
                        report.MetadataItem.Add(new MetadataItem(item.ItemPath, _itemChange.EDITED));
                    }

                    compareDic.Remove(item.ItemPath);
                }
                catch
                {
                    //zde by bylo možné odchytávat soubory, které byly odstraněny
                    report.MetadataItem.Add(new MetadataItem(item.ItemPath, _itemChange.REMOVED));
                }
            }
            foreach (var item in compareDic)
            {
                report.SnapshotItem.Add(item.Value);
                report.MetadataItem.Add(new MetadataItem(item.Value.ItemPath, _itemChange.ADDED));
            }
            return report;
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
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("backup!" + DateTime.Now);
            try
            {
                this.BackupSetup(TaskManager.TaskList);
                try
                {
                    await APIService.SendReport(new Report() { date = TaskManager.TaskList[0].Date, idConfig = TaskManager.TaskList[0].IdConfig, idClient = Application.IdOfThisClient, message = "OK", success = true, size = 0 });
                }
                catch
                {
                    ReportHolder.AddReport(new Report() { date = TaskManager.TaskList[0].Date, idConfig = TaskManager.TaskList[0].IdConfig, idClient = Application.IdOfThisClient, message = "OK", success = true, size = 0 });
                    Console.WriteLine("Send report fail");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Backup Error");
                Console.WriteLine(ex);
                try
                {
                    await APIService.SendReport(new Report() { date = TaskManager.TaskList[0].Date, idConfig = TaskManager.TaskList[0].IdConfig, idClient = Application.IdOfThisClient, message = "Backup Error: " + ex.Message, success = false, size = 0 });
                }
                catch
                {
                    ReportHolder.AddReport(new Report() { date = TaskManager.TaskList[0].Date, idConfig = TaskManager.TaskList[0].IdConfig, idClient = Application.IdOfThisClient, message = "Backup Error: " + ex.Message, success = false, size = 0 });
                    Console.WriteLine("Send report fail");
                }
            }

            TaskManager.TaskList.RemoveAt(0);
        }
    }
}