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

    void Remove(LoginInfo loginInfo);
}
