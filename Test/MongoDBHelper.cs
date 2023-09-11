using MongoDB.Bson;
using System;
using MongoDB.Driver;
using FullSimulatorPacket;
using PacketArrayHelpers;
using YouTubeHelpers;
namespace  MongoHelpers{
internal class MongoDBHelper
{
    private YouTubeHelper youTubeHelper = new YouTubeHelper();
    private  IMongoDatabase _database;
    private void InitializeMongoDB(string databaseName)
    {
        var connectionString = "mongodb+srv://MaxByng-Maddick:Kismetuni66@cluster0.a31ajbo.mongodb.net/?retryWrites=true&w=majority";
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public void InsertExtendedPacket(string username, ExtendedPacketArrays extendedPacketArrays,string databaseName)
        {
            InitializeMongoDB(databaseName);

            // Check if the collection exists
            var collectionExists = CollectionExists(username);

            var collection = _database.GetCollection<BsonDocument>(username);
            
            var sessionCount = collection.CountDocuments(new BsonDocument());
            Console.WriteLine(sessionCount);
            if (sessionCount >= 10)
            {
                var firstDocument = collection.Find(new BsonDocument()).FirstOrDefault();
                if (firstDocument != null)
                {
                    collection.DeleteOne(firstDocument);
                }
            }

            var sessionDocument = new BsonDocument
            {
                { "Username", username },
            };

            var extendedPacketType = typeof(ExtendedPacketArrays);
            var properties = extendedPacketType.GetProperties();
            foreach (var property in properties)
            {
                var propertyName = property.Name; 
                Console.WriteLine(propertyName);
                var propertyValue = property.GetValue(extendedPacketArrays);
                if (propertyValue is Array arrayValue)
        {
             var arrayString = GetArrayStringRepresentation(arrayValue);
              Console.WriteLine(arrayString);
            sessionDocument.Add(propertyName, arrayString);
        }
        else
        {
            Console.WriteLine(propertyValue);
            sessionDocument.Add(propertyName, propertyValue.ToString());
        }
            }

            if (!collectionExists)
            {
                _database.CreateCollection(username);
            }

            collection.InsertOne(sessionDocument);
        }

        public void InsertYTVideos(string collectionName, List<YouTubeHelpers.YouTubeHelper.VideoData> videoData,string databaseName,string category)
        {


            InitializeMongoDB(databaseName);

    // Check if the collection exists
    var collectionExists = CollectionExists(collectionName);

    var collection = _database.GetCollection<BsonDocument>(collectionName);

            var videoArray = new BsonArray();

    foreach (var data in videoData)
    {
        var videoDocument = new BsonDocument
        {
            { "VideoId", data.VideoId },
            { "Title", data.Title },
            { "Creator", data.Creator },
            { "Duration", data.Duration }
        };

        videoArray.Add(videoDocument);
    }

    var sessionDocument = new BsonDocument
    {
        { "Videos", category },
        { "VideoData", videoArray }
    };

    if (!collectionExists)
    {
        _database.CreateCollection(collectionName);
    }

    collection.InsertOne(sessionDocument);
        }




            public string GetLastUpdatedDate(string databaseName)
    {
        InitializeMongoDB(databaseName);
        var collectionName = "LastUpdatedVideo"; // Collection name
        var collection = _database.GetCollection<BsonDocument>(collectionName);


        // Find the single document in the collection
        var document = collection.Find(FilterDefinition<BsonDocument>.Empty).SingleOrDefault();
        if (document != null && document.Contains("lastUpdatedDate"))
        {
            // Retrieve the value of the "lastUpdatedDate" attribute as a string
            var lastUpdatedDate = document["lastUpdatedDate"].AsString;
            return lastUpdatedDate;
        }
        else
        {
            // Handle the case where the document or the attribute does not exist
            return "No Date";
        }
    }
        private bool CollectionExists(string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var collections = _database.ListCollections(new ListCollectionsOptions { Filter = filter });

            return collections.Any();
        }

        public void UpdateTarget(string databaseName, string collectionName,string newData, string targetAttribute)
    {
        InitializeMongoDB(databaseName);
        var collection = _database.GetCollection<BsonDocument>(collectionName);

        // Create a filter to find the document
        var filter = Builders<BsonDocument>.Filter.Empty;

        // Create an update definition to set the "lastUpdatedDate" attribute to the new value
        var update = Builders<BsonDocument>.Update.Set(targetAttribute, newData);

        // Update the document without retrieving it
        collection.UpdateOne(filter, update, new UpdateOptions { IsUpsert = true });
    }

       private string GetArrayStringRepresentation(Array array)
        {
            var elements = new List<string>();

            foreach (var item in array)
            {
                if (item is Array subArray)
                {
                    var subArrayElements = new List<string>();

                    foreach (var secondaryItem in subArray)
                    {
                        subArrayElements.Add(secondaryItem.ToString());
                    }

                    elements.Add($"[{string.Join(", ", subArrayElements)}]");
                }
                else
                {
                    elements.Add(item.ToString());
                }
            }

            return $"[{string.Join(", ", elements)}]";
        }

        
    }


};
