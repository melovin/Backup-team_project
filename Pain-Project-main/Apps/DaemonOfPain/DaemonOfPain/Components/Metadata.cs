using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonOfPain
{
    public class Metadata
    {
        public int IdConfig { get; set; }
        public string ConfigName { get; set; }
        public string BackupPath { get; set; }
        public DateTime CreateDate { get; set; }
        public _BackupType BackupType { get; set; }
        public int[] RetentionStats { get; set; }
        public List<MetadataItem> Items { get; set; }
        public Metadata(int idConfig, string configName, string backupPath, DateTime createDate, _BackupType backupType, int[] retentionStats )
        {
            RetentionStats = retentionStats;
            IdConfig = idConfig;
            ConfigName = configName;
            BackupPath = backupPath;
            CreateDate = createDate;
            BackupType = backupType;
            Items = new List<MetadataItem>();
        }

    }
}
