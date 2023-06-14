using OutReach.CoreBusiness.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OutReach.CoreBusiness.Model.Request
{
    public class UserRequest
    {
        public string Firstname { get; set;}
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string UserEmail { get; set; }

        public bool? IsActive { get; set; } = null;

    }

    public class AddSchoolForProfileRequest
    {
        public int Id { get; set; }

        public int SchoolId { get; set; }

    }

    public class UserInterestRequest
    {
        public int Userid { get; set; }

        public List<int> InterestId { get; set; }

    }


    public class UserProfileRequest
    {
        public User UserProfile { get; set; }

        public List<int> Interest { get; set; }

    }
}
