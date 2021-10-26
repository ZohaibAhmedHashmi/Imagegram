using System;
using System.Collections.Generic;
using System.Text;

namespace ImageGramDB.Model
{
    public class ImageDTO
    {
        public int ImageID { get; set; }
        public string ImageURL { get; set; }
        public List<CommentDTO> comments { get; set; }
    }

    public class CommentDTO
    {
        public int CommentID { get; set; }
        public string Comment { get; set; }
    }
}
