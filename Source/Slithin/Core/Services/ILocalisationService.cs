namespace Slithin.Core.Services;

public interface ILocalisationService
{
    string GetString(string key);

    void Init();
}
