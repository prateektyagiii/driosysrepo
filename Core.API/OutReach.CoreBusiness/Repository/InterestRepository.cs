using Microsoft.Extensions.Configuration;
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
    public class InterestRepository
    {
        private IConfiguration configuration;

        PgDbRepository _DBRepo;
        public InterestRepository(IConfiguration config, PgDbRepository dBRepo)
        {
            configuration = config;
            _DBRepo = dBRepo;
        }

        #region GetAllInterest
        public async Task<ListResponse<InterestResponse>> GetAllInterest(GridRequest<InterestRequest> request)
        {

            ListResponse<InterestResponse> listresponse = new ListResponse<InterestResponse>();
            try
            {
                List<DbParameter> parameters = new List<DbParameter>();

                parameters.Add(new NpgsqlParameter { ParameterName = "p_pagenumber", NpgsqlValue = request.pagenumber, NpgsqlDbType = NpgsqlDbType.Integer });
                parameters.Add(new NpgsqlParameter { ParameterName = "p_pagesize", NpgsqlValue = request.pagesize, NpgsqlDbType = NpgsqlDbType.Integer });

                if (!(request.data == null))
                {
                    parameters.Add(new NpgsqlParameter { ParameterName = "p_name", NpgsqlValue = request.data.Name == null ? string.Empty : request.data.Name, NpgsqlDbType = NpgsqlDbType.Text });
                }
               
                var result = await _DBRepo.ExecuteListAsync<InterestResponse>(DBFunctions.GetAllInterest, parameters);


                if (result.Count>0)
                {
                    listresponse.Status = ResponseStatus.Success;
                    listresponse.Message = "Interest List";
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

        #region GetInterestById

        public async Task<BaseResponse<Interest>> GetInterestById(int id)
        {
                BaseResponse<Interest> response = new BaseResponse<Interest>();
            try
            {
                List<DbParameter> parameters = new List<DbParameter>();

                parameters.Add(new NpgsqlParameter { ParameterName = "p_id", NpgsqlValue = id, NpgsqlDbType = NpgsqlDbType.Integer });
                        
                Interest result = await _DBRepo.ExecuteFirstOrDefaultAsync<Interest>(DBFunctions.GetInterestById, parameters);

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

        #region GetInterestByUserId

        public async Task<ListResponse<Tags>> GetInterestByUserId(int id)
        {
            ListResponse<Tags> response = new ListResponse<Tags>();
            try
            {


                List<DbParameter> parameters = new List<DbParameter>();

                parameters.Add(new NpgsqlParameter { ParameterName = "p_id", NpgsqlValue = id, NpgsqlDbType = NpgsqlDbType.Integer });

                var result = await _DBRepo.ExecuteListAsync<Tags>(DBFunctions.GetInterestByUserId, parameters);

                if (result != null)
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = Messages.Success;
                    response.Result = result;
                    response.TotalRecord = result.Count();
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

        #region AddInterest
        public async Task<BaseResponse<int>> AddInterest(Interest createInterest)
        {
            BaseResponse<int> response = new BaseResponse<int>();
            try
            {


                List<DbParameter> parameters = SqlParameterHelper.GetSqlParameters<Interest>(createInterest);
                var result = await _DBRepo.ExecuteScalarAsync(DBFunctions.AddInterest, parameters);
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

        #region UpdateInterest
        public async Task<BaseResponse<int>> EditInterest(Interest interest)
        {
            BaseResponse<int> response = new BaseResponse<int>();

            try
            {
                List<DbParameter> parameters = SqlParameterHelper.GetSqlParameters<Interest>(interest);
                int result = await _DBRepo.ExecuteNonQueryAsync(DBFunctions.EditInterest, parameters);

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

        #region DeleteInterest
        public async Task<BaseResponse<int>> DeleteInterest(int id, Boolean status)
        {
            BaseResponse<int> response = new BaseResponse<int>();

            try
            {
                List<DbParameter> parameters = SqlParameterHelper.GetSqlParameters<int>(id);

                parameters.Add(new NpgsqlParameter { ParameterName = "p_interestid", NpgsqlValue = id, NpgsqlDbType = NpgsqlDbType.Integer });
                parameters.Add(new NpgsqlParameter { ParameterName = "p_isdeleted", NpgsqlValue = status, NpgsqlDbType = NpgsqlDbType.Boolean });

                int result = await _DBRepo.ExecuteNonQueryAsync(DBFunctions.RemoveInterest, parameters);

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

