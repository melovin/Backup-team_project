using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonOfPain
{
    public enum _itemChange
    {
        ADDED,
        EDITED,
        REMOVED
    }
    public class MetadataItem
    {
        public string ItemPath { get; set; }
        public _itemChange ItemChange { get; set; }
        public string ItemSource { get; set; }
        public MetadataItem(string itemPath, _itemChange itemChange)
        {
            ItemPath = itemPath;
            ItemChange = itemChange;
        }
    }
}
