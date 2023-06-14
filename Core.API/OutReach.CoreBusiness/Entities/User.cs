using OutReach.CoreSharedLib.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace OutReach.CoreBusiness.Entities
{
    public class User
    {
        public int Userid { get; set; }
        public string ProfileImg { get; set; } = null;
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string Education { get; set; }
        public string Bio { get; set; }
        public int Role { get; set; }
        public int? SchoolId { get; set; } = null;

        public int? IsAdmin { get; set; } = null;
    }
}
