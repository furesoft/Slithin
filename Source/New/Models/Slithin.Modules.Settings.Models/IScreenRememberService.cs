namespace Slithin.Modules.Settings.Models;

public interface IScreenRememberService
{
    bool HasMultipleScreens();

    void Remember();

    void Restore();
}
