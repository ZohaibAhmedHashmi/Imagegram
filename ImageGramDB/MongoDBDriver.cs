using ImageGramDB.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageGramDB
{
    public class MongoDBDriver 
    {
        IMongoDatabase database;
        string ImageTable = "ImageRecord";
        string CommentTable = "CommentRecord";
        public MongoDBDriver(string ConnectionString)
        {
            var settings = MongoClientSettings.FromConnectionString(ConnectionString);
            var client = new MongoClient(settings);
            database = client.GetDatabase("ImageGram");

        }
         
        public int InsertImageRecord(ImageModel record)
        {

            var collection = database.GetCollection<ImageModel>(ImageTable);

            var sort = Builders<ImageModel>.Sort.Descending("ImageID");
            var filter = Builders<ImageModel>.Filter.Where(x => x.ID != null);
            var result = collection.Find(filter).Sort(sort).Limit(1);

            ImageModel im = (ImageModel)result.FirstOrDefault();

            record.ImageID = im != null ? 1 + im.ImageID : 0;
            DBInsertImageRecord(record);

            return record.ImageID;


        }

        public int InsertCommentRecord(CommentModel record)
        {

            var collection = database.GetCollection<CommentModel>(CommentTable);

            var sort = Builders<CommentModel>.Sort.Descending("CommentID");
            var filter = Builders<CommentModel>.Filter.Where(x => x.ImageID == record.ImageID);
            var result = collection.Find(filter).Sort(sort).Limit(1);

            CommentModel im = (CommentModel)result.FirstOrDefault();

            record.CommentID = im != null ? 1 + im.CommentID : 0;

            DBInsertCommentRecord(record);
            return record.CommentID;

        }

        public void DeleteCommentRecord(CommentModel record)
        {

            var collection = database.GetCollection<CommentModel>(CommentTable);

            var filter = Builders<CommentModel>.Filter.Where(x => x.ImageID == record.ImageID && x.CommentID == record.CommentID);
            var result = collection.DeleteOne(filter);



        }

        public List<CommentModel> GetCommentList()
        {

            List<CommentModel> ResultList = new List<CommentModel>();
            var collection = database.GetCollection<CommentModel>(CommentTable);

            var result = collection.AsQueryable();

            foreach (var row in result)
            {
                ResultList.Add((CommentModel)row);
            }

            return ResultList;
        }

        public List<ImageModel> GetImageList()
        {

            List<ImageModel> ResultList = new List<ImageModel>();
            var collection = database.GetCollection<ImageModel>(ImageTable);
            var result = collection.AsQueryable();
            foreach (var row in result)
            {
                ResultList.Add((ImageModel)row);
            }

            return ResultList;
        }

        #region DB Queries


        public async void DBInsertImageRecord(ImageModel record)
        {
            var collection = database.GetCollection<ImageModel>(ImageTable);
            collection.InsertOne(record);
        }

        public async void DBInsertCommentRecord(CommentModel record)
        {
            var collection = database.GetCollection<CommentModel>(CommentTable);
            collection.InsertOne(record);
        }

        #endregion
    }
}
