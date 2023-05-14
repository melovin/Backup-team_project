using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonOfPain
{
    public class ChangeReport
    {
        public List<SnapshotItem> SnapshotItem { get; set; }
        public List<MetadataItem> MetadataItem { get; set; }
        public ChangeReport()
        {
            SnapshotItem = new List<SnapshotItem>();
            MetadataItem = new List<MetadataItem>();
        }
    }
}
