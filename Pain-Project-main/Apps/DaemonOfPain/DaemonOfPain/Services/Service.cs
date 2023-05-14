using DaemonOfPain.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonOfPain.Services
{
    public class Service
    {
        private IService _service;
        public string _destPath = "";

        public Service(Destination dest) 
        {

            if (dest.DestinationType == DestType.FTP)
            {
                string url = dest.DestinationPath.Replace("ftp://", "");
                string[] parts = url.Split('@');
                string[] pathParts = parts[1].Split('/');

                pathParts[0] = "";
                foreach (var item in pathParts)
                {
                    this._destPath += item + "/";
                }

                _service = new FtpService(new FtpConfig(dest.DestinationPath));
            }
            else
            {
                _service = new FileService();
                _destPath = dest.DestinationPath;
            }
        }

        public void CopyFile(string source, string destination)
        {
            _service.CopyFile(source, destination);
        }

        public void CreateDir(string path)
        {
            _service.CreateDir(path);
        }

        public void DeleteDir(string path)
        {
            _service.DeleteDir(path);
        }

        public bool DirExists(string path)
        {
            return _service.DirExists(path);
        }
    }
    public interface IService
    {
        public void CopyFile(string source, string destination);
        //public void DeleteFile(string source);
        public void CreateDir(string path);
        public void DeleteDir(string path);
        public bool DirExists(string path);

    }
}
