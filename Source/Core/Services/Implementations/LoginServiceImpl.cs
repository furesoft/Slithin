using System.Linq;
using LiteDB;
using Slithin.Core.Scripting;

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

        public LoginInfo GetLoginCredentials()
        {
            var collection = _db.GetCollection<LoginInfo>();

            if (collection.Count() == 1)
            {
                return collection.FindAll().First();
            }
            else
            {
                return new(null, null, false);
            }
        }

        public void RememberLoginCredencials(LoginInfo viewModel)
        {
            var collection = _db.GetCollection<LoginInfo>();

            if (collection.Count() == 1)
            {
                //collection.Update(viewModel);
            }
            else
            {
                collection.Insert(viewModel);
            }
        }
    }
}
