using Sentry;
using Slithin.Modules.Diagnostics.Sentry.Models;

namespace Slithin.Modules.Diagnostics.Sentry;

internal class FeedbackServiceImpl : IFeedbackService
{
    public void SendFeedback(string message)
    {
        var eventId = SentrySdk.CaptureMessage("An event that will receive user feedback.");
        SentrySdk.CaptureUserFeedback(eventId, "fake@slithin.sl", message, Environment.UserName);
    }
}
