using OutReach.CoreSharedLib.Models.Common;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace OutReach.CoreSharedLib.Utilities
{
    public class Utility
    {
        public static string RandomInt(int length)
        {
            const string chars = "0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string BadRequest()
        {
            ResponseBase objResponseBaseClass = new ResponseBase();
            objResponseBaseClass.Status = ResponseStatus.BadRequest;
            objResponseBaseClass.Message = "Bad Request.";
            string value = JsonConvert.SerializeObject(objResponseBaseClass);
            return value;
        }
    }
}