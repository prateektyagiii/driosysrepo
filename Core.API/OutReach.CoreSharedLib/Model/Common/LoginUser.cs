using System;

namespace OutReach.CoreSharedLib.Models.Common
{
    public class LoginUser
    {
        public int Userid { get; set; }
        public int UserCode { get; set; }
        public int IsAdmin { get; set; }
        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Middlename { get; set; }
        public string UserEmail { get; set; }

    }

    public class ResponseBase : IDisposable
    {
        public ResponseStatus Status { get; set; }
        public string Message { get; set; }
        public long UpdatedSyncTime { get; set; }
        public long ResultCount { get; set; }
        public int PageSize { get; set; }
        public int BackPageIndex { get; set; }
        public int PageIndex { get; set; }
        public int NextPageIndex { get; set; }
        public object Result { get; set; }

        public void Dispose()
        {
            System.GC.Collect();
        }
    }

    public enum ResponseStatus
    {
        NA = 0,
        Success = 200,
        Reset = 205,
        Fail = 420,
        Created = 201,
        Accepted = 202,
        Processing = 102,
        Redirect = 307,
        UnAuthorized = 401,
        Forbidden = 403,
        BadRequest = 400,
        InternalServerError = 500,
        AlreadyExists = 409, //Conflict
        InvalidToken = 498,
        LoginTimeOut = 440,
        Timeout = 524,
        NotFound = 404,
        InProgress = -3,
        UnAuthorizedToken = -10,
        UnsupportedFile = 600
    }
}