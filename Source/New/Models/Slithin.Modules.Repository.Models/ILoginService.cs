using LiteDB;
using Slithin.Entities;

namespace Slithin.Modules.Repository.Models;

public interface ILoginService
{
    LoginInfo GetCurrentCredential();

    LoginInfo[] GetLoginCredentials();

    void RememberLoginCredencials(LoginInfo loginInfo);

    void SetLoginCredential(LoginInfo loginInfo);

    void UpdateIPAfterUpdate();

    void UpdateLoginCredential(LoginInfo info);

    void Init();
}

public interface IDatabaseService
{
    DatabaseAccessor GetDatabase();
}

public class DatabaseAccessor
{
    public DatabaseAccessor(LiteDatabase dB)
    {
        DB = dB;
    }

    ~DatabaseAccessor()
    {
        DB.Dispose();
    }

    public LiteDatabase DB { get; set; }
}
