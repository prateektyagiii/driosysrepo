using OutReach.CoreSharedLib.Models.Common;
using System;
using System.Collections.Generic;

namespace OutReach.CoreBusiness.Entity
{
    public class BaseResponse<T>
    {
        public ResponseStatus Status { get; set; } = ResponseStatus.NA;
        public string Message { get; set; }
        public T Result { get; set; }
    }
    public class BaseResponse<T, R>
    {
        public ResponseStatus Status { get; set; } = ResponseStatus.NA;
        public string Message { get; set; }
        public R Result { get; set; }
        public T Data { get; set; }

    }

    public class ListResponse<T>
    {
        public ResponseStatus Status { get; set; } = ResponseStatus.NA;
        public string Message { get; set; }
        public List<T> Result { get; set; }
        public int TotalRecord { get; set; }
    }

    public class ListResponse<T, R>
    {
        public ResponseStatus Status { get; set; } = ResponseStatus.NA;
        public string Message { get; set; }
        public List<T> Result { get; set; }
        public R Data { get; set; }
        public int TotalRecord { get; set; }
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

    public class RequestBase
    {
        public long RequestSyncTime { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string AppVersion { get; set; }
        public int ConfigVersion { get; set; }
        public string UserEmail { get; set; }
        public DateTime ReleaseDate { get; set; }
        public double UTCOffsetSeconds { get; set; }
        public int DivisionCode { get; set; } = 0;
        public string DivisionName { get; set; }

    }

}
