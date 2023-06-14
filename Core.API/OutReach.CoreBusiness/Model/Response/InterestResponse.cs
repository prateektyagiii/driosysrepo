using OutReach.CoreBusiness.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OutReach.CoreBusiness.Model.Response
{
    public class InterestResponse : Interest
    {
        public int Total { get; set; }
    }
}
