namespace Slithin.Modules.UI.Models;

public interface IStatusController
{
    void Step(string message);

    void Finish();
}
