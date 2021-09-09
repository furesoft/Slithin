using LiteDB;
using Slithin.Core;

namespace Slithin.Models
{
    public record LoginInfo(string IP, string Password, string Name)
    {
        public ObjectId _id { get; set; }
    }
}
