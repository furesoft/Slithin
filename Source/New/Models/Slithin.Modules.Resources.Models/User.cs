using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Slithin.Modules.Resources.Models;

public sealed class User
{
    [BsonElement("hashedPassword")]
    public string HashedPassword { get; set; }

    [BsonElement("_id")]
    public ObjectId ID { get; set; }

    [BsonElement("role")]
    public string Role { get; set; }

    [BsonElement("username")]
    public string Username { get; set; }
}
