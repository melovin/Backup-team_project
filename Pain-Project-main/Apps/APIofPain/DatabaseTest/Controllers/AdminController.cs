using DatabaseTest.DatabaseTables;
using DatabaseTest.DataClasses;
using DatabaseTest.Logins;
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
    [Route("AdminPage")]
    //[Auth]
    public class AdminController : ControllerBase
    {
        private MyContext context = new MyContext();
        private string dataPath = @"Data\emailSettings.json";


        //Dashboard
        [HttpGet("todayTasks")]
        public JsonResult GetTodayTasks()
        {
            try
            {
                DateTime today = DateTime.Today;
                var tasks = from t in context.Tasks
                            join a in context.Assignments on t.IdAssignment equals a.Id
                            join cl in context.Clients on a.IdClient equals cl.Id
                            join co in context.Configs on a.IdConfig equals co.Id
                            where t.Date >= today && t.Date < today.AddDays(1)
                            select new
                            {
                                TaskId = t.Id,
                                ConfigName = co.Name,
                                ClientName = cl.Name,
                                State = t.State,
                                Date = t.Date
                            };
                return new JsonResult(tasks) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception)
            {
                return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }
        [HttpGet("getPercent")]
        public JsonResult GetPercent()
        {
            try
            {
                DateTime today = DateTime.Today;
                var tasks = from t in context.Tasks
                            join a in context.Assignments on t.IdAssignment equals a.Id
                            join cl in context.Clients on a.IdClient equals cl.Id
                            join co in context.Configs on a.IdConfig equals co.Id
                            where t.Date >= today && t.Date < today.AddDays(1)
                            select new
                            {
                                TaskId = t.Id,
                                ConfigName = co.Name,
                                ClientName = cl.Name,
                                State = t.State,
                                Date = t.Date
                            };
                double result = 100.0 * tasks.Where(x => x.Date <= DateTime.Now).Count() / tasks.Count();
                if (double.IsNaN(result))
                    result = 100;
                return new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch
            {
                return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.BadRequest };
            }

        }
        [HttpGet("getSize")]
        public JsonResult GetSize()
        {
            try
            {
                DateTime now = DateTime.Now;
                var size = from t in context.Tasks
                           where t.Date <= now && t.Date > now.AddDays(-7)
                           select t.Size;
                return new JsonResult(size.Sum()) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception)
            {
                return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }
        [HttpGet("sevenDays")]
        public JsonResult GetSevenDays()
        {
            try
            {
                DateTime now = DateTime.Now;
                var tasks = from t in context.Tasks
                            join a in context.Assignments on t.IdAssignment equals a.Id
                            join cl in context.Clients on a.IdClient equals cl.Id
                            join co in context.Configs on a.IdConfig equals co.Id
                            where t.Date <= now && t.Date > now.AddDays(-7)
                            select new
                            {
                                TaskId = t.Id,
                                ConfigName = co.Name,
                                ClientName = cl.Name,
                                State = t.State,
                                Date = t.Date
                            };
                return new JsonResult(tasks) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception)
            {
                return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }

        //AddConfig
        [HttpPost("addConfig")]
        public JsonResult AddConfig(DataConfig config)
        {
            try
            {
                if (context.Configs.Where(x => x.Name == config.Name).ToList().Count != 0)
                    throw new Exception();
                Administrator creator = context.Administrators.Where(x => x.Id == config.IdAdministrator).FirstOrDefault();
                Config newConfig = new Config()
                {
                    Name = config.Name,
                    CreateDate = DateTime.Now,
                    Cron = config.Cron,
                    BackUpFormat = config.BackUpFormat,
                    BackUpType = config.BackUpType,
                    RetentionPackages = config.RetentionPackages,
                    RetentionPackageSize = config.RetentionPackageSize,
                    Sources = new List<Source>(),
                    Destinations = new List<Destination>(),

                    Administrator = creator
                };

                foreach (var source in config.Sources)
                {
                    newConfig.Sources.Add(new Source() { Config = newConfig, Path = source });
                }

                foreach (var dest in config.Destinations)
                {
                    newConfig.Destinations.Add(new Destination() { Config = newConfig, Path = dest.Path, DestType = dest.DestType });
                }

                foreach (var client in config.ClientNames)
                {
                    Assignment assignment = new Assignment() { Config = newConfig, IdClient = client.Key };
                    context.Assignments.Add(assignment);
                }
                context.Configs.Add(newConfig);

                context.SaveChanges();
                new Tasker().RegenerateTaskDatabaseTable();
                return new JsonResult("Success") { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception)
            {
                return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }

        //Configs
        [HttpGet("allConfigs")]
        public JsonResult GetAllConfigs()
        {
            try
            {
                List<DataConfig> configs = new List<DataConfig>();
                var q =
                    from pathsArray in (
                        from srcArray in (
                            from innerConfigs in context.Configs.ToList()
                            from innerSources in context.Sources.ToList().Where(x => x.IdConfig == innerConfigs.Id).DefaultIfEmpty()
                            group innerSources == null ? null : innerSources.Path by innerConfigs.Id into grp
                            select new { id = grp.Key, sources = grp }
                        )
                        from destArray in (
                            from innerConfigs in context.Configs.ToList()
                            from innerDestinations in context.Destinations.ToList().Where(x => x.IdConfig == innerConfigs.Id).DefaultIfEmpty()
                            group innerDestinations by innerConfigs.Id into grp
                            select new { id = grp.Key, destinations = grp }
                        ).Where(x => x.id == srcArray.id).DefaultIfEmpty()
                        select new { id = srcArray.id, srcArray, destArray = destArray.destinations }
                    )
                    from outerConfigs in context.Configs.ToList().Where(x => x.Id == pathsArray.srcArray.id).DefaultIfEmpty()
                    from admins in context.Administrators.ToList().Where(x => x.Id == outerConfigs.IdAdministrator).DefaultIfEmpty()

                    from assignments in (
                        from a in context.Assignments.ToList().DefaultIfEmpty()
                        from innerClients in context.Clients.ToList().Where(x => x.Id == a.IdClient).DefaultIfEmpty()
                        group (a?.Client) by a.IdConfig into grp
                        select new { id = grp.Key, ids = grp == null ? null : grp }
                    ).Where(x => x.id == outerConfigs.Id).DefaultIfEmpty()
                    select new
                    {
                        ID = outerConfigs.Id,
                        Name = outerConfigs.Name,
                        CreateDate = outerConfigs.CreateDate,
                        Cron = outerConfigs.Cron,
                        BackupFormat = outerConfigs.BackUpFormat,
                        BackupType = outerConfigs.BackUpType,
                        RetentionPackages = outerConfigs.RetentionPackages,
                        RetentionPackageSize = outerConfigs.RetentionPackageSize,
                        //AdminName = admins != null ? admins.Name : null,
                        AdminName = admins?.Name,
                        Sources = pathsArray.srcArray.sources,
                        Destinations = pathsArray.destArray,
                        ////Clients = assignments != null ? assignments.ids.Select(x => new { x.Id, x.Name }) : null
                        Clients = assignments?.ids.Select(x => new { x.Id, x.Name })
                    };
                foreach (var item in q)
                {
                    List<DataDestination> dests = new List<DataDestination>();
                    foreach (var dest in item.Destinations)
                    {
                        if (dest != null)
                            dests.Add(new DataDestination() { DestType = dest.DestType, Path = dest.Path });
                    }
                    DataConfig data = new DataConfig()
                    {
                        Id = item.ID,
                        Sources = item.Sources.ToList()[0] == null ? new List<string>() : item.Sources.ToList(),
                        Destinations = dests,
                        CreateDate = item.CreateDate.ToString("s"),
                        BackUpFormat = item.BackupFormat,
                        BackUpType = item.BackupType,
                        Cron = item.Cron,
                        Name = item.Name,
                        RetentionPackages = item.RetentionPackages,
                        RetentionPackageSize = item.RetentionPackageSize,
                        AdminName = item.AdminName != null ? item.AdminName : "Deleted User",
                        ClientNames = item.Clients?.ToDictionary(x => x.Id, x => x.Name)
                    };
                    configs.Add(data);
                }
                return new JsonResult(configs) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception)
            {
                return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.NotFound };
            }
        }
        [HttpGet("getConfigByID")]
        public JsonResult GetConfigByID(int id)
        {
            try
            {
                var q =
                    from pathsArray in (
                        from srcArray in (
                            from innerConfigs in context.Configs.ToList()
                            from innerSources in context.Sources.ToList().Where(x => x.IdConfig == innerConfigs.Id).DefaultIfEmpty()
                            group innerSources == null ? null : innerSources.Path by innerConfigs.Id into grp
                            select new { id = grp.Key, sources = grp }
                        )
                        from destArray in (
                            from innerConfigs in context.Configs.ToList()
                            from innerDestinations in context.Destinations.ToList().Where(x => x.IdConfig == innerConfigs.Id).DefaultIfEmpty()
                            group innerDestinations by innerConfigs.Id into grp
                            select new { id = grp.Key, destinations = grp }
                        ).Where(x => x.id == srcArray.id).DefaultIfEmpty()
                        select new { id = srcArray.id, srcArray, destArray = destArray.destinations }
                    )
                    from outerConfigs in context.Configs.ToList().Where(x => x.Id == pathsArray.srcArray.id && x.Id == id)
                    from admins in context.Administrators.ToList().Where(x => x.Id == outerConfigs.IdAdministrator).DefaultIfEmpty()

                    from assignments in (
                        from a in context.Assignments.ToList().DefaultIfEmpty()
                        from innerClients in context.Clients.ToList().Where(x => x.Id == a.IdClient).DefaultIfEmpty()
                        group (a?.Client) by a.IdConfig into grp
                        select new { id = grp.Key, ids = grp == null ? null : grp }
                    ).Where(x => x.id == outerConfigs.Id).DefaultIfEmpty()
                    select new
                    {
                        ID = outerConfigs.Id,
                        Name = outerConfigs.Name,
                        CreateDate = outerConfigs.CreateDate,
                        Cron = outerConfigs.Cron,
                        BackupFormat = outerConfigs.BackUpFormat,
                        BackupType = outerConfigs.BackUpType,
                        RetentionPackages = outerConfigs.RetentionPackages,
                        RetentionPackageSize = outerConfigs.RetentionPackageSize,
                        //AdminName = admins != null ? admins.Name : null,
                        AdminName = admins?.Name,
                        Sources = pathsArray.srcArray.sources,
                        Destinations = pathsArray.destArray,
                        ////Clients = assignments != null ? assignments.ids.Select(x => new { x.Id, x.Name }) : null
                        Clients = assignments?.ids.Select(x => new { x.Id, x.Name })
                    };
                foreach (var item in q)
                {
                    if (item.ID != id)
                        continue;
                    List<DataDestination> dests = new List<DataDestination>();
                    foreach (var dest in item.Destinations)
                    {
                        if (dest != null)
                            dests.Add(new DataDestination() { DestType = dest.DestType, Path = dest.Path });
                    }
                    DataConfig data = new DataConfig()
                    {
                        Id = item.ID,
                        Sources = item.Sources.ToList()[0] == null ? new List<string>() : item.Sources.ToList(),
                        Destinations = dests,
                        CreateDate = item.CreateDate.ToString("s"),
                        BackUpFormat = item.BackupFormat,
                        BackUpType = item.BackupType,
                        Cron = item.Cron,
                        Name = item.Name,
                        RetentionPackages = item.RetentionPackages,
                        RetentionPackageSize = item.RetentionPackageSize,
                        AdminName = item.AdminName != null ? item.AdminName : "Deleted User",
                        ClientNames = item.Clients?.ToDictionary(x => x.Id, x => x.Name)
                    };
                    return new JsonResult(data) { StatusCode = (int)HttpStatusCode.OK };
                }
                return new JsonResult("Config not found!") { StatusCode = (int)HttpStatusCode.NotFound };
            }
            catch (Exception)
            {
                return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.NotFound };
            }
        }

        [HttpPut("updateConfig")]
        public JsonResult UpdateConfig(int id, DataConfig editedConfig)
        {
            List<int> temp = context.Assignments.Where(x => x.IdConfig == id).Select(x => x.Id).ToList();
            List<Task> tasks = new List<Task>();
            List<Assignment> ass = context.Assignments.Where(x => x.IdConfig == id).ToList();

            foreach (var item in context.Tasks.Where(x => temp.Contains(x.IdAssignment)))
            {
                tasks.Add(item);
            }
            RemoveConfig(id);
            try
            {
                Administrator creator = context.Administrators.Where(x => x.Id == editedConfig.IdAdministrator).FirstOrDefault();
                Config newConfig = new Config()
                {
                    Id = id,
                    Name = editedConfig.Name,
                    CreateDate = DateTime.Parse(editedConfig.CreateDate),
                    Cron = editedConfig.Cron,
                    BackUpFormat = editedConfig.BackUpFormat,
                    BackUpType = editedConfig.BackUpType,
                    RetentionPackages = editedConfig.RetentionPackages,
                    RetentionPackageSize = editedConfig.RetentionPackageSize,
                    Sources = new List<Source>(),
                    Destinations = new List<Destination>(),

                    Administrator = creator
                };
                foreach (var source in editedConfig.Sources)
                {
                    newConfig.Sources.Add(new Source() { Config = newConfig, Path = source });
                }

                foreach (var dest in editedConfig.Destinations)
                {
                    newConfig.Destinations.Add(new Destination() { Config = newConfig, Path = dest.Path, DestType = dest.DestType });
                }
                context.Configs.Add(newConfig);
                context.SaveChanges();
                foreach (var client in editedConfig.ClientNames)
                {
                    Assignment assignment = new Assignment() { IdConfig = newConfig.Id, IdClient = client.Key };
                    if (ass.Where(x => x.IdConfig == assignment.IdConfig && x.IdClient == assignment.IdClient).Count() != 0)
                    {
                        foreach (var item in ass)
                        {
                            if (item.IdConfig == assignment.IdConfig && item.IdClient == assignment.IdClient)
                                context.Assignments.Add(item);
                        }
                    }
                    else
                        context.Assignments.Add(assignment);
                }

                context.SaveChanges();
                foreach (var item in tasks)
                {
                    context.Tasks.Add(item);
                }
                context.SaveChanges();
                new Tasker().RegenerateTaskDatabaseTable();
                return new JsonResult("Success") { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception)
            {
                return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.NotFound };
            }
        }
        [HttpDelete("removeConfig")]
        public JsonResult RemoveConfig(int idConfig)
        {
            try
            {
                Config editConfig = context.Configs.Find(idConfig);
                context.Configs.Remove(editConfig);
                context.SaveChanges();
                new Tasker().RegenerateTaskDatabaseTable();
                return new JsonResult("Success") { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception)
            {
                return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.NotFound };
            }
        }
        [HttpDelete("removeClientFromConfig")]
        public JsonResult RemoveClientFromConfig(int idConfig, int idClient) //RemoveConfigFromClient
        {
            Assignment assignment = context.Assignments.Where(x => x.IdConfig == idConfig && x.IdClient == idClient).FirstOrDefault();

            if (assignment != null)
            {
                context.Assignments.Remove(assignment);
                context.SaveChanges();
                return new JsonResult("Success") { StatusCode = (int)HttpStatusCode.OK };
            }
            return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.NotFound };
        }

        //Logs
        [HttpGet("allLogs")]
        public JsonResult GetAllLogs()
        {
            try
            {
                DateTime today = DateTime.Now;
                var q =
                from assignments in context.Assignments.ToList()
                join tasks in context.Tasks.ToList() on assignments.Id equals tasks.IdAssignment
                join clients in context.Clients.ToList() on assignments.IdClient equals clients.Id
                join configs in context.Configs.ToList() on assignments.IdConfig equals configs.Id
                orderby tasks.Date
                where tasks.Date < today
                select new
                {
                    Id = tasks.Id,
                    State = tasks.State,
                    Message = tasks.Message,
                    Date = tasks.Date,
                    //Size = tasks.Size, //Neukazujeme, možná změna?
                    Config_name = configs.Name,
                    Client_name = clients.Name,
                };
                return new JsonResult(q) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception)
            {
                return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.NotFound };
            }
        }

        //Clients
        [HttpGet("allClients")]
        public JsonResult GetAllClients()
        {
            try
            {
                var q =
                    from clients in context.Clients.ToList()
                    from assignments in context.Assignments.ToList().Where(x => x.IdClient == clients.Id).DefaultIfEmpty()
                    from configs in context.Configs.ToList().Where(x => (assignments != null ? x.Id == assignments.IdConfig : false)).DefaultIfEmpty()
                    from administrators in context.Administrators.ToList().Where(x => (configs != null ? x.Id == configs.IdAdministrator : false)).DefaultIfEmpty()
                    group configs by clients.Id into grp
                    from clients in context.Clients.ToList().Where(x => x.Id == grp.Key).DefaultIfEmpty()
                    select new
                    {
                        Id = grp.Key,
                        Name = clients.Name,
                        Ip = clients.IpAddress,
                        Mac = clients.MacAddress,
                        Active = clients.Active,
                        Online = (DateTimeOffset.Now - clients.LastSeen).TotalMinutes <= 5,
                        Configs = grp.Select(x => x == null ? null : new
                        {
                            id = x.Id,
                            name = x.Name,
                            createDate = x.CreateDate,
                            backUpType = x.BackUpType,
                            adminName = x.Administrator != null ? x.Administrator.Name : "Deleted User",
                        })
                    };
                return new JsonResult(q) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception)
            {
                return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.NotFound };
            }


        }
        [HttpDelete("removeClient")]
        public JsonResult RemoveClient(int idClient)
        {
            Client client = context.Clients.Find(idClient);

            if (client != null)
            {
                context.Clients.Remove(client);
                context.SaveChanges();
                return new JsonResult("Client was successfuly removed") { StatusCode = (int)HttpStatusCode.OK };
            }
            return new JsonResult("User not found!") { StatusCode = (int)HttpStatusCode.NotFound };
        }
        [HttpPut("activeChange")]
        public JsonResult ActiveClientChange(Dictionary<int, bool> changes)
        {
            foreach (var item in changes)
            {
                Client client = context.Clients.Find(item.Key);
                if (client == null)
                {
                    return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.NotFound };
                }
                client.Active = item.Value;
            }
            try
            {
                context.SaveChanges();
                return new JsonResult("Success") { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception)
            {
                return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }

        //Users
        [HttpGet("allUsers")]
        public JsonResult GetAllUsers()
        {
            try
            {
                var q =
                from admins in context.Administrators.ToList()
                from logins in context.LoginLog.ToList().Where(x => x.IdAdministrator == admins.Id).DefaultIfEmpty()
                group (logins) by admins.Id into grp
                from admins in context.Administrators.ToList().Where(x => x.Id == grp.Key).DefaultIfEmpty()
                select new
                {
                    Id = grp.Key,
                    Name = admins.Name,
                    Surname = admins.Surname,
                    CreateDate = admins.AccountCreation,
                    Email = admins.Email,
                    Logs = grp.ToList().Select(x => x == null ? null : new { x.LoginTime, x.IpAddress })
                };
                return new JsonResult(q) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception)
            {

                return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.NotFound };
            }
        }
        [HttpDelete("removeUser")]
        public JsonResult RemoveUser(int idUser)
        {
            if (idUser == 1)
                return new JsonResult("Cannot remove this user!") { StatusCode = (int)HttpStatusCode.BadRequest };
            Administrator admin = context.Administrators.Find(idUser);
            if (admin != null)
            {
                context.Administrators.Remove(admin);
                context.SaveChanges();
                return new JsonResult("User removed successfuly.") { StatusCode = (int)HttpStatusCode.OK };
            }
            return new JsonResult("Failed to remove user!") { StatusCode = (int)HttpStatusCode.BadRequest };
        }
        [HttpPost("addUser")]
        public JsonResult AddUser(Administrator user)
        {
            if (context.Administrators.Where(x => x.Login == user.Login).FirstOrDefault() == null)
            {
                Administrator admin = new Administrator()
                {
                    Login = user.Login,
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Email,
                    DarkMode = user.DarkMode,
                    AccountCreation = DateTime.Now.ToString("s"),
                    Password = BCrypt.Net.BCrypt.HashPassword(user.Password)
                };

                context.Administrators.Add(admin);
                context.SaveChanges();
                return new JsonResult("User added successfuly.") { StatusCode = (int)HttpStatusCode.OK };
            }
            return new JsonResult("Failed to add user!") { StatusCode = (int)HttpStatusCode.BadRequest };
        }
        [HttpGet("clientsByConfig")]
        public JsonResult ClientsByConfig(int idConfig)
        {
            try
            {
                if (context.Configs.Find(idConfig) == null)
                    throw new Exception();
                var q = from clients in context.Clients.ToList().Where(x => x.Active)
                        from a in context.Assignments.ToList().Where(x => x.IdClient == clients.Id && x.IdConfig == idConfig).DefaultIfEmpty()
                        select new
                        {
                            Id = clients.Id,
                            Name = clients.Name,
                            Active = a == null ? false : true
                        };
                return new JsonResult(q) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch
            {
                return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.NotFound };
            }
        }
        [HttpPut("changeClientsOnConfig")]
        public JsonResult ChangeClientsOnConfig(int idConfig, Dictionary<int, bool> changes)
        {
            try
            {
                Config config = context.Configs.Find(idConfig);
                if (config == null)
                    return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.NotFound };
                foreach (var item in context.Assignments)
                {
                    if (item.IdConfig == idConfig && changes.ContainsKey(item.IdClient))
                    {
                        bool change = changes[item.IdClient];
                        changes.Remove(item.IdClient);
                        if (!change)
                            context.Assignments.Remove(item);
                    }
                }
                foreach (var item in changes.Where(x => x.Value == true))
                {
                    Assignment assignment = new Assignment() { Config = config, IdClient = item.Key };
                    context.Assignments.Add(assignment);
                }
                context.SaveChanges();
                return new JsonResult("Success") { StatusCode = (int)HttpStatusCode.OK };
            }
            catch
            {
                return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.NotFound };
            }

        }
        [HttpPut("darkmodeChange")]
        public JsonResult DarkmodeChange(int idUser, bool change)
        {
            try
            {
                Administrator admin = context.Administrators.Find(idUser);
                if (admin == null)
                    return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.NotFound };
                admin.DarkMode = change;
                context.SaveChanges();
                return new JsonResult("Success") { StatusCode = (int)HttpStatusCode.OK };
            }
            catch
            {
                return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.BadRequest };
            }

        }

        [HttpPut("changeEmailSettings")]
        public JsonResult ChangeEmailSettings(EmailSettings data)
        {
            try
            {
                StreamWriter sw = new StreamWriter(dataPath);
                sw.WriteLine(JsonConvert.SerializeObject(data));

                sw.Close();
                return new JsonResult("Success") { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception)
            {
                return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.BadRequest };
            }

        }
        [HttpGet("getEmailSettings")]
        public JsonResult GetEmailSettings()
        {
            try
            {
                StreamReader sr = new StreamReader(dataPath);
                string data = sr.ReadToEnd();
                EmailSettings emailSettings = JsonConvert.DeserializeObject<EmailSettings>(data);
                if (emailSettings == null)
                    return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.NotFound };
                sr.Close();
                return new JsonResult(emailSettings) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }

    }
}
