using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonOfPain
{
    public class Source
    {
        public string Path { get; set; }
        public List<SnapshotItem> Items { get; set; }
        public Source()
        {
            this.Items = new List<SnapshotItem>();
        }
    }
}
