using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DaemonOfPain.Controller.ClassesToSend
{
    public class Computer
    {
        public string MACaddress { get; set; }
        public string IPaddress { get; set; }
        public string Name { get; set; }
        public Computer()
        {
            Name = Environment.MachineName;
            IPaddress = GetLocalIPAddress();
            MACaddress = GetMacAddress();
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
            string mac = "";
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    mac = nic.GetPhysicalAddress().ToString() == "" ? mac : nic.GetPhysicalAddress().ToString();
                    if (nic.Name == "Ethernet")
                        break;
                }
            }
            if (mac != "")
                return mac;
            throw new Exception("No mac address");
        }
    }
}
