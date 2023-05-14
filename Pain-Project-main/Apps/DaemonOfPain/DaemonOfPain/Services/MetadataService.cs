using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonOfPain.Services
{
    public class MetadataService
    {
        public void WriteMetadata(string path, Metadata meta)
        {
            StreamWriter sw = new StreamWriter(path + @"\Metadata.json");

            sw.Write(JsonConvert.SerializeObject(meta));
            sw.Close();
        }

        public void FtpWriteMetadata(Service service, string path, Metadata meta)
        {
            string temp = @"..\..\..\temp";
            if (!Directory.Exists(temp))
                Directory.CreateDirectory(temp);

            WriteMetadata(temp, meta);

            service.CopyFile(temp + @"\Metadata.json", path + @"/Metadata.json");
            File.Delete(temp + @"\Metadata.json"); 

        }
        public Metadata GetMetadata(string path)
        {
            StreamReader sr = new StreamReader(path + @"\Metadata.json");

            string data = sr.ReadToEnd();
            sr.Close();

            return JsonConvert.DeserializeObject<Metadata>(data);
        }


        //již otestováno - funkční
        public List<Metadata> MetaSearcher(string path, bool returnFirstRecord = false)//prohledá všechny podsložky a v listu vrátí všechny metadata, na která narazí
        {                                               //returnFirstRecord znamená, že jakmile najde jeden jediný záznam, už nehledá dál

            return MetaSearcherCycle(path, new List<Metadata>(), returnFirstRecord, 5);//5 znamená, že ve stromové struktuře půjde mažimálně 5 podložek hluboko(aby neprohléhával zálohovaná data - ztráta času)

        }
        private List<Metadata> MetaSearcherCycle(string path, List<Metadata> Mlist, bool returnFirstRecord, int counter)
        {
            if(counter > 0)
            {
                DirectoryInfo directories = new DirectoryInfo(path);
                if (File.Exists(directories.FullName + "\\Metadata.json"))
                {
                    Mlist.Add(GetMetadata(directories.FullName));
                    if (returnFirstRecord) { return Mlist; }
                }
                foreach (var item in directories.GetDirectories())
                {
                    if(!(returnFirstRecord && Mlist.Count>0))
                    {
                        Mlist = MetaSearcherCycle(item.FullName, Mlist, returnFirstRecord, counter - 1);
                    }
                }
            }
            return Mlist;
        }
    }
}
