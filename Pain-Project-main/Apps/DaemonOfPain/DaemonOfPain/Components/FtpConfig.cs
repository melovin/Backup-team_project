using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonOfPain.Components
{
    public class FtpConfig
    {
        public string Host { get; set; }
        public string BackupFolder { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public FtpConfig(string url)
        {
            url = url.Replace("ftp://", "");

            string[] parts = url.Split('@');
            string[] pathParts = parts[1].Split('/');

            this.Host = "ftp://" + pathParts[0];
            pathParts[0] = "";
            this.BackupFolder = "";
            foreach (var item in pathParts)
            {
                this.BackupFolder += item + "/";
            }

            this.Username = parts[0].Split(':')[0];
            this.Password = parts[0].Split(':')[1];
        }
    }
}
