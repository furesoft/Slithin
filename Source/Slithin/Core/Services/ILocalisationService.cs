namespace Slithin.Core.Services;

public interface ILocalisationService
{
    string GetString(string key);

    public string GetStringFormat(string key, params object[] args)
    {
        return string.Format(key, args);
    }

    void Init();
}
