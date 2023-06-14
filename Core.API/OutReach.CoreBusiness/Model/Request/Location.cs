using System;
using System.Collections.Generic;
using System.Text;

namespace OutReach.CoreBusiness.Model.Request
{
    public class Location
    {
        public string Country { get; set; }
        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        
    }

    /*public class Location
    {
        public string Country { get; set; }
        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Location(string Country_, string City_, double Latitude_, double Longitude_)
        {
            this.Country = Country_;
            this.City = City_;
            this.Latitude = Latitude_;
            this.Longitude = Longitude_;
        }*/
    }
