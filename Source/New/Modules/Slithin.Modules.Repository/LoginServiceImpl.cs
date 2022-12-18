using AuroraModularis.Core;
using LiteDB;
using Slithin.Entities;
using Slithin.Modules.Repository.Models;

namespace Slithin.Modules.Repository;

public class LoginServiceImpl : ILoginService
{
    private IPathManager _pathManager;
    private IDatabaseService _dbService;
    private LoginInfo? _selectedLoginCredential;
    private Container _container;

    public LoginServiceImpl(Container container)
    {
        _container = container;
    }

    public void Init()
    {
        _pathManager = _container.Resolve<IPathManager>();
        _dbService = _container.Resolve<IDatabaseService>();
    }

    public LoginInfo GetCurrentCredential()
    {
        return _selectedLoginCredential;
    }

    public LoginInfo[] GetLoginCredentials()
    {
        var db = _dbService.GetDatabase();
        var collection = db.DB.GetCollection<LoginInfo>();

        return collection.FindAll().ToArray();
    }

    public void RememberLoginCredencials(LoginInfo info)
    {
        var db = _dbService.GetDatabase();
        var collection = db.DB.GetCollection<LoginInfo>();

        collection.Insert(info);
    }

    public void SetLoginCredential(LoginInfo loginInfo)
    {
        _selectedLoginCredential = loginInfo;

        _pathManager.ReLink(loginInfo.Name);
    }

    public void UpdateIPAfterUpdate()
    {
        var db = _dbService.GetDatabase();
        var collection = db.DB.GetCollection<LoginInfo>();

        collection.Update(_selectedLoginCredential);
    }

    public void UpdateLoginCredential(LoginInfo info)
    {
        var db = _dbService.GetDatabase();
        var collection = db.DB.GetCollection<LoginInfo>();

        collection.Update(info);
    }

    public void Remove(LoginInfo loginInfo)
    {
        var db = _dbService.GetDatabase();

        var collection = db.DB.GetCollection<LoginInfo>();
        collection.Delete(loginInfo._id);
    }
}
