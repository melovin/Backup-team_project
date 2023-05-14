using Quartz;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DaemonOfPain.Encryption
{
    public class EncryptionKeysManager : IJob
    {
        public static string ServerKey { get; set; }
        public static DateTime ServerKeyExpiration { get; set; } = DateTime.Now.AddDays(-1);
        private static RSACryptoServiceProvider MyKey { get; set; }
        public Task Execute(IJobExecutionContext context)
        {
            NewKeys();
            return Task.CompletedTask;
        }
        public static void NewKeys()
        {
            MyKey = new RSACryptoServiceProvider(1024);
        }
        public static void SetNewServerKey(string key)
        {
            ServerKey = key;
            ServerKeyExpiration = DateTime.Now.AddHours(1).AddMinutes(-DateTime.Now.Minute).AddSeconds(-DateTime.Now.Second).AddMilliseconds(-DateTime.Now.Millisecond);
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
