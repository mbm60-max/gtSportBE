using MongoDB.Bson;
using MongoDB.Driver;

var connectionString = "mongodb+srv://maxbm:Kismetuni66@pdtoolcluster.een0p7c.mongodb.net/?retryWrites=true&w=majority";
var client = new MongoClient(connectionString);
var database = client.GetDatabase("Test");

var collectionName = "Test Collection";
database.CreateCollection(collectionName);

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
}

public class YourClassName
{
    public void InsertPersonDocument()
    {
        var collection = database.GetCollection<Person>(collectionName);

        var document = new Person
        {
            Name = "John Doe",
            Age = 30,
            Email = "johndoe@example.com"
        };

        collection.InsertOne(document);
    }
}