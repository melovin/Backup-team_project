using EmailSender.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailSender
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Application app = new Application();
            await app.Start();
        }
    }
}
