namespace Slithin.Modules.Diagnostics.Sentry.Models;

/// <summary>
/// A service to send user feedback
/// </summary>
public interface IFeedbackService
{
    void SendFeedback(string message);
}
