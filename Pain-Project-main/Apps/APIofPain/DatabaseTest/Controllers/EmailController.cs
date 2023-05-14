using DatabaseTest.DatabaseTables;
using DatabaseTest.DataClasses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace DatabaseTest.Controllers
{
    [ApiController]
    [Route("Email")]
    public class EmailController : ControllerBase
    {
        private MyContext context = new MyContext();
        private string dataPath = @"Data\emailSettings.json";


        [HttpGet("getEmails")]
        public JsonResult GetEmails()
        {
            try
            {
                List<string> emails = this.context.Administrators.Select(x => x.Email).ToList();

                if (emails == null)
                    return new JsonResult("Emails not found!") { StatusCode = (int)HttpStatusCode.NotFound };
                return new JsonResult(emails) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception)
            {
                return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.BadRequest };

            }
        }

        [HttpGet("getSettings")]
        public JsonResult GetSettings()
        {
            try
            {
                StreamReader sr = new StreamReader(dataPath);
                EmailSettings em = JsonConvert.DeserializeObject<EmailSettings>(sr.ReadToEnd());
                if (em == null)
                    return new JsonResult("Settings not found!") { StatusCode = (int)HttpStatusCode.NotFound };
                return new JsonResult(em) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception)
            {
                return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.BadRequest };

            }
        }
        [HttpGet("getReports")]
        public JsonResult GetReports()
        {

            try
            {
                //List<Task> tasks = new List<Task>();
                StreamReader sr = new StreamReader(dataPath);
                EmailSettings em = JsonConvert.DeserializeObject<EmailSettings>(sr.ReadToEnd());

                if (em.Freq == Freq.WEEKLY)
                {
                    DateTime today = DateTime.Today;
                    var tasks = from t in context.Tasks
                                join a in context.Assignments on t.IdAssignment equals a.Id
                                join cl in context.Clients on a.IdClient equals cl.Id
                                join co in context.Configs on a.IdConfig equals co.Id
                                where t.Date >= today.AddDays(-7) && t.Date < today.AddDays(1)
                                select new
                                {
                                    TaskId = t.Id,
                                    ConfigName = co.Name,
                                    ClientName = cl.Name,
                                    State = t.State,
                                    Message = t.Message,
                                    Date = t.Date,
                                };
                    sr.Close();
                    return new JsonResult(tasks) { StatusCode = (int)HttpStatusCode.OK };
                }

                else if (em.Freq == Freq.DAILY)
                {
                    DateTime today = DateTime.Today;
                    var tasks = from t in context.Tasks
                                join a in context.Assignments on t.IdAssignment equals a.Id
                                join cl in context.Clients on a.IdClient equals cl.Id
                                join co in context.Configs on a.IdConfig equals co.Id
                                where t.Date >= today.AddDays(-1) && t.Date < today.AddDays(1)
                                select new
                                {
                                    TaskId = t.Id,
                                    ConfigName = co.Name,
                                    ClientName = cl.Name,
                                    State = t.State,
                                    Message = t.Message,
                                    Date = t.Date,
                                };
                    sr.Close();
                    return new JsonResult(tasks) { StatusCode = (int)HttpStatusCode.OK };
                }
                else
                {
                    DateTime today = DateTime.Today;
                    var tasks = from t in context.Tasks
                                join a in context.Assignments on t.IdAssignment equals a.Id
                                join cl in context.Clients on a.IdClient equals cl.Id
                                join co in context.Configs on a.IdConfig equals co.Id
                                where t.Date >= today.AddMonths(-1) && t.Date < today.AddDays(1)
                                select new
                                {
                                    TaskId = t.Id,
                                    ConfigName = co.Name,
                                    ClientName = cl.Name,
                                    State = t.State,
                                    Message = t.Message,
                                    Date = t.Date
                                };
                    sr.Close();
                    return new JsonResult(tasks) { StatusCode = (int)HttpStatusCode.OK };
                }
            }
            catch (Exception ex)
            {
                return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }
    }
}
