using OutReach.CoreBusiness.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OutReach.CoreBusiness.Model.Response
{
    public class SchoolResponse : School
    {
        public int Total {get; set;}
    }
}
