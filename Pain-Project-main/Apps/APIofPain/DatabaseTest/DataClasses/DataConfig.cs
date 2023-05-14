using DatabaseTest.DatabaseTables;
using System.Collections.Generic;

namespace DatabaseTest.DataClasses
{
    public class DataConfig
    {
        public int Id { get; set; }
        public int ?IdAdministrator { get; set; }
        public string ?AdminName { get; set; }
        public string Name { get; set; }
        public string CreateDate { get; set; }
        public string Cron { get; set; }
        public string BackUpFormat { get; set; }
        public string BackUpType { get; set; }
        public int RetentionPackages { get; set; }
        public int RetentionPackageSize { get; set; }
        public List<string> Sources { get; set; }
        public List<DataDestination> Destinations { get; set; }
        public Dictionary<int, string>? ClientNames { get; set; }
    }
    public class DataDestination
    {
        public string DestType { get; set; }
        public string Path { get; set; }
    }
}
