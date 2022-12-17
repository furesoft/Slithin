using Slithin.Modules.Device.Models;

namespace Slithin.Modules.Device.Mock;

public class XochitlMock : IXochitlService
{
    public bool GetIsBeta()
    {
        return false;
    }

    public string GetProperty(string key, string section)
    {
        return "no value";
    }

    public string[] GetShareEmailAddresses()
    {
        return new[] { "test@max.mus" };
    }

    public string GetToken(string key, string section)
    {
        return "";
    }

    public void Init()
    {
    }

    public void Save()
    {
    }

    public void SetPowerOffDelay(int value)
    {
    }

    public void SetProperty(string key, string section, object value)
    {
    }

    public void SetShareMailAddresses(IEnumerable<string> mailAddresses)
    {
    }

    public void Upload()
    {
    }
}
