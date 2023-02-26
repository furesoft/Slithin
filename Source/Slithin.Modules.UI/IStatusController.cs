namespace Slithin.Modules.UI.Models;

public interface IStatusController : IDisposable
{
    CancellationToken Token { get; }

    void Cancel();

    void Step(string message);

    void Finish();
}
