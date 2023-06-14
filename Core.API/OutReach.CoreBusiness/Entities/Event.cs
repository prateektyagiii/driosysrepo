using OutReach.CoreSharedLib.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace OutReach.CoreBusiness.Entities
{
    public class Event
	{
		public int EventId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Userid { get; set; }
		public int InterestId { get; set; }
		public bool PublicEvent { get; set; }
		public int Capacity { get; set; }
        public DateTime EventTime { get; set; }
        public string StreetAddress1 { get; set; }
		public string StreetAddress2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zipcode { get; set; }
		public string Country { get; set; }
		public Double Latitude { get; set; }
		public Double Longitude { get; set; }

		

	}
}
