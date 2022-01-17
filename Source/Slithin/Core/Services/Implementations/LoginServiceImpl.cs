using System.Linq;
using LiteDB;
using Slithin.Models;

namespace Slithin.Core.Services.Implementations;

public class LoginServiceImpl : ILoginService
{
    private readonly LiteDatabase _db;
    private readonly IPathManager _pathManager;
    private LoginInfo _selectedLoginCredential;

    public LoginServiceImpl(LiteDatabase db, IPathManager pathManager)
    {
        _db = db;
        _pathManager = pathManager;
    }

    public LoginInfo GetCurrentCredential()
    {
        return _selectedLoginCredential;
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

    public void SetLoginCredential(LoginInfo loginInfo)
    {
        _selectedLoginCredential = loginInfo;

        _pathManager.Relink();
    }

    public void UpdateIPAfterUpdate()
    {
        var collection = _db.GetCollection<LoginInfo>();

        collection.Update(_selectedLoginCredential);
    }

    public void UpdateLoginCredential(LoginInfo info)
    {
        var collection = _db.GetCollection<LoginInfo>();

        collection.Update(info);
    }
}
