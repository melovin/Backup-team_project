using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonOfPain
{
    public enum _ItemType
    {
        FILE,
        FOLDER
    }
    public class SnapshotItem
    {
        public string ItemPath { get; set; }
        public _ItemType ItemType { get; set; }
        public string Date { get; set; }
        public string Root { get; set; }
        public SnapshotItem(string itemPath, _ItemType itemType, string date, string root)
        {
            Root = root;
            ItemPath = itemPath;
            ItemType = itemType;
            Date = date;
        }
    }
}
