using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonOfPain.Components
{
    public class HeapItem
    {
        public int ConfigID { get; set; }
        public string Destination { get; set; }
        public int PackageRetentionNumber { get; set; }
        public int BackupRetentionNumber { get; set; }
        public Metadata Metadata { get; set; }

        public HeapItem(int configID, string destination, Metadata metadata)
        {
            ConfigID = configID;
            Destination = destination;
            Metadata = metadata;
            PackageRetentionNumber = metadata.RetentionStats[0];
            BackupRetentionNumber = metadata.RetentionStats[1];
        }
    }
}
