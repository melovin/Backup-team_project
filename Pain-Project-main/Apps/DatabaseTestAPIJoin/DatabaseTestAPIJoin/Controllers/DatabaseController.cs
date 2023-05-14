using DatabaseTest.Controllers;
using DatabaseTestAPIJoin.Database.Models;
using DatabaseTestAPIJoin.ReturnModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseTestAPIJoin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatabaseController : ControllerBase
    {
        private MyContext myContext = new MyContext();

        [HttpPost("InsertUser")]
        public void InsertUser(CompleteUser userCurrent)
        {
            User us = new User() { name = userCurrent.name, surname = userCurrent.surname, createDate = "2020-01-01 12:12:12", Sources = new List<Source>() };
            foreach (string item in userCurrent.sources)
            {
                //myContext.tbSources.Add(new Source() { User = us, source = item });
                us.Sources.Add(new Source() { User = us, source = item });
            }

            myContext.tbUsers.Add(us);
            myContext.SaveChanges();
        }
        [HttpGet("GetUserById")]
        public CompleteUser GetUserById(int id)
        {
            User foundUser = myContext.tbUsers.Where(user => user.id == id).FirstOrDefault();
            CompleteUser completeUser = new CompleteUser()
            {
                id = foundUser.id,
                create_date = foundUser.createDate,
                name = foundUser.name,
                surname = foundUser.surname,
                sources = new List<string>()
            };

            var q = from c in myContext.tbSources
                    where c.idUser == id
                    select new {c.source};
            q.ToList();
            foreach (var item in q)
            {
                completeUser.sources.Add(item.ToString());
            }

            return completeUser;
        }
    }
}
