using System;
using System.Collections.Generic;
using System.Text;

namespace OutReach.CoreBusiness.Model.Request
{
   public class EventRequest
    {
        public string City { get; set; }
        public string Name { get; set; }
    }

    public class UserEventRequest
    {
        public int Userid { get; set; }
        public int EventId{ get; set; }
        public bool Status { get; set; }
    }

    public class UserEventStatusRequest
    {
        public int Userid { get; set; }
        public int EventId { get; set; }
    }

    public class UserEventChatRequest
    {
        public int Userid { get; set; }
        public int EventId { get; set; }
        public string Message { get; set; }
    }

}
