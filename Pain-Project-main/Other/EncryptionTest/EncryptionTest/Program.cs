using ExpressEncription;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace EncryptionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Generování klíče

            Console.WriteLine(AesOperation.GenerateKey(5));


            //AES

            var key = "ORSex6FYVwEGwTiOTaMc2EZot04fY1h3";

            var str = "{\"Data\":\"{\\\"MACaddress\\\":\\\"00090FAA0001\\\",\\\"IPaddress\\\":\\\"192.168.91.13\\\",\\\"Name\\\":\\\"LAPTOP-6ORTTLOT\\\"}\",\"PublicKey\":\"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDY39TnNORajyFLG9mNWU6ujMqEk3R7obmdez6lvB4YNUicPaHeCBBY1NXMQftMb4T6zBcwIDhxQqkON3J02G9/ODB2eZCQ1I46ct+wlEHoPbLtZA0oyjBVOXJZXxLz34e7ptyPgdZFq2egyfnEWaOtcmKOMgyfuuBvcIQIQYDAdQIDAQAB\",\"Id\":null}";
            var encryptedString = AesOperation.EncryptString(key, str);
            Console.WriteLine($"encrypted string = {encryptedString}");

            var decryptedString = AesOperation.DecryptString(key, encryptedString);
            Console.WriteLine($"decrypted string = {decryptedString}");

            Console.ReadKey();




            //RSA

            //RSACryptoServiceProvider provider = new RSACryptoServiceProvider(1024);

            //string publicKey = Rsa.ExportPublicKey(provider);
            //string privateKey = Rsa.ExportPrivateKey(provider);

            //Console.WriteLine(publicKey);
            //Console.WriteLine(privateKey);

            //RSACryptoServiceProvider publicProvider = Rsa.ImportPublicKey(publicKey);
            //RSACryptoServiceProvider privateProvider = Rsa.ImportPrivateKey(privateKey);

            //var cipherText = Rsa.Encrypt("hello world", publicKey);
            //var plainText = Rsa.Decrypt(cipherText, privateKey);
            //Console.WriteLine(plainText);
        }
    }
}
