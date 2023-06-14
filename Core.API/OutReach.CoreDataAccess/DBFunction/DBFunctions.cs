namespace OutReach.CoreDataAccess.DBFunction
{
    public class DBFunctions
    {

        #region Common

        public const string GetUserAuthentication = "\"OR_APP\".fn_auth_get_user_by_email";
        public const string SaveUserAuthSession = "\"OR_APP\".fn_auth_save_user_auth_session";
        public const string UserRegistration = "\"OR_APP\".\"FN_OR_M_USER_INSERT\"";
        public const string UserProfileUpdate = "\"OR_APP\".\"FN_OR_M_USER_UPDATE\"";
        public const string UserProfileGetById = "\"OR_APP\".\"FN_OR_M_USER_SELECTBYID\"";
        public const string GetAllSchool = "\"OR_APP\".\"FN_OR_M_SCHOOL_GETALL\"";
        public const string GetAllInterest = "\"OR_APP\".\"FN_OR_M_INTEREST_GETALL\"";
        public const string UserForgotPassword = "\"OR_APP\".\"FN_OR_M_USER_FORGOTPASSWORD\"";
        public const string UserPasswordUpdate = "\"OR_APP\".\"FN_OR_M_USER_PASSWORD_UPDATE\"";
        public const string UserOTPVerify = "\"OR_APP\".\"FN_OR_M_USER_OTP_VERIFY\"";
        public const string GetInterestById = "\"OR_APP\".\"FN_OR_M_INTEREST_SELECTBYID\"";
        public const string GetSchoolById = "\"OR_APP\".\"FN_OR_M_SCHOOL_SELECTBYID\"";
        public const string GetAllEvent = "\"OR_APP\".\"FN_OR_M_EVENT_GETALL\"";
        public const string GetEventById = "\"OR_APP\".\"FN_OR_M_EVENT_SELECTBYID\"";
        public const string UserInterest = "\"OR_APP\".\"FN_OR_T_TAGS_INSERT\"";
        public const string GetInterestByUserId = "\"OR_APP\".\"FN_OR_T_TAGS_SELECTBYUSER\"";
        public const string UpdateInterest = "\"OR_APP\".\"FN_OR_T_TAGS_UPDATE\"";
        public const string DeleteInterest = "\"OR_APP\".\"FN_OR_T_TAGS_DELETE\"";
        public const string AddEvent = "\"OR_APP\".\"FN_OR_M_EVENT_INSERT\"";
        public const string GetEventByInterest = "\"OR_APP\".\"FN_OR_M_EVENT_SELECTBYINTEREST\"";
        public const string JoinEvent = "\"OR_APP\".\"FN_OR_T_USEREVENT_INSERT\"";
        public const string GetJoinEventById = "\"OR_APP\".\"FN_OR_T_USEREVENT_SELECTBYID\"";
        public const string JoinGroupChat = "\"OR_APP\".\"FN_OR_T_GROUPCHAT_INSERT\"";
        public const string EventUserRequestList = "\"OR_APP\".\"FN_OR_EVENT_REQUEST_LIST\"";
        public const string GetMyEvent = "\"OR_APP\".\"FN_OR_M_EVENT_SELECTBYUSERID\"";
        public const string GetEventByUser = "\"OR_APP\".\"FN_OR_M_EVENT_SELECTBYINTEREST\"";
        public const string GetTotalParticipateEventByUser = "\"OR_APP\".\"FN_OR_T_USEREVENT_SELECTBYUSERID\"";
        public const string GetUserEventStatus = "\"OR_APP\".\"FN_OR_USEREVENT_STATUS\"";
        public const string AddUserEventChat = "\"OR_APP\".\"FN_OR_T_GROUPCHAT_INSERT\"";
        public const string GetUserEventChatByEvent = "\"OR_APP\".\"FN_OR_T_GROUPCHAT_SELECTBYEVENTID\"";
        public const string GetAllUser= "\"OR_APP\".\"FN_OR_M_USER_GETALL\"";             
        public const string ApproveEvent = "\"OR_APP\".\"FN_OR_T_USEREVENT_UPDATEBYEVENTIDANDUSERID\"";
        public const string EventDetail = "\"OR_APP\".\"FN_OR_M_EVENT_DETAIL\"";
        public const string AddInterest = "\"OR_APP\".\"FN_OR_M_INTEREST_INSERT\"";
        public const string AddSchool = "\"OR_APP\".\"FN_OR_M_SCHOOL_INSERT\"";
        public const string UpdateSchool = "\"OR_APP\".\"FN_OR_M_SCHOOL_UPDATE\"";
        public const string EditInterest = "\"OR_APP\".\"FN_OR_M_INTEREST_UPDATE\"";
        public const string RemoveInterest = "\"OR_APP\".\"FN_OR_M_INTEREST_DELETE\"";
        public const string DeleteSchool = "\"OR_APP\".\"FN_OR_M_SCHOOL_DELETE\"";
        public const string DeleteEvent = "\"OR_APP\".\"FN_OR_M_EVENT_DELETE\"";
        public const string LeftEvent = "\"OR_APP\".\"FN_OR_USEREVENT_UPDATE_STATUS\"";


        #endregion
    }
}