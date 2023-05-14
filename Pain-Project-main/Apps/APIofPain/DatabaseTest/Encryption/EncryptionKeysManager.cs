using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DatabaseTest.Encryption
{
    public class EncryptionKeysManager : IJob
    {
        private static RSACryptoServiceProvider MyKey { get; set; }
        public Task Execute(IJobExecutionContext context)
        {
            NewKeys();
            return Task.CompletedTask;
        }
        public static void ReadMyKeyFromFile()
        {
            StreamReader sr = new StreamReader("");
            string data = sr.ReadToEnd();
            sr.Close();

            MyKey.FromXmlString(data);
        }
        public static void WriteMyKeyToFile()
        {
            StreamWriter sw = new StreamWriter("");
            sw.Write(MyKey.ToXmlString(true));
            sw.Close();
        }
        public static void NewKeys()
        {
            MyKey = new RSACryptoServiceProvider(1024);

        }

        public static string GetPublicKey()
        {
            return RsaProcessor.ExportPublicKey(MyKey);
        }
        public static string GetPrivateKey()
        {
            return RsaProcessor.ExportPrivateKey(MyKey);
        }
    }
}
