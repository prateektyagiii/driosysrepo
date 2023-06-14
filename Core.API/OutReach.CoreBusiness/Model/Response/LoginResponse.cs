using OutReach.CoreBusiness.Entities;
using OutReach.CoreBusiness.Entity;

namespace OutReach.CoreBusiness.Models.Response
{
    public class LoginResponse
    {
        public string access_token { get; set; }
        public string userid { get; set; }
        public int UserCode { get; set; }
        public string token_type { get; set; }
        public string issued_at { get; set; }
        public string expire_at { get; set; }
        public string expires_in { get; set; }
        public string client_access_token { get; set; }
        public string client_referesh_token { get; set; }
    }
    
    public class RegisterResponse
    {
        public int userid { get; set; }
        public string email { get; set; }
        public string access_token { get; set; }

    }
}