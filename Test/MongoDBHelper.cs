using MongoDB.Bson;
using MongoDB.Driver;
using FullSimulatorPacket;
using PacketArrayHelpers;
namespace  MongoHelpers{
internal class MongoDBHelper
{
 
    private  IMongoDatabase _database;
    private void InitializeMongoDB()
    {
        var connectionString = "mongodb+srv://MaxByng-Maddick:Kismetuni66@cluster0.a31ajbo.mongodb.net/?retryWrites=true&w=majority";
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase("UserSessions");
    }

    public void InsertExtendedPacket(string username, ExtendedPacketArrays extendedPacketArrays)
        {
            InitializeMongoDB();

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

        private bool CollectionExists(string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var collections = _database.ListCollections(new ListCollectionsOptions { Filter = filter });

            return collections.Any();
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
