using DaemonOfPain.Components;
using FluentFTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonOfPain.Services
{
    public class FtpService : IService
    {
        FtpClient client;
        public FtpService(FtpConfig config)
        {
            try
            {
                this.client = new FtpClient(config.Host, config.Username, config.Password);
                client.Connect();
            }
            catch (Exception)
            {
                throw new Exception("Couldn't connect to FTP server!");
            }

        }
        public void CopyFile(string source, string destination)
        {
            client.UploadFile(source, destination);
        }
        public void CreateDir(string path)
        {
            client.CreateDirectory(path, true);
        }
        public void DeleteDir(string path)
        {
            client.DeleteDirectory(path);
        }
        public bool DirExists(string path)
        {
            return client.DirectoryExists(path);
        }
    }
}
