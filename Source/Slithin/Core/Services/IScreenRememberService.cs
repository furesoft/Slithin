namespace Slithin.Core.Services;

public interface IScreenRememberService
{
    bool HasMultipleScreens();

    void Remember();

    void Restore();
}
