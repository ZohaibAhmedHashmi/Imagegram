using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Imagegram.BLL;
using Imagegram.Model;

namespace ImageUploadDemo.Controllers
{
    [Route("ImageGram/[controller]")]
    public class ImageController : ControllerBase
    {

        public static IWebHostEnvironment _environment;
        public static RequestHandler _rqHandler;
        public ImageController(IWebHostEnvironment environment, RequestHandler rqHandler)
        {
            _environment = environment;
            _rqHandler = rqHandler;
        }
        [HttpGet]
        public async Task<IActionResult> Get(FIleUploadAPI objfile)
        {

            return Ok(_rqHandler.GetAllList());
        }
        
        [HttpPost]
        public async Task<IActionResult> Post(FIleUploadAPI objfile)
        {

            //Is valid function will run all the validation against the file
            ValidResponse isValid = ValidateFunctions.isValid(objfile.files);
             

            if (isValid.Valid)
            {
                try
                {
                    // Method Will Upload the File
                    int ImgID = await _rqHandler.uploadFile(_environment, objfile.files);


                    return Ok("Status : Success! \nImageID:"+ImgID+"\nMessage:" + isValid.Message);
                    
                }
                catch (Exception ex)
                {
                    return Problem(ex.ToString());
                }
                finally
                {
                    // For garbage collector
                    isValid = null;
                    _rqHandler = null;
                }
            }
            else
            {
                return BadRequest("Error! "+isValid.Message);
            }

        }

    }
}