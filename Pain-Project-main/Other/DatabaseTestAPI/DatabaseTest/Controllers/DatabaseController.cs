using DatabaseTest.DatabaseTables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatabaseController : ControllerBase
    {
        MyContext myContext = new MyContext();

        [HttpGet("GetClients")]
        public IEnumerable<Client> GetClients()
        {
            return myContext.Clients;
        }
        [HttpGet("GetAdmins")]
        public IEnumerable<Administrator> GetAdmins()
        {
            return myContext.Administrators;
        }

        [HttpPost("PostClient")]
        public void PostClient(Client clientGet)
        {
            myContext.Clients.Add(clientGet);

            myContext.SaveChanges();
        }

        [HttpPost("PostAdmin")]
        public void PostAdmin(string loginName, string password, bool isAdmin, string cronEmail, string name, string Surname, bool darkMode, string email)
        {
            Administrator admin = new Administrator()
            {
                Login = loginName,
                Password = password,
                Admin = isAdmin,
                CronEmail = cronEmail,
                Name = name,
                Surname = Surname,
                DarkMode = darkMode,
                Email = email
            };
            myContext.Administrators.Add(admin);

            myContext.SaveChanges();
        }

        [HttpDelete("DeleteClient")]
        public void DeleteClient(int id)
        {
            Client client = myContext.Clients.Find(id);
            myContext.Clients.Remove(client);

            myContext.SaveChanges();
        }
        [HttpDelete("DeleteAdmin")]
        public void DeleteAdmin(int id)
        {
            Administrator admin = myContext.Administrators.Find(id);
            myContext.Administrators.Remove(admin);

            myContext.SaveChanges();
        }
    }
}
