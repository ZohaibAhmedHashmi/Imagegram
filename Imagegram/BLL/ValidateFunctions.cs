using Imagegram.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Imagegram.BLL
{
    public static class ValidateFunctions
    {
        public static ValidResponse isValid(IFormFile file)
        {

            if (file == null || file.Length == 0)
                return new ValidResponse { Valid = false, Message = "No Valid file found" };


            string ext = Path.GetExtension(file.FileName).ToLower();

            if (!ValidFileSize(file.Length))
                return new ValidResponse { Valid = false, Message = "File Size is Greater then 100MB" };
            if(!ValidFileFormat(ext))
                return new ValidResponse { Valid = false, Message = "File format"+ ext + " is Not valid" };

            return new ValidResponse { Valid = true,Message= "File uploaded successfully(" + file.FileName+")" };
        }
        private static bool ValidFileSize(double FileSize)
        {
            Double MaxAllowedSize = 100 * 1024 * 1024;
            if (FileSize > 0 && FileSize <= MaxAllowedSize)
                return true;
            else
                return false;
        }
        private static bool ValidFileFormat(string FileFormat)
        {
            var test = ConfigurationManager.AppSettings["AllowedFormat"];

            IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".png", ".bmp" };

            if (AllowedFileExtensions.Contains(FileFormat))
                return true;
            else
                return false;
        }

    }

}

