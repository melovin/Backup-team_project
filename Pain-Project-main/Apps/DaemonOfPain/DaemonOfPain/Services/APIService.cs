using DaemonOfPain;
using DaemonOfPain.Controller.ClassesToSend;
using DaemonOfPain.Services;
using DaemonOfPain.Services.APIClasses;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using DaemonOfPain.Encryption;
using DatabaseTest.DataClasses;

namespace DaemonOfPain
{
    public class APIService : IJob
    {
        static HttpClient client = new HttpClient();

        private static void Setup()
        {
            if (client.BaseAddress == null)
            {
                client.BaseAddress = new Uri(@"https://localhost:5001/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }
        public static async Task<string> LoginToServer()
        {
            Setup();
            try
            {
                string id = await LoginToServer(new Computer());
                Console.WriteLine("API1 - LoginToServer");
                return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        static async Task<string> LoginToServer(Computer pc)
        {
            APIRequest request = new APIRequest() { Data = JsonConvert.SerializeObject(pc), PublicKey = EncryptionKeysManager.GetPublicKey() };
            EncryptedAPIRequest enRequest = RsaProcessor.CombinedEncryptRequest(AesProcessor.GenerateKey(), EncryptionKeysManager.ServerKey, request);

            HttpResponseMessage response = await client.PostAsJsonAsync("/Daemon/AddDaemon", enRequest);
            response.EnsureSuccessStatusCode();
            EncryptedAPIResponse data = JsonConvert.DeserializeObject<EncryptedAPIResponse>(response.Content.ReadAsStringAsync().Result);
            return RsaProcessor.CombinedDecryptResponse(EncryptionKeysManager.GetPrivateKey(), data);
        }
        //****************************************************************************************************************


        public static async Task<string> GetServerPublicKey()
        {
            Setup();
            string response = await client.GetStringAsync("/Daemon/GetPublicKey");
            response = response.Substring(1, response.Length - 2);
            return response;
        }


        //****************************************************************************************************************

        public static async Task GetConfigs()
        {
            Setup();
            try
            {
                List<APIconfig> respose = await GetConfigs(Application.IdOfThisClient);
                Application.DataService.WriteAllConfigs(APIconfig.ConvertListToConfig(respose));
                Console.WriteLine("API2 - GetConfigs");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static async Task<List<APIconfig>> GetConfigs(string id)
        {
            APIRequest request = new APIRequest() { Id = id, PublicKey = EncryptionKeysManager.GetPublicKey() };
            EncryptedAPIRequest enRequest = RsaProcessor.CombinedEncryptRequest(AesProcessor.GenerateKey(), EncryptionKeysManager.ServerKey, request);
            string enRequestString = JsonConvert.SerializeObject(enRequest);
            string enResponse = await client.GetStringAsync($"/Daemon/GetConfigs" + "?enRequestString=" + enRequestString);
            string response = RsaProcessor.CombinedDecryptResponse(EncryptionKeysManager.GetPrivateKey(), JsonConvert.DeserializeObject<EncryptedAPIResponse>(enResponse));
            IEnumerable<APIconfig> config = null;
            config = JsonConvert.DeserializeObject<List<APIconfig>>(response);
            return (List<APIconfig>)config;
        }
        //****************************************************************************************************************
        public static async Task SendReport(Report report)
        {
            Setup();
            var url = await SendReport2(report);
        }
        static async Task<Uri> SendReport2(Report report)
        {
            EncryptedAPIRequest enRequest = RsaProcessor.CombinedEncryptRequest(AesProcessor.GenerateKey(), EncryptionKeysManager.ServerKey, new APIRequest() { Data = JsonConvert.SerializeObject(report), Id = Application.IdOfThisClient, PublicKey = EncryptionKeysManager.GetPublicKey() });
            HttpResponseMessage response = await client.PostAsJsonAsync("/Daemon/sendReport", enRequest);
            response.EnsureSuccessStatusCode();
            Console.WriteLine("API3 - SendReport");
            return response.Headers.Location;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await GetConfigs();
        }
    }
}

