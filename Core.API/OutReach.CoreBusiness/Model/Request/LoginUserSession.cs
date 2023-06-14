using System;

namespace OutReach.CoreBusiness.Models.Request
{
    public class LoginUserSession
    {
        public string SessionId { get; set; }
        public int UserCode { get; set; }
        public string UserEmail { get; set; }
        public string OneTimeToken { get; set; }
        public DateTime ValidTill { get; set; }
    }
}