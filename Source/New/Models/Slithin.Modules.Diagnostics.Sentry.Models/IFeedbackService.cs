﻿namespace Slithin.Modules.Diagnostics.Sentry.Models;

/// <summary>
/// A service to send user feedback
/// </summary>
public interface IFeedbackService
{
    /// <summary>
    /// Transfer feedback to developer
    /// </summary>
    /// <param name="message"></param>
    void SendFeedback(string message);
}
