using DatabaseTest.Controllers;
using DatabaseTest.DatabaseTables;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace DatabaseTest.Logins
{
    public class AuthService
    {
        const string _SECRET = "SILNE_HESLO";

        private MyContext context = new MyContext();

        public string Authenticate(Credentials cd, string ip)
        {
            Administrator admin = context.Administrators.Where(x => x.Login == cd.Login).FirstOrDefault();
            if (admin == null)
                throw new Exception("Invalid user");

            if (!BCrypt.Net.BCrypt.Verify(cd.Password, admin.Password))
                throw new Exception("Invalid password");

            LoginLog log = new LoginLog() { Administrator = admin, LoginTime = DateTime.Now.ToString("s"), IpAddress = ip };
            context.LoginLog.Add(log);
            context.SaveChanges();

            return JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(_SECRET)
                .AddClaim("exp", DateTimeOffset.UtcNow.AddSeconds(3600).ToUnixTimeSeconds())
                .AddClaim("user", new
                {
                    Id = admin.Id,
                    Create = admin.AccountCreation,
                    Name = admin.Name,
                    Surname = admin.Surname,
                    Email = admin.Email,
                    Darkmode = admin.DarkMode
                })
                .Encode();
        }
        public bool VerifyToken(string token)
        {
            try
            {
                string json = JwtBuilder.Create()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(_SECRET)
                    .MustVerifySignature()
                    .Decode(token);
                return true;
            }
            catch 
            {
                return false;
            }
        }
        public string ReLog(int id)
        {
            Administrator admin = context.Administrators.Where(x => x.Id == id).FirstOrDefault();

            return JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(_SECRET)
                .AddClaim("exp", DateTimeOffset.UtcNow.AddSeconds(3600).ToUnixTimeSeconds())
                .AddClaim("user", new
                {
                    Id = admin.Id,
                    Create = admin.AccountCreation,
                    Name = admin.Name,
                    Surname = admin.Surname,
                    Email = admin.Email,
                    Darkmode = admin.DarkMode
                })
                .Encode();
        }
    }
}
