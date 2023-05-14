using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonOfPain
{
    public enum DestType
    {
        DRIVE,
        FTP
    }
    public class Destination
    {
        public DestType DestinationType{ get; set; }
        public string DestinationPath{ get; set; }
        public Destination(string path, DestType destType)
        {
            DestinationPath = path;
            DestinationType = destType;
        }
    }
}
