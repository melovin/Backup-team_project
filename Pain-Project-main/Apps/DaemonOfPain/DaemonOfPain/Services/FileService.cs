using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonOfPain.Services
{
    public class FileService : IService
    {
        public void CopyFile(string source, string destination)
        {
            File.Copy(source, destination, true);
        }

        public void CreateDir(string path)
        {
            Directory.CreateDirectory(path);
        }

        public void DeleteDir(string path)
        {
            Directory.Delete(path, true);
        }

        public bool DirExists(string path)
        {
            return Directory.Exists(path);
        }
    }
}
