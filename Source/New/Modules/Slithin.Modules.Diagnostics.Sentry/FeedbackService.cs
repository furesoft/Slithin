using AuroraModularis.Core;
using Sentry;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.Diagnostics.Sentry.Models;

namespace Slithin.Modules.Diagnostics.Sentry;

internal class FeedbackServiceImpl : IFeedbackService
{
    public void SendFeedback(string message, string title = "User feedback.", 
        string email = "fake@slithin.de")
    {
        var profanityFilter = ServiceContainer.Current.Resolve<IProfanityFilter>();

        if (profanityFilter.HasProfanities(message))
        {
            return;
        }
        
        var eventId = SentrySdk.CaptureMessage(title);
        SentrySdk.CaptureUserFeedback(eventId, email, message, Environment.UserName);
    }
}
