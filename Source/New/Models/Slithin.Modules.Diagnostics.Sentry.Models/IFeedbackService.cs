namespace Slithin.Modules.Diagnostics.Sentry.Models;

public interface IFeedbackService
{
    void SendFeedback(string message);
}
