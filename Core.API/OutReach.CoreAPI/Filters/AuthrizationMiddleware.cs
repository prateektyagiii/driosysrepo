using OutReach.CoreSharedLib.Models.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace OutReach.CoreAPI.Middlewares
{

    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public Task Invoke(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated && context.Request.Headers.Any(x => x.Key == "Authorization") && !string.IsNullOrEmpty(context.Request.Headers.SingleOrDefault(x => x.Key == "Authorization").Value))
            {
                string token = context.Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value.ToString().Substring(6).Trim();
                if (new JwtSecurityTokenHandler().CanReadToken(token))
                {
                    JwtSecurityToken jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                    LoginUser loginUser = new LoginUser();
                    loginUser.UserCode = Convert.ToInt32(jwt.Claims.FirstOrDefault(x => x.Type == "UserCode").Value);
                    loginUser.UserName = jwt.Claims.FirstOrDefault(x => x.Type == "unique_name").Value;
                    loginUser.UserEmail = jwt.Claims.FirstOrDefault(x => x.Type == "email").Value;
                    loginUser.Firstname = jwt.Claims.FirstOrDefault(x => x.Type == "given_name").Value;
                    loginUser.Lastname = jwt.Claims.FirstOrDefault(x => x.Type == "family_name").Value;
                    loginUser.Middlename = (jwt.Claims.FirstOrDefault(x => x.Type == "Middlename").Value);

                    context.Request.HttpContext.Items["LoginUser"] = loginUser;
                }
            }
            return _next(context);
        }
    }
}
