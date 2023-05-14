using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net;

namespace DatabaseTest.Logins
{
    public class AuthAttribute : Attribute, IAuthorizationFilter
    {
        private AuthService auth = new AuthService();
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string token = context.HttpContext.Request.Headers["Authorization"].ToString().Split(' ').Last();

            if (!this.auth.VerifyToken(token))
                context.Result = new JsonResult("Authentication failed!") { StatusCode = (int)HttpStatusCode.Unauthorized };
        }
    }
}
