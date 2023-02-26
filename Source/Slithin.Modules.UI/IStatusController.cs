namespace Slithin.Modules.UI.Models;

public interface IStatusController
{
    CancellationToken Token { get; }

    void Cancel();

    void Step(string message);

    void Finish();
}
