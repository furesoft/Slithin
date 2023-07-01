using AuroraModularis.Core;
using Slithin.Entities;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.Repository.Models;

namespace Slithin.Modules.Repository;

internal class LoginServiceImpl : ILoginService
{
    private IPathManager _pathManager;
    private IDatabaseService _dbService;
    private LoginInfo? _selectedLoginCredential;
    private readonly ServiceContainer _container;

    public LoginServiceImpl(ServiceContainer container)
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
        Init();

        var db = _dbService.GetDatabase();

        return db.FindAll<LoginInfo>().ToArray();
    }

    public void RememberLoginCredencials(LoginInfo loginInfo)
    {
        var db = _dbService.GetDatabase();

        db.Insert(loginInfo);
    }

    public void SetLoginCredential(LoginInfo loginInfo)
    {
        _selectedLoginCredential = loginInfo;

        _pathManager.ReLink(loginInfo.Name);
    }

    public void UpdateIPAfterUpdate()
    {
        var db = _dbService.GetDatabase();

        db.Update<LoginInfo>(_selectedLoginCredential);
    }

    public void UpdateLoginCredential(LoginInfo info)
    {
        var db = _dbService.GetDatabase();

        db.Update<LoginInfo>(info);
    }

    public void Remove(LoginInfo loginInfo)
    {
        var db = _dbService.GetDatabase();

        db.Delete<LoginInfo>(loginInfo._id);
    }
}
