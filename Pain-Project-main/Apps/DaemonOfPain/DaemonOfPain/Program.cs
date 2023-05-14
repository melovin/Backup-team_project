using DaemonOfPain.Encryption;
using DaemonOfPain.Services;
using NCrontab;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace DaemonOfPain
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Application app = new Application();
            await app.StartApplication();

            //EncryptionKeysManager.NewKeys();
            //string s = EncryptionKeysManager.GetPrivateKey();
            //EncryptionKeysManager.NewKeys();
            //string y = EncryptionKeysManager.GetPrivateKey();
            //EncryptionKeysManager.NewKeys();
            //Thread.Sleep(3000);
            //string z = EncryptionKeysManager.GetPrivateKey();
            //Console.ReadKey();

        }

    }
}
