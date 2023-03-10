namespace Slithin.Modules.UI.Models;

/// <summary>
/// A controller for the status modal. It closes autmaticly with using statement
/// </summary>
public interface IStatusController : IDisposable
{
    /// <summary>
    /// The CancellationToken to check if the current task should be cancelled
    /// </summary>
    CancellationToken Token { get; }

    /// <summary>
    /// Close the status modal and trigger IsCancellationRequested on Token
    /// </summary>
    void Cancel();

    /// <summary>
    /// Change the status message
    /// </summary>
    /// <param name="message"></param>
    void Step(string message);

    /// <summary>
    /// Close the status modal
    /// </summary>
    void Finish();
}
