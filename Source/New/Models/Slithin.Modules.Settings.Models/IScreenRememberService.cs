namespace Slithin.Modules.Settings.Models;

/// <summary>
/// A service to remember on which screen is the window shown
/// </summary>
public interface IScreenRememberService
{
    bool HasMultipleScreens();

    void Remember();

    void Restore();
}
