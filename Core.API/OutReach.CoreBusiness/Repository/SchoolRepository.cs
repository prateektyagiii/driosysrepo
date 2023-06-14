using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using NpgsqlTypes;
using OutReach.CoreBusiness.Entities;
using OutReach.CoreBusiness.Entity;
using OutReach.CoreBusiness.Model.Request;
using OutReach.CoreBusiness.Model.Response;
using OutReach.CoreDataAccess.DBFunction;
using OutReach.CoreDataAccess.PostgreDBConnection;
using OutReach.CoreSharedLib.Models.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutReach.CoreBusiness.Repository
{
   
    public class SchoolRepository
    {
        private IConfiguration configuration;

        PgDbRepository _DBRepo;
        public SchoolRepository(IConfiguration config, PgDbRepository dBRepo)
        {
            configuration = config;
            _DBRepo = dBRepo;
        }

        #region GetAllSchool
        public async Task<ListResponse<SchoolResponse>> GetAllSchool(GridRequest<SchoolRequest> request)
        {
            ListResponse<SchoolResponse> listresponse = new ListResponse<SchoolResponse>();
            try
            {
                List<DbParameter> parameters = new List<DbParameter>();

                parameters.Add(new NpgsqlParameter { ParameterName = "p_pagenumber", NpgsqlValue = request.pagenumber, NpgsqlDbType = NpgsqlDbType.Integer });
                parameters.Add(new NpgsqlParameter { ParameterName = "p_pagesize", NpgsqlValue = request.pagesize, NpgsqlDbType = NpgsqlDbType.Integer });
              
                if (!(request.data == null))
                    {
                        parameters.Add(new NpgsqlParameter { ParameterName = "p_name", NpgsqlValue = request.data.Name == null ? string.Empty : request.data.Name, NpgsqlDbType = NpgsqlDbType.Text });
                    parameters.Add(new NpgsqlParameter { ParameterName = "p_city", NpgsqlValue = request.data.City == null ? string.Empty : request.data.City, NpgsqlDbType = NpgsqlDbType.Text });
                }
                
                var result = await _DBRepo.ExecuteListAsync<SchoolResponse>(DBFunctions.GetAllSchool, parameters);



                if (result.Count > 0)
                {
                    listresponse.Status = ResponseStatus.Success;
                    listresponse.Message = Messages.Success;
                    listresponse.Result = result;
                    listresponse.TotalRecord = result[0].Total;
                }
                else
                {
                    listresponse.Status = ResponseStatus.NotFound;
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

        #region GetSchoolById
        public async Task<BaseResponse<School>> GetSchoolById(int id)
        {
            BaseResponse<School> response = new BaseResponse<School>();
            try
            {
               List<DbParameter> parameters = new List<DbParameter>();

              parameters.Add(new NpgsqlParameter { ParameterName = "p_id", NpgsqlValue = id, NpgsqlDbType = NpgsqlDbType.Integer });

                School result = await _DBRepo.ExecuteFirstOrDefaultAsync<School>(DBFunctions.GetSchoolById, parameters);

                if (result != null)
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = Messages.Success;
                    response.Result = result;
                }
                else
                {
                    response.Status = ResponseStatus.Fail;
                    response.Message = Messages.RecordNotFound;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;

        }
        #endregion

        #region AddSchool
        public async Task<BaseResponse<int>> AddSchool(School createSchool)
        {
            BaseResponse<int> response = new BaseResponse<int>();
            try
            {


                List<DbParameter> parameters = SqlParameterHelper.GetSqlParameters<School>(createSchool);
                var result = await _DBRepo.ExecuteScalarAsync(DBFunctions.AddSchool, parameters);
                int Id = Convert.ToInt32(result);

                switch (Id)
                {

                    case -1:
                        response.Status = ResponseStatus.AlreadyExists;
                        response.Message = Messages.AlreadyExsist;
                        break;

                    default:
                        response.Status = ResponseStatus.Created;
                        response.Message = Messages.RecordSaved;
                        // response.Result = Id;
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

        #region UpdateSchool
        public async Task<BaseResponse<int>> UpdateSchool(School school)
        {
            BaseResponse<int> response = new BaseResponse<int>();

            try
            {
                List<DbParameter> parameters = SqlParameterHelper.GetSqlParameters<School>(school);
                int result = await _DBRepo.ExecuteNonQueryAsync(DBFunctions.UpdateSchool, parameters);

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

        #region DeleteSchool
        public async Task<BaseResponse<int>> DeleteSchool(int id,Boolean status)
        {
            BaseResponse<int> response = new BaseResponse<int>();

            try
            {
                List<DbParameter> parameters = new List<DbParameter>();

                parameters.Add(new NpgsqlParameter { ParameterName = "p_schoolid", NpgsqlValue = id, NpgsqlDbType = NpgsqlDbType.Integer });
                parameters.Add(new NpgsqlParameter { ParameterName = "p_isdeleted", NpgsqlValue = status, NpgsqlDbType = NpgsqlDbType.Boolean });

                int result = await _DBRepo.ExecuteNonQueryAsync(DBFunctions.DeleteSchool, parameters);

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
    }
}
