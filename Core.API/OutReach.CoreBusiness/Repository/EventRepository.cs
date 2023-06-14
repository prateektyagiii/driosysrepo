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
   public class EventRepository
    {
        private IConfiguration configuration;
        private UserRepository _userRepository;
        private InterestRepository _interestRepository;

        PgDbRepository _DBRepo;
        private SchoolRepository _schoolRepository;

        public EventRepository(IConfiguration config, PgDbRepository dBRepo, UserRepository userRepository,InterestRepository interestRepository, SchoolRepository schoolRepository)
        {
            configuration = config;
            _userRepository = userRepository;
            _DBRepo = dBRepo;
            _interestRepository = interestRepository;
            _schoolRepository = schoolRepository;
        }

        #region GetAllEvent
        public async Task<ListResponse<EventTotal>> GetAllEvent(GridRequest<EventRequest> request)
            
        {
            ListResponse<EventTotal> listresponse = new ListResponse<EventTotal>();
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

                var result = await _DBRepo.ExecuteListAsync<EventTotal>(DBFunctions.GetAllEvent, parameters);

                if (result.Count>0)
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

        #region GetEventById
        public async Task<BaseResponse<EventDetailResponse>> GetEventById(int id)
        {
            BaseResponse<EventDetailResponse> response = new BaseResponse<EventDetailResponse>();
            try
            {
                List<DbParameter> parameters = new List<DbParameter>();

                parameters.Add(new NpgsqlParameter { ParameterName = "p_id", NpgsqlValue = id, NpgsqlDbType = NpgsqlDbType.Integer });


                //List<DbParameter> parameters = SqlParameterHelper.GetSqlParameters<int>(id);


                EventDetailResponse result = await _DBRepo.ExecuteFirstOrDefaultAsync<EventDetailResponse>(DBFunctions.GetEventById, parameters);

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
       
        #region AddEvent
        public async Task<BaseResponse<int>> AddEvent(Event createEvent)
        {
            BaseResponse<int> response = new BaseResponse<int>();
            try
            {


                List<DbParameter> parameters = SqlParameterHelper.GetSqlParameters<Event>(createEvent);
                var result = await _DBRepo.ExecuteScalarAsync(DBFunctions.AddEvent,parameters);
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

        #region DeleteEvent
        public async Task<BaseResponse<int>> DeleteEvent(int id, Boolean status)
        {
            BaseResponse<int> response = new BaseResponse<int>();
            try
            {


                List<DbParameter> parameters = new List<DbParameter>();
                parameters.Add(new NpgsqlParameter { ParameterName = "p_eventid", NpgsqlValue = id, NpgsqlDbType = NpgsqlDbType.Integer });
                parameters.Add(new NpgsqlParameter { ParameterName = "p_isdeleted", NpgsqlValue = status, NpgsqlDbType = NpgsqlDbType.Boolean });
                var result = await _DBRepo.ExecuteScalarAsync(DBFunctions.DeleteEvent, parameters);

               int Id = Convert.ToInt32(result);



                switch (Id)
                {

                    case 0:
                        response.Status = ResponseStatus.NotFound;
                        response.Message = "Event is Not Exist!";
                        break;

                    default:
                        response.Status = ResponseStatus.Success;
                        response.Message ="Event Delete Sucessfully!";
                        response.Result = Id;
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

        #region LeftEvent
        public async Task<BaseResponse<int>> LeftEvent(int id,int eventid)
        {
            BaseResponse<int> response = new BaseResponse<int>();
            try
            {


                List<DbParameter> parameters = new List<DbParameter>();
                parameters.Add(new NpgsqlParameter { ParameterName = "p_userid", NpgsqlValue = id, NpgsqlDbType = NpgsqlDbType.Integer });
                parameters.Add(new NpgsqlParameter { ParameterName = "p_eventid", NpgsqlValue = eventid, NpgsqlDbType = NpgsqlDbType.Integer });
                var result = await _DBRepo.ExecuteScalarAsync(DBFunctions.LeftEvent, parameters);

                int Id = Convert.ToInt32(result);

                switch (Id)
                {

                    case -1:
                        response.Status = ResponseStatus.AlreadyExists;
                        response.Message = Messages.AlreadyExsist;
                        break;

                    default:
                        response.Status = ResponseStatus.Success;
                        response.Message = "Remove user successfully";
                        response.Result = Id;
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

        #region GetEventByInterest
        public async Task<ListResponse<Event>> GetEventByInterest(int id)
        {
            ListResponse<Event> response = new ListResponse<Event>();
            try
            {
                
                List<DbParameter> parameters = new List<DbParameter>();

                parameters.Add(new NpgsqlParameter { ParameterName = "p_id", NpgsqlValue = id, NpgsqlDbType = NpgsqlDbType.Integer });

                var result = await _DBRepo.ExecuteListAsync<Event>(DBFunctions.GetEventByInterest, parameters);

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
            catch (Exception)
            {
                response.Status = ResponseStatus.BadRequest;
                response.Message = Messages.BadRequest;
            }

            return response;

        }
        #endregion

        #region GetEventByUser
        public async Task<ListResponse<Event>> GetEventByUser(int id)
        {
            ListResponse<Event> response = new ListResponse<Event>();

            try
            {
                List<Event> events =new List<Event>();

                List<DbParameter> parameters = new List<DbParameter>();
                parameters.Add(new NpgsqlParameter { ParameterName = "p_id", NpgsqlValue = id, NpgsqlDbType = NpgsqlDbType.Integer });

                var userprofile = await _userRepository.UserProfile(id);

                foreach (var temp in userprofile.Result.Interest)
                {
                   
                   foreach (var item in GetEventByInterest(Convert.ToInt32(temp.InterestId)).Result.Result)
                    {
                        events.Add(item);
                    }
                }

                if (response != null)
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = Messages.Success;
                    response.Result = events;
                    response.TotalRecord = events.Count();

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

        #region GetEventBynearBy
        public async Task<ListResponse<GetEventBynearByResponse>> GetEventBynearBy(int id)
        {
            ListResponse<GetEventBynearByResponse> response = new ListResponse<GetEventBynearByResponse>();

            try
            {
                List<Event> events = new List<Event>();

                List<DbParameter> parameters = new List<DbParameter>();
                parameters.Add(new NpgsqlParameter { ParameterName = "p_id", NpgsqlValue = id, NpgsqlDbType = NpgsqlDbType.Integer });

                var userprofile = await _userRepository.UserProfile(id);

                var schoolDetails = await _schoolRepository.GetSchoolById((int)userprofile.Result.UserProfile.SchoolId);

                Location schoollocation = new Location
                {
                    Latitude = schoolDetails.Result.Latitude,
                    Longitude = schoolDetails.Result.Longitude
                };

                foreach (var temp in userprofile.Result.Interest)
                {
                    foreach (var item in GetEventByInterest(Convert.ToInt32(temp.InterestId)).Result.Result)
                    {
                        events.Add(item);

                    }
                }
              
                List<GetEventBynearByResponse> distances = new List<GetEventBynearByResponse>();

                events.ForEach(location => {
                  var result1 =  GetEventDetail(location.EventId);

                    double distance = GetDistance(location.Latitude, location.Longitude, schoolDetails.Result.Latitude, schoolDetails.Result.Longitude);

                        distances.Add(new GetEventBynearByResponse
                        {EventId= location.EventId,
                        Userid = location.Userid,
                        Name = location.Name,
                        Description =location.Description,
                        PublicEvent = location.PublicEvent,
                        EventTime = location.EventTime,
                        Capacity = location.Capacity,
                        StreetAddress1 = location.StreetAddress1,
                        StreetAddress2=location.StreetAddress2,
                        City = location.City,
                        State = location.State,
                        Country = location.Country,
                        Zipcode = location.Zipcode,
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        distance = distance,
                        totaluser = result1.Result.Result.totaluser,
                        InterestId = location.InterestId
                        });
                });

                distances = distances.OrderBy(n => n.distance).ToList();

                if (response != null)
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = Messages.Success;
                    response.Result = distances;
                    response.TotalRecord = distances.Count();

                }
                else
                {
                    response.Status = ResponseStatus.Fail;
                    response.Message = Messages.RecordNotFound;
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

        #region GetTotalParticipateEventByUser
        public async Task<BaseResponse<GetUserEvent>> GetTotalParticipateEventByUser(int id)
        {
            BaseResponse<GetUserEvent> response = new BaseResponse<GetUserEvent>();
            try
            {

                List<DbParameter> parameters = new List<DbParameter>();
                List<EventListByUser> eventResponse = new List<EventListByUser>();

                parameters.Add(new NpgsqlParameter { ParameterName = "p_id", NpgsqlValue = id, NpgsqlDbType = NpgsqlDbType.Integer });
                var result = await _DBRepo.ExecuteListAsync<EventListByUser>(DBFunctions.GetTotalParticipateEventByUser, parameters);

          

                foreach (var temp in result)
                {

                   // var item = (GetEventById(Convert.ToInt32(temp.EventId)).Result.Result);
                    EventListByUser eventListByUser = new EventListByUser
                    { 
                        EventId = temp.EventId,
                        Userid = temp.Userid,
                        InterestId= temp.InterestId,
                        Name = temp.Name,
                        Description = temp.Description,
                        Capacity= temp.Capacity,
                        EventTime= temp.EventTime,
                        PublicEvent= temp.PublicEvent,
                        Status= temp.Status

                    };
                    eventResponse.Add(eventListByUser);

                }

                GetUserEvent eventTotalResponse = new GetUserEvent
                {
                    Userid = id,
                    Events = eventResponse
                };
                   
                if (response != null)
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = Messages.Success;
                    response.Result = eventTotalResponse;
                   // response.TotalRecord = eventResponse.Count();
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

        #region JoinEvent
        public async Task<BaseResponse<int>> JoinEvent(UserEventRequest userEventRequest)
        {
            BaseResponse<int> response = new BaseResponse<int>();
            try
            {
                var checkstatus = GetEventById(userEventRequest.EventId);
                List<DbParameter> parameters = new List<DbParameter>();

                if (checkstatus.Result.Result.PublicEvent)
                {
                    userEventRequest.Status = true;
                    
                }
                else
                {
                    userEventRequest.Status = false;

                }
                var result = await AddUserEvent(userEventRequest);
                int Id = Convert.ToInt32(result.Result);



                if (Id != -1)
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = Messages.Success;
                  //  response.Result = result;
                }
                else
                {
                    response.Status = ResponseStatus.Fail;
                    response.Message = Messages.RecordNotFound;
                }

            }

            catch (Exception)
            {
                response.Status = ResponseStatus.BadRequest;
                response.Message = Messages.BadRequest;
            }
            return response;

        }
        #endregion

        #region ApproveEvent
        public async Task<BaseResponse<int>> ApproveEvent(UserEventRequest userEventRequest)
        {
            BaseResponse<int> response = new BaseResponse<int>();
            try
            {
                
                List<DbParameter> parameters = SqlParameterHelper.GetSqlParameters<UserEventRequest>(userEventRequest);


                var result = await _DBRepo.ExecuteFirstOrDefaultAsync<int>(DBFunctions.ApproveEvent, parameters);

                if (result != 0)
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

            catch (Exception )
            {
                response.Status = ResponseStatus.BadRequest;
                response.Message = Messages.BadRequest;
            }
            return response;

        }
        #endregion

        #region GetUserEventById
        public async Task<BaseResponse<UserEvent>> GetUserEventById(int id)
        {
            BaseResponse<UserEvent> response = new BaseResponse<UserEvent>();
            try
            {
                List<DbParameter> parameters = new List<DbParameter>();

                parameters.Add(new NpgsqlParameter { ParameterName = "p_id", NpgsqlValue = id, NpgsqlDbType = NpgsqlDbType.Integer });


                //List<DbParameter> parameters = SqlParameterHelper.GetSqlParameters<int>(id);


                UserEvent result = await _DBRepo.ExecuteFirstOrDefaultAsync<UserEvent>(DBFunctions.GetJoinEventById, parameters);

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

        #region AddUserEvent
        public async Task<BaseResponse<int>> AddUserEvent(UserEventRequest userEventRequest)
        {
            BaseResponse<int> response = new BaseResponse<int>();
            try
            {
                List<DbParameter> parameters = new List<DbParameter>();

                parameters.Add(new NpgsqlParameter { ParameterName = "p_usereventid", NpgsqlValue = 0, NpgsqlDbType = NpgsqlDbType.Integer });
                parameters.Add(new NpgsqlParameter { ParameterName = "p_userid", NpgsqlValue = userEventRequest.Userid, NpgsqlDbType = NpgsqlDbType.Integer });
                parameters.Add(new NpgsqlParameter { ParameterName = "p_eventid", NpgsqlValue = userEventRequest.EventId, NpgsqlDbType = NpgsqlDbType.Integer });
                parameters.Add(new NpgsqlParameter { ParameterName = "p_status", NpgsqlValue = userEventRequest.Status, NpgsqlDbType = NpgsqlDbType.Boolean });


               // List<DbParameter> parameters = SqlParameterHelper.GetSqlParameters<UserEvent>(userEvent);
                var result = await _DBRepo.ExecuteScalarAsync(DBFunctions.JoinEvent, parameters);
                
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
                        response.Result = Id;
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

        #region GetMyEventByid
        public async Task<BaseResponse<EventResponse>> GetMyEventByid(int id)
        {
            BaseResponse<EventResponse> response = new BaseResponse<EventResponse>();
            try
            {
                List<DbParameter> parameters = new List<DbParameter>();

                parameters.Add(new NpgsqlParameter { ParameterName = "p_userid", NpgsqlValue = id, NpgsqlDbType = NpgsqlDbType.Integer });

                var result = await _DBRepo.ExecuteListAsync<MyeventList>(DBFunctions.GetMyEvent, parameters);

                EventResponse eventResponse = new EventResponse
                {
                    Userid = id,
                    Events = result
                };

                if (eventResponse.Events != null)
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = Messages.Success;
                    response.Result =eventResponse;
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

        #region GetEventUserRequest
        public async Task<BaseResponse<EventUserRequestList>> GetEventUserRequest(int id)
        {
            BaseResponse<EventUserRequestList> response = new BaseResponse<EventUserRequestList>();
            try
            {
                List<DbParameter> parameters = new List<DbParameter>();
                List<EventUserRequest> eventUserRequest = new List<EventUserRequest>();
                EventUserRequestList eventResponse = new EventUserRequestList();

                parameters.Add(new NpgsqlParameter { ParameterName = "p_eventid", NpgsqlValue = id, NpgsqlDbType = NpgsqlDbType.Integer });
                var eventdetails = GetEventById(id);
                var userdetails = _userRepository.GetUserById(eventdetails.Result.Result.Userid);
                var result = await _DBRepo.ExecuteListAsync<EventUserDetail>(DBFunctions.EventUserRequestList, parameters);

                foreach (var temp in result)
                {
                    eventUserRequest.Add(new EventUserRequest{
                        JoinUser=temp.JoinUser,
                        UserName=temp.UserName,
                        ProfileImg= temp.ProfileImg,
                        Status=temp.Status
                });
                }

              /*  if(eventUserRequest.Count() > 0)
                {*/
                    eventResponse = new EventUserRequestList
                    {
                        EventId = id,
                        Userid = eventdetails.Result.Result.Userid,
                        EventCreatedName = userdetails.Result.Result.Firstname+ userdetails.Result.Result.Middlename+ userdetails.Result.Result.Lastname,
                        CreatorProfile = userdetails.Result.Result.ProfileImg,
                        Users = eventUserRequest
                    };

               /* }
                else
                {
                    eventResponse = new EventUserRequestList
                    {
                        EventId = id,
                        Userid = eventdetails.Result.Result.Userid,
                        EventCreatedName = userdetails.Result.Result.Firstname + userdetails.Result.Result.Middlename + userdetails.Result.Result.Lastname,
                        CreatorProfile = userdetails.Result.Result.ProfileImg,
                        Users = eventUserRequest
                    };
                }*/

                if (result.Count() > 0 )
                {
                   
                    response.Status = ResponseStatus.Success;
                    response.Message = Messages.Success;
                    response.Result = eventResponse;
                }
                else
                {
                    response.Status = ResponseStatus.NotFound;
                    response.Message = Messages.RecordNotFound;
                    response.Result = eventResponse;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;

        }
        #endregion

        #region GetUserEventStatus
        public async Task<BaseResponse<UserEventStatusResponse>> GetUserEventStatus(UserEventStatusRequest userEventStatus)
        {
            BaseResponse<UserEventStatusResponse> response = new BaseResponse<UserEventStatusResponse>();
            try
            {
                List<DbParameter> parameters = new List<DbParameter>();

                parameters.Add(new NpgsqlParameter { ParameterName = "p_userid", NpgsqlValue = userEventStatus.Userid, NpgsqlDbType = NpgsqlDbType.Integer });
                parameters.Add(new NpgsqlParameter { ParameterName = "p_eventid", NpgsqlValue = userEventStatus.EventId, NpgsqlDbType = NpgsqlDbType.Integer });

                UserEventStatusResponse result = await _DBRepo.ExecuteFirstOrDefaultAsync<UserEventStatusResponse>(DBFunctions.GetUserEventStatus, parameters);

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

            catch (Exception )
            {
                response.Status = ResponseStatus.BadRequest;
                response.Message = Messages.BadRequest;
            }
            return response;

        }
        #endregion

        #region AddUserEventChat
        public async Task<BaseResponse<int>> AddUserEventChat(UserEventChatRequest userEventChatRequest)
        {
            BaseResponse<int> response = new BaseResponse<int>();
            try
            {
                List<DbParameter> parameters = new List<DbParameter>();
               // List<DbParameter> parameters = SqlParameterHelper.GetSqlParameters<UserEventChatRequest>(userEventChatRequest);
                parameters.Add(new NpgsqlParameter { ParameterName = "p_userid", NpgsqlValue = userEventChatRequest.Userid, NpgsqlDbType = NpgsqlDbType.Integer });
                parameters.Add(new NpgsqlParameter { ParameterName = "p_eventid", NpgsqlValue = userEventChatRequest.EventId, NpgsqlDbType = NpgsqlDbType.Integer });

                UserEventStatusResponse result = await _DBRepo.ExecuteFirstOrDefaultAsync<UserEventStatusResponse>(DBFunctions.GetUserEventStatus, parameters);
                
                if (result.Status == true)
                {
                    parameters.Add(new NpgsqlParameter { ParameterName = "p_message", NpgsqlValue = userEventChatRequest.Message, NpgsqlDbType = NpgsqlDbType.Text });
                    
                    var result1 = await _DBRepo.ExecuteScalarAsync(DBFunctions.AddUserEventChat, parameters);
                    int Id = Convert.ToInt32(result1);

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
                else
                {
                    response.Status = ResponseStatus.Fail;
                    response.Message = "Request Yet to be Approved From Event Creator";
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

        #region GetGroupChatByEvent
        public async Task<ListResponse<UserEventChatResponse>> GetGroupChatByEvent(int id)
        {
            ListResponse<UserEventChatResponse> response = new ListResponse<UserEventChatResponse>();
            try
            {
                List<DbParameter> parameters = new List<DbParameter>();

                parameters.Add(new NpgsqlParameter { ParameterName = "p_id", NpgsqlValue = id, NpgsqlDbType = NpgsqlDbType.Integer });


                //List<DbParameter> parameters = SqlParameterHelper.GetSqlParameters<int>(id);


                var result = await _DBRepo.ExecuteListAsync<UserEventChatResponse>(DBFunctions.GetUserEventChatByEvent, parameters);

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
            catch (Exception )
            {
                response.Status = ResponseStatus.Fail;
                response.Message = Messages.RecordNotFound;
            }

            return response;

        }
        #endregion

        #region GetEventDetail
        public async Task<BaseResponse<EventDetail>> GetEventDetail(int id)
        {
            BaseResponse<EventDetail> response = new BaseResponse<EventDetail>();

            try
            {
                EventDetail eventsdetails = new EventDetail();

                List<DbParameter> parameters = new List<DbParameter>();
                parameters.Add(new NpgsqlParameter { ParameterName = "p_id", NpgsqlValue = id, NpgsqlDbType = NpgsqlDbType.Integer });

               var eventdetail = await GetEventById(id);
                var creatorDetails = await _userRepository.GetUserById(eventdetail.Result.Userid);
               var userdetail = await _DBRepo.ExecuteListAsync<UserDetail>(DBFunctions.EventDetail, parameters);
                List<Userdata> userdata = new List<Userdata>();

                foreach (var temp in userdetail)
                {
                    if (creatorDetails.Result.Userid != temp.ParticipatedId)
                    {
                        var user = new Userdata() { ParticipatedId = temp.ParticipatedId, ParticipateName = temp.ParticipateName, ProfileImg = temp.ProfileImg };
                        userdata.Add(user);
                    }
                   /* eventsdetails = new EventDetail()
                    { Userid= temp.Userid,Name = temp.Name,CreatorImg=temp.CreatorImg, Description = temp.Description,PublicEvent=temp.PublicEvent, EventTime = temp.EventTime, users = userdata,EventCreatedName=temp.EventCreatedName,totaluser = userdata.Count()};*/
                }
                eventsdetails = new EventDetail()
                { Userid = eventdetail.Result.Userid, CreatorImg = creatorDetails.Result.ProfileImg,Name = eventdetail.Result.Name, Description = eventdetail.Result.Description, EventTime = eventdetail.Result.EventTime, EventCreatedName = creatorDetails.Result.Firstname + " "+ creatorDetails.Result.Lastname, PublicEvent = eventdetail.Result.PublicEvent, users = userdata, totaluser = userdata.Count() };

                if (response != null)
                 {
                     response.Status = ResponseStatus.Success;
                     response.Message = Messages.Success;
                     response.Result = eventsdetails;
                    

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

        #region GetDistance
        public static double GetDistance(double latitude, double longitude, double otherLatitude, double otherLongitude)
{
    var d1 = latitude * (Math.PI / 180.0);
    var num1 = longitude * (Math.PI / 180.0);
    var d2 = otherLatitude * (Math.PI / 180.0);
    var num2 = otherLongitude * (Math.PI / 180.0) - num1;
    var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

    return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
}
        #endregion

    }
}

