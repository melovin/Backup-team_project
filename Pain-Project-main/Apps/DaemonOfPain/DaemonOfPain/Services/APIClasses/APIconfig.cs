using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DaemonOfPain.Config;

namespace DaemonOfPain.Services.APIClasses
{
    public class APIconfig
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cron { get; set; }
        public string BackupType { get; set; }
        public string BackupFormat { get; set; }
        public int[] Retention { get; set; } = new int[2];
        public List<string> Sources { get; set; }
        public List<Destination> Destinations { get; set; }

        public Config ConvertToConfig()
        {
            _BackupType type;
            _BackupFormat format;
            if(this.BackupType == "FB") { type = _BackupType.FB; }
            else if (this.BackupType == "DI") { type = _BackupType.IN; }
            else { type = _BackupType.DI; }
            if (this.BackupFormat == "AR") { format = _BackupFormat.AR; }
            else { format = _BackupFormat.PT; }

            return new Config() { Id = this.Id, Name = this.Name, BackupFormat = format, BackupType = type, Cron = this.Cron, Destinations = this.Destinations, Sources = this.Sources, Retention = this.Retention };
        }
        public static List<Config> ConvertListToConfig(List<APIconfig> list)
        {
            List<Config> configList = new List<Config>();
            foreach (var item in list)
            {
                configList.Add(item.ConvertToConfig());
            }
            return configList;
        }
    }
}
