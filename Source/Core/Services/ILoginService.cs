using Slithin.Models;

namespace Slithin.Core.Services
{
    public interface ILoginService
    {
        LoginInfo[] GetLoginCredentials();

        void RememberLoginCredencials(LoginInfo loginInfo);

        void SetLoginCredential(LoginInfo loginInfo);

        void UpdateIPAfterUpdate();
    }
}
