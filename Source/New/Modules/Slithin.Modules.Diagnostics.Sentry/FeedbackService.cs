using Sentry;
using Slithin.Modules.Diagnostics.Sentry.Models;

namespace Slithin.Modules.Diagnostics.Sentry;

internal class FeedbackServiceImpl : IFeedbackService
{
    public void SendFeedback(string message, string title = "An event that will receive user feedback.", 
        string email = "fake@slithin.de")
    {
        var eventId = SentrySdk.CaptureMessage(title);
        SentrySdk.CaptureUserFeedback(eventId, email, message, Environment.UserName);
    }
}
