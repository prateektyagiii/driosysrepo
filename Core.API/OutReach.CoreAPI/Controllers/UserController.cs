using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutReach.CoreAPI.Filters;
using OutReach.CoreBusiness.Entities;
using OutReach.CoreBusiness.Entity;
using OutReach.CoreBusiness.Model.Request;
using OutReach.CoreBusiness.Model.Response;
using OutReach.CoreBusiness.Models.Request;
using OutReach.CoreBusiness.Models.Response;
using OutReach.CoreBusiness.Repository;
using OutReach.CoreRepository.PGRepository;
using OutReach.CoreService.Service;
using OutReach.CoreSharedLib.Model.Common;
using OutReach.CoreSharedLib.Models.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ResponseBase = OutReach.CoreSharedLib.Models.Common.ResponseBase;

namespace OutReach.CoreAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {

        private UserRepository _userRepository;
        private IAuthenticationService _authservice;
        private AuthenticationRepository _authenticationRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly ProfileConfiguration _profileConfiguration;


        public UserController(UserRepository userRepository, AuthenticationRepository authenticationRepository, AuthenticationService _authenticationService,IWebHostEnvironment webHostEnvironment,ProfileConfiguration profileConfiguration)
        {
            _userRepository = userRepository;
            _authservice = _authenticationService;
            _authenticationRepository = authenticationRepository;
            _environment = webHostEnvironment;
            _profileConfiguration = profileConfiguration;
        }

        [AllowAnonymous]
        [HttpPost("GetAllUser")]
        public async Task<ActionResult> GetAllUser([FromQuery] GridRequest<UserRequest> gridRequest)
        {
            var result = await _userRepository.GetAllUser(gridRequest);

            return Ok(result);

        }



        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(User user)
        {
            var UserResponse = await _userRepository.RegisterUser(user);
            if(UserResponse.Result != null)
            {
                LoginRequest loginRequest = new LoginRequest()
                {
                    Username = user.UserEmail,
                    Password = user.UserPassword

                };

                var result = await _authservice.UserAuthentication(loginRequest);
                UserResponse.Result.access_token = result.Result.access_token;
            }
           
            

            return Ok(UserResponse);
            



        }

      

        [Authorize]
        [AllowAnonymous]
        [HttpGet("GetUserById/{id}")]

        public async Task<IActionResult> GetUserById(int id)
        {

            return Ok(await _userRepository.GetUserById(id));

        }

        [Authorize]
        [AllowAnonymous]
        [HttpGet("UserProfile/{id}")]
        public async Task<IActionResult> UserProfile(int id)
        {
            return Ok(await _userRepository.UserProfile(id));
        }

        [Authorize]
        [AllowAnonymous]
        [HttpPost("UpdateUserProfile")]
        public async Task<IActionResult> UpdateUserProfile(UserProfileRequest userProfileRequest)
        {

            return Ok(await _userRepository.UpdateUserProfile(userProfileRequest));

        }


        [AllowAnonymous]
        [HttpPost("AddUserProfile/{id}")]
        //[Consumes("multipart/form-data")]
        public async Task<ActionResult> AddUserProfile(int id,ImageRequestObject imageRequestObject)
        {
            BaseResponse<string> _Response = new BaseResponse<string>();
          
                byte[] byteArray = null;

                if (imageRequestObject?.ImageBase64 != null)
                {
                    byteArray = Convert.FromBase64String(imageRequestObject.ImageBase64);
                    string UniqueFileName = Guid.NewGuid().ToString() + ".jpeg";
                   _Response =await _userRepository.AddUserProfile(id, byteArray, UniqueFileName);
                }
         
            return Ok(_Response);
        }



        
        [AllowAnonymous]
        [HttpPost("UpdateUserpassword")]
        public async Task<IActionResult> UpdateUserpassword(ForgotPasswordRequest forgotPasswordRequest)
        {
                return Ok(await _userRepository.UpdateUserPassword(forgotPasswordRequest));
         }


        [AllowAnonymous]
        [HttpPost("OtpVerification")]
        public async Task<IActionResult> OtpVerification(OtpVerificationRequest verificationRequest)
        {

            return Ok(await _userRepository.OtpVerify(verificationRequest));

        }


        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {

            return Ok(await _userRepository.ResetPassword(resetPasswordRequest));

        }


        [AllowAnonymous]
        [HttpPost("AddSchoolForProfile")]
        public async Task<IActionResult> AddSchoolForProfile(AddSchoolForProfileRequest addSchoolForProfileRequest)
        {
            var result = await _userRepository.AddSchoolForProfile(addSchoolForProfileRequest);
            return Ok(result);

        }

        [AllowAnonymous]
        [HttpPost("UserInterest")]
        public async Task<IActionResult> UserInterest(UserInterestRequest userInterestRequest)
        {

            return Ok(await _userRepository.UserInterest(userInterestRequest));

        }

    }
}
