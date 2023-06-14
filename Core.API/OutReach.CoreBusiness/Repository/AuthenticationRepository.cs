using OutReach.CoreDataAccess.PostgreDBConnection;
using OutReach.CoreDataAccess.DBFunction;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using OutReach.CoreBusiness.Models.Request;
using OutReach.CoreSharedLib.Models.Common;
using OutReach.CoreBusiness.Entities;

namespace OutReach.CoreRepository.PGRepository
{
    /// <summary>
    /// Report repository for communicate postgres database
    /// </summary>
    public class AuthenticationRepository
    {
        private IConfiguration configuration;

        PgDbRepository _DBRepo;
        public AuthenticationRepository(IConfiguration config, PgDbRepository dBRepo)
        {
            configuration = config;
            _DBRepo = dBRepo;
        }

        #region Authentication
        public async Task<LoginUser> UserAuthentication(LoginRequest _photoAuditRequest)
        {
            List<DbParameter> parameters = SqlParameterHelper.GetSqlParameters<LoginRequest>(_photoAuditRequest);
            LoginUser result = await _DBRepo.ExecuteFirstOrDefaultAsync<LoginUser>(DBFunctions.GetUserAuthentication, parameters);
            return result;
        }
        public async Task<int> SaveUserAuthSession(LoginUserSession userAuthSession)
        {
            List<DbParameter> parameters = SqlParameterHelper.GetSqlParameters<LoginUserSession>(userAuthSession);
            int result = await _DBRepo.ExecuteNonQueryAsync(DBFunctions.SaveUserAuthSession, parameters);
            return result;
        }
      
        #endregion
    }
}
