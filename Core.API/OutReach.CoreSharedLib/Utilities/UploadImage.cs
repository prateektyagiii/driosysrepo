using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core;
using OutReach.CoreSharedLib.Model.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutReach.CoreSharedLib.Utilities
{
    public class UploadImage
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ProfileConfiguration _profileConfiguration;


        public UploadImage(IWebHostEnvironment environment, ProfileConfiguration profileConfiguration)
        {

            _environment = environment;
            _profileConfiguration = profileConfiguration;
    }
        #region Upload Image

        public async Task<string> SaveImage(int id, IFormFile file)
        {
            if (file.Length > 0)
            {
                try
                {

                    if (!Directory.Exists(_environment.ContentRootPath + "\\App_Media\\"))
                    {
                        Directory.CreateDirectory(_environment.ContentRootPath + "\\App_Media\\");
                    }

                    string wwwRootPath = _environment.ContentRootPath;

                    var fullpath = wwwRootPath + _profileConfiguration.FolderName;
                    string Filename = Path.GetFileNameWithoutExtension(file.FileName);
                    string extension = Path.GetExtension(file.FileName);

                    string fileName = Filename + "_" + Guid.NewGuid().ToString() + extension;

                    //  var filename = CheckFile(fullpath,fileName);

                    using (var fileStream = new FileStream(fullpath + fileName, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                        return _profileConfiguration.UserImageUrl + _profileConfiguration.FolderName + fileName;

                    }

                }
                catch (Exception ex)
                {
                    return  ex.ToString();
                }
            }
            else
            {
                    return "Unsuccessful" ;
                }
            }

        #endregion


        public string CheckFile(string fullpath,string filename)
        {
            /* Delete similar file*/
            string[] similarFiles =
            Directory.GetFiles(fullpath, filename + ".*")
            .Except(new[] { filename }, StringComparer.OrdinalIgnoreCase).ToArray();

            //Delete these files
            foreach (var similarFile in similarFiles)
                File.Delete(similarFile);

            return filename;
        }
    }
}


