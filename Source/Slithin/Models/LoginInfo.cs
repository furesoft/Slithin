using LiteDB;

namespace Slithin.Models
{
    public record LoginInfo
    {
        public ObjectId _id { get; set; }

        public string Name { get; set; }
        public string Password { get; set; }

        public string IP { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
