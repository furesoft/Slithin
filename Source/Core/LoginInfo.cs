using LiteDB;

namespace Slithin.Core
{
    public record LoginInfo(string IP, string Password, bool Remember)
    {
        public ObjectId _id { get; set; }
    }
}
