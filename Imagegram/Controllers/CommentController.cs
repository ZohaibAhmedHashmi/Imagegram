using Imagegram.BLL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Imagegram.Controllers
{
    [Route("ImageGram/[controller]")]
    public class CommentController : Controller
    {
        RequestHandler _rq;
        public CommentController(RequestHandler requestHandler)
        {
            _rq = requestHandler;
        }
        [HttpPost]
        public async Task<IActionResult> Post(int? ImageID, string Comment)
        {
            if (ImageID == null || Comment == null)
                return BadRequest("ImageID or Comment value not valid");
            try { 
            return Ok("Success\nCommentID:" + await _rq.PostCommentOnImage(Convert.ToInt32(ImageID), Comment) + "\nImageID:"+ Convert.ToInt32(ImageID));
            }
            catch
            {
                return Problem("Some Error Occured.");
            }

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? ImageID, int? CommentID)
        {
            if (ImageID == null || CommentID == null)
                return BadRequest("ImageID or CommentID value not valid");
            try
            {
                _rq.DeleletCommentFromImage(Convert.ToInt32(ImageID), Convert.ToInt32(CommentID));
                return Ok("Success\nDeleted CommentID:" + CommentID);
            }
            catch
            {
                return Problem("Some Error Occured.");
            }

        }
    }
}
