using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonOfPain
{
    public enum _BackupType
    {
        FB,
        DI,
        IN
    }
    public enum _BackupFormat
    {
        PT,
        AR
    }
    public class Config
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cron { get; set; }
        public _BackupType BackupType { get; set; }
        public _BackupFormat BackupFormat { get; set; }
        public int[] Retention { get; set; } = new int[2];
        public List<string> Sources { get; set; }
        public List<Destination> Destinations { get; set; }


        public Config()
        {
            Retention = new int[2] { 0, 1};
            Sources = new List<string>();
            Destinations = new List<Destination>();
        }
    }
}
