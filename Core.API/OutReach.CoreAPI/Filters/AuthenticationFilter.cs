using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using OutReach.CoreSharedLib.Models.Common;
using OutReach.CoreSharedLib.Utilities;
using OutReach.CoreService.Service;
using Microsoft.AspNetCore.Authorization;

namespace OutReach.CoreAPI.Filters
{
    public class AuthenticationFilter : ActionFilterAttribute
    {
        AuthenticationService _authservice;
        public AuthenticationFilter(AuthenticationService _authenticationService)
        {
            _authservice = _authenticationService;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (this.IsAnonymousAction(filterContext))
            //{
            //    return;
            //}

            //var AppToken = filterContext.HttpContext.Request.Headers.SingleOrDefault(x => x.Key == "Authorization").Value.ToString().Replace("Bearer", "").Trim();
            //string jwtToken = string.Empty;

            //if (!string.IsNullOrEmpty(AppToken) && AppToken.Count() > 0)
            //    jwtToken = AppToken;

            //var ObjUser = _authservice.IsValidTokenUser(jwtToken);

            //if (ObjUser != null)
            //{
            //    LoginUser objLoginUser = new LoginUser();

            //    objLoginUser.UserID = ObjUser._id;
            //    objLoginUser.CognitoUserName = ObjUser.USER_ID;
            //    objLoginUser.Email = ObjUser.EMAIL_ID;

            //    filterContext.HttpContext.Request.HttpContext.Items["LoginUser"] = objLoginUser;
            //}
            //else
            //    filterContext.Result = new JsonResult(new { System.Net.HttpStatusCode.Unauthorized });

            #region USING AuthorizationMiddleware
            if (!filterContext.ModelState.IsValid)
            {
                var resp = Utility.BadRequest();
                filterContext.Result = new ContentResult { Content = resp, ContentType = "application/json" };
                return;
            }
            filterContext.HttpContext.Request.HttpContext.Items["LoginUser"] = (LoginUser)filterContext.HttpContext.Items["LoginUser"];
            filterContext.HttpContext.Request.HttpContext.Items["RequestData"] = JsonConvert.SerializeObject(filterContext.ActionArguments.Values);
            filterContext.HttpContext.Request.HttpContext.Items["PageEnterTime"] = DateTime.UtcNow;
            #endregion
        }

        // For by pass any method  used attribute [AllowAnonymous] on method
        private bool IsAnonymousAction(ActionExecutingContext filterContext)
        {
            var controllerActionDescriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                return controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                    .Any(a => a.GetType().Equals(typeof(AllowAnonymousAttribute)));
            }
            return false;
        }

    }
}
