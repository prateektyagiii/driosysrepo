using OutReach.CoreBusiness.Models.Request;
using OutReach.CoreBusiness.Models.Response;
using OutReach.CoreSharedLib.Models.Common;
using OutReach.CoreBusiness.Entity;
using OutReach.CoreRepository.PGRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OutReach.CoreSharedLib.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OutReach.CoreService.Service
{
    /// <summary>
    /// Sissense service Interface
    /// </summary>
    public interface IAuthenticationService
    {
        Task<BaseResponse<LoginResponse>> UserAuthentication(LoginRequest _request);
        //Task<BaseResponse<List<LoginRequest>>> GetInAppUser(LoginRequest _photoAuditRequest);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppSettings _appSettings;
        AuthenticationRepository _authRepository;
        private IConfiguration _configuration;

        public AuthenticationService(IHttpContextAccessor httpContextAccessor, IOptions<AppSettings> appSettings, AuthenticationRepository authenticationRepository
            , IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _appSettings = appSettings.Value;
            _authRepository = authenticationRepository;
            _configuration = configuration;
        }

        #region Native Reports
        public async Task<BaseResponse<LoginResponse>> UserAuthentication(LoginRequest _request)
        {
            var response = new BaseResponse<LoginResponse>();
            response.Status = ResponseStatus.Fail;
            try
            {
                LoginUser _loginuser = await _authRepository.UserAuthentication(_request);
                if (_loginuser != null)
                {
                    LoginUserSession userAuthSession;
                    CheckUser(_loginuser, out userAuthSession);
                    int result = await _authRepository.SaveUserAuthSession(userAuthSession);

                    int tokenExpiryHours = Convert.ToInt32(_appSettings.TokenExpiryInHour);
                    LoginResponse Loginresponse = new LoginResponse();
                    Loginresponse.access_token = userAuthSession.OneTimeToken;
                    Loginresponse.userid = _loginuser.UserEmail;
                    Loginresponse.UserCode = _loginuser.UserCode;
                    Loginresponse.token_type = "Bearer";
                    Loginresponse.issued_at = DateTime.UtcNow.Ticks.ToString();
                    Loginresponse.expire_at = DateTime.UtcNow.AddHours(tokenExpiryHours).Ticks.ToString();
                    Loginresponse.expires_in = (DateTime.UtcNow.AddHours(tokenExpiryHours).Ticks - DateTime.UtcNow.Ticks).ToString();

                    response.Status = ResponseStatus.Success;
                    response.Result = Loginresponse;
                    response.Message = Messages.LoginSuccess;
                }

            }
            catch (Exception )
            {
                response.Result = null;
                response.Status = ResponseStatus.UnAuthorized;
                response.Message = Messages.InvalidUserNamePassword;
            }
            return response;
        }
        private void CheckUser(LoginUser user, out LoginUserSession userAuthSession)
        {
            try
            {
                int tokenExpiryHours = Convert.ToInt32(_appSettings.TokenExpiryInHour);
                string Token = BuildToken(user, tokenExpiryHours);

                userAuthSession = new LoginUserSession();
                userAuthSession.SessionId = Guid.NewGuid().ToString();
                userAuthSession.OneTimeToken = Token;
                userAuthSession.UserCode = user.UserCode;
                userAuthSession.UserEmail = user.UserEmail;
                userAuthSession.ValidTill = DateTime.UtcNow.AddHours(tokenExpiryHours);

            }
            catch (Exception )
            {
                throw;
            }
        }
        private string BuildToken(LoginUser user, int tokenExpiryHours)
        {

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName));
            claims.Add(new Claim(JwtRegisteredClaimNames.GivenName, user.Firstname));
            claims.Add(new Claim(JwtRegisteredClaimNames.FamilyName, user.Lastname));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.UserEmail));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, user.UserEmail));

            claims.Add(new Claim("Middlename", user.Middlename ?? String.Empty));
            claims.Add(new Claim("UserCode", Convert.ToString(user.UserCode)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(tokenExpiryHours),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion
    }
}