using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ConsoleApp6
{
    internal class Program
    {
        static HttpClient client = new HttpClient();

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri(@"https://localhost:44345/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                Client pc = new Client() { 
                    IpAddress = GetLocalIPAddress(),
                    MacAddress = GetMacAddress(),
                    Name = "PC"
                };

                var url = await CreateClientPCAsync(pc);
                Console.WriteLine($"Created at {url}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static async Task<Uri> CreateClientPCAsync(Client pc)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "Database/PostClient", pc);
            Console.WriteLine(response.ToString());
            Console.ReadKey(true);
            response.EnsureSuccessStatusCode();

            return response.Headers.Location;
        }
        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }



        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        public static string GetMacAddress()
        {
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    return nic.GetPhysicalAddress().ToString();
                }
            }
            throw new Exception("No mac address");
        }
    }
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }
        public bool Active { get; set; }
        public Client()
        {

        }
    }

}
