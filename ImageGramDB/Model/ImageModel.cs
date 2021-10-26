using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGramDB.Model
{
    public class ImageModel
    {
        [BsonId]
        public Guid ID { get; set; }
        public int ImageID { get; set; }
        public string ImageURL { get; set; }
    }
}
