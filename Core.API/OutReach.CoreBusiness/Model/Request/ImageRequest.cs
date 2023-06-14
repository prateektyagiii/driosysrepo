using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace OutReach.CoreBusiness.Model.Request
{
    public class ImageRequestObject
    {
        public string ImageBase64 { get; set; }
        public string ImageName { get; set; }

    }


    public class SaveFileRequest
    {
      //  public int Userid { get; set; }
        public IFormFile ProfileImg { get; set; }


    }
  
}
