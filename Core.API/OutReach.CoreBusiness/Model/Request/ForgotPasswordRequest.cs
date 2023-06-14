using System;
using System.Collections.Generic;
using System.Text;

namespace OutReach.CoreBusiness.Model.Request
{
    public class ForgotPasswordRequest
    {
        public string Useremail { get; set; }
   
    }

    public class OtpVerificationRequest
    {
        public int otp { get; set; }

        public string Useremail { get; set; }

    }

    public class ResetPasswordRequest
    {
        public string useremail { get; set; }
        public string userpassword { get; set; }
    }
    }
