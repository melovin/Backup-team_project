using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonOfPain
{
    public class Snapshot
    {
        public int ConfigID { get; set; }
        public Dictionary<string, Source> Sources { get; set; }

        public Snapshot()
        {
            Sources = new Dictionary<string, Source>();
        }
    }
}
