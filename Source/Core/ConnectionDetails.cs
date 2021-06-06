namespace Slithin.Core
{
    public class ConnectionDetails
    {
        public LiteDB.ObjectId? _id { get; set; }
        public string IP { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ConnectionDetails()
        {
            IP = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
        }
    }
}