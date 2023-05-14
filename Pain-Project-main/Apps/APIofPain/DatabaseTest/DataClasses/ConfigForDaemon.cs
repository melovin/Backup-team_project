using DatabaseTest.DatabaseTables;
using System.Collections.Generic;

namespace DatabaseTest.DataClasses
{
    public class ConfigForDaemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cron { get; set; }
        public string BackUpFormat { get; set; }
        public string BackUpType { get; set; }
        public int[] Retention { get; set; } = new int[2];
        public List<string> Sources { get; set; }
        public List<DataDestination> Destinations { get; set; }
    }
}
