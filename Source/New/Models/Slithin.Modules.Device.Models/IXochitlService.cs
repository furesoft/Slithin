namespace Slithin.Modules.Device.Models;

/// <summary>
/// Service to work with the Xochitl configuration file
/// </summary>
public interface IXochitlService
{
    /// <summary>
    /// Check if the user logged on the device is in beta channel
    /// </summary>
    /// <returns></returns>
    bool GetIsBeta();

    /// <summary>
    /// Get a property from the xochitl.conf ini file
    /// </summary>
    /// <param name="key"></param>
    /// <param name="section"></param>
    /// <returns></returns>
    string GetProperty(string key, string section);
    
    string[] GetShareEmailAddresses();

    string GetToken(string key, string section);

    void Init();

    /// <summary>
    /// Save all property changes to local xochitl.conf
    /// </summary>
    void Save();

    void SetPowerOffDelay(int value);

    void SetProperty(string key, string section, object value);

    void SetShareMailAddresses(IEnumerable<string> mailAddresses);

    /// <summary>
    /// Upload the local xochitl.conf to the device
    /// </summary>
    void Upload();
}
