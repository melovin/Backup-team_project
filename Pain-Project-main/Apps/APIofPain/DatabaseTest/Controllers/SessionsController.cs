using DatabaseTest.DatabaseTables;
using DatabaseTest.Logins;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace DatabaseTest.Controllers
{
    [ApiController]
    [Route("AdminPage")]
    public class SessionsController : ControllerBase
    {
        private AuthService auth = new ();
        private MyContext context = new ();

        [HttpPost("login")]
        public JsonResult Login(Credentials cd)
        {
            try
            {
                var ip = HttpContext.Connection.RemoteIpAddress.ToString();

                return new JsonResult(this.auth.Authenticate(cd, ip != null ? ip : ""));
            }
            catch
            {
                return new JsonResult("Invalid username or password!") { StatusCode = (int)HttpStatusCode.Unauthorized };
            }
        }
        [Auth]
        [HttpGet("relog")]
        public JsonResult Relog(int id)
        {
            try
            {
                return new JsonResult(this.auth.ReLog(id));
            }
            catch
            {
                return new JsonResult("Cannot resolve request!") { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }
    }
}
