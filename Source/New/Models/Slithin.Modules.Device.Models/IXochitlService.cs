namespace Slithin.Modules.Device;

public interface IXochitlService
{
    bool GetIsBeta();

    string GetProperty(string key, string section);

    string[] GetShareEmailAddresses();

    string GetToken(string key, string section);

    void Init();

    void Save();

    void SetPowerOffDelay(int value);

    void SetProperty(string key, string section, object value);

    void SetShareMailAddresses(IEnumerable<string> mailAddresses);

    void Upload();
}
