using LiteDB;

namespace Slithin.Models
{
    public record LoginInfo(string IP, string Password)
    {
        public ObjectId _id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
