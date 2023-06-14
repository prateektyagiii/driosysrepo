using System;
using System.Collections.Generic;
using System.Text;

namespace OutReach.CoreBusiness.Entities
{
    public class UserEvent
    {

        public int UserEventId { get; set; }
        public int Userid { get; set; }
        public int EventId
        { get; set; }
        public bool Status
        { get; set; }

    }
}