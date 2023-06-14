using System;
using System.Collections.Generic;
using System.Text;

namespace OutReach.CoreSharedLib.Model.Common
{
    public interface IBaseEntity
    {
        //  string _id { get; set; }
        string CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        string UpdatedBy { get; set; }
        DateTime? UpdatedDate { get; set; }
        bool IsActive {get;set;}
        bool IsDeleted { get; set; }
    }

    public class BaseEntity : IBaseEntity
    {
      //  public string _id { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.MinValue;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedDate { get; set; } = DateTime.MinValue;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }

    public class BaseEntityDomain
    {
      //  public string _id { get; set; }
    }
}
