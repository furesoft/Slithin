using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SlithinMarketplace.Models;

public sealed class User
{
    [BsonElement("id")]
    public ObjectId _id { get; set; }

    [BsonElement("hashedPassword")]
    public string HashedPassword { get; set; }

    [BsonElement("role")]
    public string Role { get; set; }

    [BsonElement("username")]
    public string Username { get; set; }
}
