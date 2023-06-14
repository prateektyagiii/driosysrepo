using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OutReach.CoreBusiness.Models.Request;
using OutReach.CoreAPI.Filters;
using OutReach.CoreBusiness.Models.Response;
using OutReach.CoreBusiness.Entity;
using System.Threading.Tasks;
using OutReach.CoreService.Service;
using OutReach.CoreBusiness.Entities;
using OutReach.CoreRepository.PGRepository;
using System;
using OutReach.CoreSharedLib.Models.Common;
using Microsoft.AspNetCore.Cors;

namespace OutReach.CoreAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    public class AuthenticateController : ControllerBase
    {
        private IAuthenticationService _authservice;

        private AuthenticationRepository _authenticationRepository;
        public AuthenticateController(AuthenticationService _authenticationService, AuthenticationRepository authenticationRepository )
        {
            _authservice = _authenticationService;
            _authenticationRepository = authenticationRepository;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<BaseResponse<LoginResponse>>> Login(LoginRequest userParam)
        {
            var result = await _authservice.UserAuthentication(userParam);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("AdminLogin")]
        public async Task<ActionResult<BaseResponse<LoginResponse>>> AdminLogin(LoginRequest _request)
        {
            LoginUser _loginuser = await _authenticationRepository.UserAuthentication(_request);
            BaseResponse<LoginResponse> result = new BaseResponse<LoginResponse>();

            if (_loginuser.IsAdmin == 1)
            {
                result = await _authservice.UserAuthentication(_request);
            }
            else
            {
                result.Status = ResponseStatus.UnAuthorized;
                result.Message = Messages.InvalidUserNamePassword;
            }
            return Ok(result);
        }
    }
}