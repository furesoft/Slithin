using System.Linq;
using LiteDB;
using Slithin.Core.Scripting;
using Slithin.Models;

namespace Slithin.Core.Services.Implementations
{
    public class LoginServiceImpl : ILoginService
    {
        private readonly LiteDatabase _db;
        private readonly EventStorage _events;

        public LoginServiceImpl(LiteDatabase db, EventStorage events)
        {
            _db = db;
            _events = events;
        }

        public LoginInfo[] GetLoginCredentials()
        {
            var collection = _db.GetCollection<LoginInfo>();

            return collection.FindAll().ToArray();
        }

        public void RememberLoginCredencials(LoginInfo info)
        {
            var collection = _db.GetCollection<LoginInfo>();

            collection.Insert(info);
        }
    }
}
