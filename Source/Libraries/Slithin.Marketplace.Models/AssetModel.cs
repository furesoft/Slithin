using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SlithinMarketplace.Models;

public class AssetModel
{
    [BsonElement("creatorID")]
    public ObjectId CreatorID { get; set; }

    [BsonElement("fileID")]
    public string FileID { get; set; }

    [BsonElement("_id")]
    public ObjectId ID { get; set; }

    [BsonElement("uploadTime")]
    public DateTime UploadTime { get; set; }
}
