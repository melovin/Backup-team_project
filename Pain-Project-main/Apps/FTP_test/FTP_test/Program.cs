using System;
using System.IO;
using System.Net;

namespace FTP_test
{
    internal class Program
    {
        #region
        const string tajneJmeno = "karton-namiru.cz";
        const string tajneHeslo = "IntuityADMIN21";
        #endregion


        static void Main(string[] args)
        {
            //CreateFolder("testing");
            //UploadFile(@"C:\Users\troli\Desktop\Stranky\Orka\Obsah.txt", @"ftp://ftp.orka-cb.cz/orka-cb_cz/testing/Obsah.txt");
            RemoveFileFolder("ftp://ftp.orka-cb.cz/orka-cb_cz/testing");
            //DownloadFile("ftp://ftp.orka-cb.cz/orka-cb_cz/testing/Obsah.txt");
            //Console.WriteLine(DirExists("ftp://ftp.orka-cb.cz/orka-cb_cz/testing"));
        }
        public static void CreateFolder(string cesta)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://ftp.orka-cb.cz/orka-cb_cz/" + cesta);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            request.Credentials = new NetworkCredential(tajneJmeno, tajneHeslo);

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                Console.WriteLine(reader.ReadToEnd());
            }
        }
        public static void UploadFile(string from, string to)
        {
            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential(tajneJmeno, tajneHeslo);
                client.UploadFile(to, WebRequestMethods.Ftp.UploadFile, from);
            }
        }
        public static void RemoveFileFolder(string to)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(to);
            request.Method = WebRequestMethods.Ftp.RemoveDirectory; //Funguje jen když je složka prázdná, musí se whilem projek a možná rekurzí idk aby to šlapalo
            //request.Method = WebRequestMethods.Ftp.DeleteFile; //Pouze na soubory
            request.Credentials = new NetworkCredential(tajneJmeno, tajneHeslo); ;

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                Console.WriteLine(reader.ReadToEnd());
            }
        }
        public static void DownloadFile(string path)
        {
            WebClient request = new WebClient();
            request.Credentials = new NetworkCredential(tajneJmeno, tajneHeslo);
            try
            {
                byte[] newFileData = request.DownloadData(path);
                string fileString = System.Text.Encoding.UTF8.GetString(newFileData);
                Console.WriteLine(fileString);
            }
            catch (WebException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public static bool DirExists(string dirPath)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(dirPath);
                request.Credentials = new NetworkCredential(tajneJmeno, tajneHeslo);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
