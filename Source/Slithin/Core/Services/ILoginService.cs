using Slithin.Models;

namespace Slithin.Core.Services;

public interface ILoginService
{
    LoginInfo GetCurrentCredential();

    LoginInfo[] GetLoginCredentials();

    void RememberLoginCredencials(LoginInfo loginInfo);

    void SetLoginCredential(LoginInfo loginInfo);

    void UpdateIPAfterUpdate();

    void UpdateLoginCredential(LoginInfo info);
}
