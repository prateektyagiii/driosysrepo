using OutReach.CoreBusiness.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OutReach.CoreBusiness.Model.Response
{
    public class UserProfileResponse
    {
        public User UserProfile { get; set; }
        public string Schoolname { get; set; }
        public List<Tags> Interest { get; set; }

    }

    public class UserTotal : User
        {
        public int Total { get; set; }
    }

   
}
