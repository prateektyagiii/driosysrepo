using OutReach.CoreBusiness.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OutReach.CoreBusiness.Model.Response
{
   public class EventResponse
    {
        public int Userid { get; set; }
        public List<MyeventList> Events { get; set; }
    }

    public class EventDetailResponse : Event 
    {
        public string EventCreatedName { get; set; }
        
    }


    public class MyeventList
    {
		public int EventId { get; set; }
        public int Userid { get; set; }
        public int InterestId { get; set; }
        public string Name { get; set; }
		public string Description { get; set; }
		public bool PublicEvent { get; set; }
		public int Capacity { get; set; }
        public DateTime EventTime { get; set; }
      
	}

    public class EventUserRequestList
    {
        public int Userid { get; set; }
        public int EventId { get; set; }
        public string EventCreatedName { get; set; }

        public string CreatorProfile { get; set; }
        public List<EventUserRequest> Users { get; set; }
    }

    public class EventUserRequest
    {
   
        public int JoinUser { get; set; }
        public string UserName { get; set; }
        public string ProfileImg { get; set; }
        public bool Status { get; set; }
		
    }

    public class EventUserDetail : EventUserRequest
    {
        public int Userid { get; set; }
        public int EventId { get; set; }
        public string EventCreatedName { get; set; }
        public string CreatorProfile { get; set; }
    }

    public class GetUserEvent
    {
        public int Userid { get; set; }

        public List<EventListByUser> Events { get; set; }

    }

    public class GetTotalParticipateEventByUser
    {
        public int Userid { get; set; }
        public List<EventListByUser> Events { get; set; }
    }

    public class EventListByUser : MyeventList
    {
        public bool Status { get; set; }
    }

    public class GetEventBynearByResponse : Event
    {
        public double distance { get; set; }
        public int totaluser { get; set; }
        
    }

    public class UserEventStatusResponse
    {
        public bool Status { get; set; }
        
    }
    public class UserEventChatResponse
    {
        public int Userid { get; set; }
        public string Name { get; set; }
        public string ProfileImg { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }

    }

    public class EventDetail
    {
        public int Userid { get; set; }
        public string Name { get; set; }
        public string CreatorImg { get; set; }

        public string Description { get; set; }
        public DateTime EventTime { get; set; }
        public string EventCreatedName { get; set; }
        public bool PublicEvent { get; set; }
        public int totaluser { get; set; }
        public List<Userdata> users { get; set; }
     

    }

    public class Userdata
    {
        public int ParticipatedId { get; set; }
        public string ParticipateName { get; set; }
        public string ProfileImg { get; set; }
        /*public string EventCreatedName { get; set; }*/

    }

    public class UserDetail : Event
    {
        public int ParticipatedId { get; set; }

        public string ParticipateName { get; set; }
        public string CreatorImg { get; set; }

        public string ProfileImg { get; set; }

        public string EventCreatedName { get; set; }
    }

    public class EventTotal : Event
    {
        public int Total { get; set; } 
    }

}
