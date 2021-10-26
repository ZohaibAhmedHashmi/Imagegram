using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ImageGramDB.Model
{
    public class CommentModel
    {
        [BsonId]
        public Guid CommentGUID { get; set; }
        public int CommentID { get; set; }
        public int ImageID { get; set; }
        public string Comment { get; set; }
    }

}