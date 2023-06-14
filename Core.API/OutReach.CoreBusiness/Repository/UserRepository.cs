using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using NpgsqlTypes;
using OutReach.CoreBusiness.Entities;
using OutReach.CoreBusiness.Entity;
using OutReach.CoreBusiness.Model.Request;
using OutReach.CoreBusiness.Model.Response;
using OutReach.CoreBusiness.Models.Request;
using OutReach.CoreBusiness.Models.Response;
using OutReach.CoreDataAccess.DBFunction;
using OutReach.CoreDataAccess.PostgreDBConnection;
using OutReach.CoreRepository.PGRepository;
using OutReach.CoreSharedLib.Model.Common;
using OutReach.CoreSharedLib.Models.Common;
using OutReach.CoreSharedLib.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OutReach.CoreBusiness.Repository
{
    public class UserRepository
    {
        private IConfiguration configuration;
        private EmailService _emailService;
        private InterestRepository _interestRepository;
        private SchoolRepository _schoolRepository;
        private UploadImage _uploadImage;
        private ProfileConfiguration _profileConfiguration;
        private readonly IWebHostEnvironment _environment;


        PgDbRepository _DBRepo;

        public UserRepository(IConfiguration config, PgDbRepository dBRepo,IWebHostEnvironment webHostEnvironment, EmailService emailService,UploadImage uploadImage, InterestRepository interestRepository, SchoolRepository schoolRepository,ProfileConfiguration profileConfiguration)
        {
            configuration = config;
            _DBRepo = dBRepo;
            _interestRepository = interestRepository;
            _schoolRepository = schoolRepository;
            _emailService = emailService;
            _uploadImage = uploadImage;
            _profileConfiguration = profileConfiguration;
            _environment = webHostEnvironment;
        }


      

        #region GetAllUser
        public async Task<ListResponse<UserTotal>> GetAllUser(GridRequest<UserRequest> request)

        {
            ListResponse<UserTotal> listresponse = new ListResponse<UserTotal>();
            try
            {
                List<DbParameter> parameters = new List<DbParameter>();

                parameters.Add(new NpgsqlParameter { ParameterName = "p_pagenumber", NpgsqlValue = request.pagenumber, NpgsqlDbType = NpgsqlDbType.Integer });
                parameters.Add(new NpgsqlParameter { ParameterName = "p_pagesize", NpgsqlValue = request.pagesize, NpgsqlDbType = NpgsqlDbType.Integer });

                if (!(request.data == null))
                {
                    parameters.Add(new NpgsqlParameter { ParameterName = "p_firstname", NpgsqlValue = request.data.Firstname == null ? string.Empty : request.data.Firstname, NpgsqlDbType = NpgsqlDbType.Text });
                    parameters.Add(new NpgsqlParameter { ParameterName = "p_middlename", NpgsqlValue = request.data.Middlename == null ? string.Empty : request.data.Middlename, NpgsqlDbType = NpgsqlDbType.Text });
                    parameters.Add(new NpgsqlParameter { ParameterName = "p_lastname", NpgsqlValue = request.data.Lastname == null ? string.Empty : request.data.Lastname, NpgsqlDbType = NpgsqlDbType.Text });
                    parameters.Add(new NpgsqlParameter { ParameterName = "p_useremail", NpgsqlValue = request.data.UserEmail == null ? string.Empty : request.data.UserEmail, NpgsqlDbType = NpgsqlDbType.Text });
                    parameters.Add(new NpgsqlParameter { ParameterName = "p_active", NpgsqlValue = request.data.IsActive, NpgsqlDbType = NpgsqlDbType.Boolean });

                }

                var result = await _DBRepo.ExecuteListAsync<UserTotal>(DBFunctions.GetAllUser, parameters);

                if (result.Count > 0)
                {
                    listresponse.Status = ResponseStatus.Success;
                    listresponse.Message = Messages.Success;
                    listresponse.Result = result;
                    listresponse.TotalRecord = result[0].Total;
                }
                else
                {
                    listresponse.Status = ResponseStatus.Fail;
                    listresponse.Message = Messages.RecordNotFound;
                    listresponse.Result = result;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listresponse;

        }
        #endregion

        #region RegisterUser

        public async Task<BaseResponse<RegisterResponse>> RegisterUser(User user)
        {
            BaseResponse<RegisterResponse> response = new BaseResponse<RegisterResponse>();
            try
            {

                if (user.SchoolId == 0) user.SchoolId = null;

                List<DbParameter> parameters = SqlParameterHelper.GetSqlParameters<User>(user);
                var result = await _DBRepo.ExecuteScalarAsync(DBFunctions.UserRegistration, parameters);
                int Id = Convert.ToInt32(result);
                RegisterResponse registerResponse = new RegisterResponse
                {
                    userid = Id,
                    email = user.UserEmail,
                    access_token = null,
                };


                switch (Id)
                {

                    case -1:
                        response.Status = ResponseStatus.AlreadyExists;
                        response.Message = Messages.AlreadyExsist;
                        break;

                    default:
                        response.Status = ResponseStatus.Created;
                        response.Message = Messages.RecordSaved;
                        response.Result = registerResponse;
                        break;

                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;

        }
        #endregion

        #region UpdateUserPassword
        public async Task<BaseResponse<int>> UpdateUserPassword(ForgotPasswordRequest forgotPasswordRequest)
        {
            BaseResponse<int> response = new BaseResponse<int>();

            try
            {
                List<DbParameter> parameters = SqlParameterHelper.GetSqlParameters<ForgotPasswordRequest>(forgotPasswordRequest);
                var result = await _DBRepo.ExecuteScalarAsync(DBFunctions.UserForgotPassword, parameters);
                int Otp = Convert.ToInt32(result);
                if (Otp != 0)
                {
                    string messageBody = "OneTimePassword = " + Otp;
                    var message = new EmailMessage(new List<string> { forgotPasswordRequest.Useremail }, "Otp", messageBody);
                    _emailService.SendEmail(message);
                    response.Status = ResponseStatus.Success;
                    response.Message = Messages.Success;
                    // response.Result = Otp;
                }


            }
            catch (Exception )
            {
                response.Status = ResponseStatus.BadRequest;
                response.Message = Messages.BadRequest;

            }

            return response;

        }
        #endregion

        #region OtpVerifyForPassword
        public async Task<BaseResponse<int>> OtpVerify(OtpVerificationRequest verificationRequest)
        {
            BaseResponse<int> response = new BaseResponse<int>();

            try
            {
                List<DbParameter> parameters = SqlParameterHelper.GetSqlParameters<OtpVerificationRequest>(verificationRequest);
                var result = await _DBRepo.ExecuteScalarAsync(DBFunctions.UserOTPVerify, parameters);
                int status = Convert.ToInt32(result);

                switch (status)
                {
                    case 0:
                        response.Status = ResponseStatus.Timeout;
                        response.Message = Messages.TokenExpired;
                         response.Result = Convert.ToInt32(result);
                        break;
                    case 1:
                        response.Status = ResponseStatus.Success;
                        response.Message = Messages.Success;
                         response.Result = Convert.ToInt32(result);
                        break;
                    default:
                        response.Status = ResponseStatus.Fail;
                        response.Message = Messages.FailResponse;
                        response.Result = Convert.ToInt32(result);
                        break;

                }

            }
            catch (Exception )
            {
                response.Status = ResponseStatus.BadRequest;
                response.Message = Messages.BadRequest;

            }

            return response;

        }
        #endregion

        #region ResetPassword
        public async Task<BaseResponse<int>> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            BaseResponse<int> response = new BaseResponse<int>();

            try
            {
                List<DbParameter> parameters = SqlParameterHelper.GetSqlParameters<ResetPasswordRequest>(resetPasswordRequest);
                var result = await _DBRepo.ExecuteScalarAsync(DBFunctions.UserPasswordUpdate, parameters);

                int status = Convert.ToInt32(result);
                if (status != 0)
                {
                    response.Status = ResponseStatus.Reset;
                    response.Message = Messages.ResetPassword;
                    /*                    response.Result = status;
                    */
                }


            }
            catch (Exception )
            {
                response.Status = ResponseStatus.BadRequest;
                response.Message = Messages.BadRequest;

            }

            return response;

        }
        #endregion

        #region GetUserById
        public async Task<BaseResponse<User>> GetUserById(int id)
        {
            BaseResponse<User> response = new BaseResponse<User>();
            try
            {
                List<DbParameter> parameters = new List<DbParameter>();
                parameters.Add(new NpgsqlParameter { ParameterName = "p_id", NpgsqlValue = id, NpgsqlDbType = NpgsqlDbType.Integer });

                User result = await _DBRepo.ExecuteFirstOrDefaultAsync<User>(DBFunctions.UserProfileGetById, parameters);

                if (result != null)
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = Messages.Success;
                    response.Result = result;
                }
            }
            catch (Exception )
            {
                response.Status = ResponseStatus.BadRequest;
                response.Message = Messages.BadRequest;
            }
            return response;

        }
        #endregion

        #region UpdateProfile
        public async Task<BaseResponse<int>> UpdateProfile(User user)
        {
            BaseResponse<int> response = new BaseResponse<int>();

            try
            {
                List<DbParameter> parameters = SqlParameterHelper.GetSqlParameters<User>(user);
                int result = await _DBRepo.ExecuteNonQueryAsync(DBFunctions.UserProfileUpdate, parameters);

                if (result != 0)
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = Messages.Success;
                    // response.Result = result;

                }


            }
            catch (Exception )
            {
                response.Status = ResponseStatus.BadRequest;
                response.Message = Messages.BadRequest;
            }

            return response;

        }
        #endregion

        #region AddSchoolForProfile

        public async Task<BaseResponse<int>> AddSchoolForProfile(AddSchoolForProfileRequest addSchoolForProfile)
        {
            BaseResponse<int> response = new BaseResponse<int>();
            try
            {
                var userDetails = await GetUserById(addSchoolForProfile.Id);
                userDetails.Result.SchoolId = addSchoolForProfile.SchoolId;

                if (userDetails.Result != null)
                {
                    var result = await UpdateProfile(userDetails.Result);
                    if (result != null)
                    {
                        response.Status = ResponseStatus.Success;
                        response.Message = Messages.Success;
                        // response.Result = result.Result;
                    }

                }

            }
            catch (Exception )
            {
                response.Status = ResponseStatus.BadRequest;
                response.Message = Messages.BadRequest;
            }
            return response;

        }
        #endregion

        #region AddUserInterest
        public async Task<BaseResponse<int>> UserInterest(UserInterestRequest userInterestRequest)
        {
            BaseResponse<int> response = new BaseResponse<int>();

            try
            {
                List<DbParameter> parameters = new List<DbParameter>();

                var allInterest = userInterestRequest.InterestId;
                int Id = -1;

                foreach (int temp in allInterest)
                {
                    parameters.Add(new NpgsqlParameter { ParameterName = "p_userid", NpgsqlValue = userInterestRequest.Userid, NpgsqlDbType = NpgsqlDbType.Integer });
                    parameters.Add(new NpgsqlParameter { ParameterName = "p_interestid", NpgsqlValue = temp, NpgsqlDbType = NpgsqlDbType.Integer });

                    var result = await _DBRepo.ExecuteScalarAsync(DBFunctions.UserInterest, parameters);
                    Id = Convert.ToInt32(result);
                    parameters.Clear();

                }
                switch (Id)
                {

                    case -1:
                        response.Status = ResponseStatus.AlreadyExists;
                        response.Message = Messages.AlreadyExsist;
                        break;

                    default:
                        response.Status = ResponseStatus.Created;
                        response.Message = Messages.RecordSaved;
                        //response.Result = ;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;

        }
        #endregion

        #region UserProfile

        public async Task<BaseResponse<UserProfileResponse>> UserProfile(int id)
        {
            BaseResponse<UserProfileResponse> response = new BaseResponse<UserProfileResponse>();

            try
            {
                List<DbParameter> parameters = new List<DbParameter>();
                parameters.Add(new NpgsqlParameter { ParameterName = "p_id", NpgsqlValue = id, NpgsqlDbType = NpgsqlDbType.Integer });

                var userDetails = await GetUserById(id);
                
                var userInterest = await _interestRepository.GetInterestByUserId(id);
                var schoolDetails = await _schoolRepository.GetSchoolById((int)userDetails.Result.SchoolId);

                UserProfileResponse userProfile = new UserProfileResponse
                {
                    UserProfile = userDetails.Result,
                    Schoolname = schoolDetails.Result.Name,
                    Interest = userInterest.Result

                };

                User result = await _DBRepo.ExecuteFirstOrDefaultAsync<User>(DBFunctions.UserProfileGetById, parameters);

                if (result != null)
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = Messages.Success;
                    response.Result = userProfile;
                }


            }
            catch (Exception )
            {
                response.Status = ResponseStatus.BadRequest;
                response.Message = Messages.BadRequest;
            }
            return response;

        }
        #endregion

        #region UpdateUserProfile

        public async Task<BaseResponse<int>> UpdateUserProfile(UserProfileRequest userProfileRequest)
        {
            BaseResponse<int> response = new BaseResponse<int>();

            try
            {
                List<DbParameter> parameters = new List<DbParameter>();

                var userInterest = await _interestRepository.GetInterestByUserId(userProfileRequest.UserProfile.Userid);

                foreach (var temp in userInterest.Result)
                {
                    parameters.Add(new NpgsqlParameter { ParameterName = "p_tagsid", NpgsqlValue = temp.TagsId, NpgsqlDbType = NpgsqlDbType.Integer });
                    parameters.Add(new NpgsqlParameter { ParameterName = "p_isdeleted", NpgsqlValue = false, NpgsqlDbType = NpgsqlDbType.Boolean });
                    await _DBRepo.ExecuteScalarAsync(DBFunctions.DeleteInterest, parameters);
                    parameters.Clear();
                }

                UserInterestRequest userInterestRequest = new UserInterestRequest
                {
                    Userid = userProfileRequest.UserProfile.Userid,
                    InterestId = userProfileRequest.Interest
                };

                await UserInterest(userInterestRequest);

                parameters = SqlParameterHelper.GetSqlParameters<User>(userProfileRequest.UserProfile);
                var result = await _DBRepo.ExecuteScalarAsync(DBFunctions.UserProfileUpdate, parameters);

                if (result != null)
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = Messages.Success;
                    response.Result = Convert.ToInt32(result);
                }


            }
            catch (Exception )
            {
                response.Status = ResponseStatus.BadRequest;
                response.Message = Messages.BadRequest;
            }
            return response;

        }
        #endregion

        #region AddUserProfile
        public async Task<BaseResponse<string>> AddUserProfile(int id, byte[] byteArray, string UniqueFileName)
        {
            BaseResponse<string> response = new BaseResponse<string>();

            try
            {
               

                var userDetails = await GetUserById(id);
              
                if (byteArray != null)
                {
                    var filePath = _environment.ContentRootPath + _profileConfiguration.FolderName;

                    string imgPath = Path.Combine(filePath, UniqueFileName);


                    File.WriteAllBytes(imgPath, byteArray);


                    userDetails.Result.ProfileImg = _profileConfiguration.UserImageUrl + _profileConfiguration.FolderName + UniqueFileName ;
                    var result = await UpdateProfile(userDetails.Result);

                    if (result.Status == ResponseStatus.Success)
                    {
                        response.Status = ResponseStatus.Success;
                        response.Message = "Profile Image Uploaded Successfully";
                        response.Result = userDetails.Result.ProfileImg;

                   }
                  
                }
              



            }
            catch
            {
                response.Status = ResponseStatus.BadRequest;
                response.Message =Messages.BadRequest;
            }

            return response;

        }

        #endregion

        #region UpdateUserProfileImage
        public async Task<BaseResponse<int>> UpdateUserProfileImage(int id, SaveFileRequest saveFileRequest)
        {
            BaseResponse<int> response = new BaseResponse<int>();

            try
            {
                var userDetails = await GetUserById(id);
                if (userDetails.Result.ProfileImg.IsNullOrEmpty())
                {
                    string filename = await _uploadImage.SaveImage(id, saveFileRequest.ProfileImg);
                    userDetails.Result.ProfileImg = filename;
                    var result = UpdateProfile(userDetails.Result);

                }
                else
                {

                }


            }
            catch
            {

            }

            return response;

        }

        #endregion

       }
}
