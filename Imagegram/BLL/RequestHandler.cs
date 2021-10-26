using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks; 
using System.Drawing;
using System.Drawing.Imaging;
using ImageGramDB;
using Microsoft.Extensions.Configuration;
using static ImageGramDB.MongoDBDriver;
using ImageGramDB.Model;

namespace Imagegram.BLL
{
    public class RequestHandler
    {
        MongoDBDriver _mDB; 
        public RequestHandler(MongoDBDriver dbdriver)
        {
            _mDB = dbdriver;
        }

     

        public async Task<int> uploadFile(IWebHostEnvironment _environment, IFormFile file)
        {
            string filepath;

            using (FileStream filestream = System.IO.File.Create(_environment.ContentRootPath + "\\uploads\\" + file.FileName))
            {

                if (!Directory.Exists(_environment.ContentRootPath + "\\uploads\\"))
                {
                    Directory.CreateDirectory(_environment.ContentRootPath + "\\uploads\\");
                }

                file.CopyTo(filestream);

                filepath = filestream.Name;


                string ext = Path.GetExtension(file.FileName).ToLower();
                if (ext != ".jpg")
                    filepath = await convertFileToJPG(filestream);

                filestream.Flush();

                return _mDB.InsertImageRecord( new ImageModel { ImageURL = filepath });

            }
        }
        public async Task<string> convertFileToJPG(FileStream filestream)
        {
            string name = Path.GetFileNameWithoutExtension(filestream.Name);
            string path = Path.GetDirectoryName(filestream.Name);

            Image file = Image.FromStream(filestream);

            string filesavepath = path + @"/" + name + ".jpg";
            file.Save(filesavepath, ImageFormat.Jpeg);
            file.Dispose();

            return filesavepath;

        }

        internal async Task<List<ImageDTO>> GetAllList()
        {
            List<ImageModel> ListImageData = _mDB.GetImageList();
            List<CommentModel> ListCommentData = _mDB.GetCommentList();



            List<ImageDTO> ImageCommentList = (from I in ListImageData
                                               select new ImageDTO
                                               {
                                                   ImageID = I.ImageID,
                                                   ImageURL = I.ImageURL,
                                                   comments = (from c in ListCommentData
                                                               where c.ImageID == I.ImageID
                                                               orderby c.CommentID descending
                                                               select (new CommentDTO()
                                                               {
                                                                   Comment = c.Comment,
                                                                   CommentID = c.CommentID
                                                               })).ToList() 

                                               }).OrderByDescending(x=> x.comments.Count).ToList();
             

            return ImageCommentList;
        }

        #region Comment Functionality
        internal async void DeleletCommentFromImage(int imgID, int commentID)
        {
            _mDB.DeleteCommentRecord( new CommentModel() {ImageID = imgID, CommentID = commentID } );
        }
        internal async Task<int> PostCommentOnImage(int imgID, string comment)
        {

            return _mDB.InsertCommentRecord(new CommentModel() { ImageID = imgID, Comment = comment });
        }
        #endregion

    }
}
