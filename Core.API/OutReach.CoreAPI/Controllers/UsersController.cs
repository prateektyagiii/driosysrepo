using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WAT.CoreBusiness.Repository;
using WAT.CoreBusiness.Models.Request;
using WAT.CoreBusiness.Entities;
using WAT.CoreAPI.Filters;
using WAT.CoreSharedLib.Models.Common;
using System.Net;

namespace WAT.CoreAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    public class UsersController : ControllerBase
    {
        private IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]LoginRequest userParam)
        {
            //var user = _userRepository.WATCognitoLogin(userParam);
            //if (user == null)
            //    return BadRequest(new { message = "Username or password is incorrect" });

            return Ok();
        }

        [HttpPost("GetUsersEmail")]
        public IActionResult GetUsersEmail(string emailId)
        {
            var objLoginUser = (LoginUser)this.Request.HttpContext.Items["LoginUser"];

            var user = _userRepository.GetbyEmail(emailId);
            return Ok(user);
        }
    }
}