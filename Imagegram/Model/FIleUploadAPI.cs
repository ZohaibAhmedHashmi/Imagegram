using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Imagegram.Model
{
    public class FIleUploadAPI
    {
        public IFormFile files
        {
            get;
            set;
        }
    }
}
