using System.Windows.Input;
using AuroraModularis.Core;
using Slithin.Core.MVVM;
using Slithin.Modules.Diagnostics.Sentry.Models;

namespace Slithin.Modules.Bot.MessageModels;

public class UnknownMessageModel : SimpleMessageModel
{
    private readonly UnknownRequestMessage _ukmsg;
    private bool _isVisible = true;

    public UnknownMessageModel(UnknownRequestMessage ukmsg)
    {
        _ukmsg = ukmsg;

        Command = new DelegateCommand(SendFeedback);
    }

    private void SendFeedback(object obj)
    {
        var feedbackService = ServiceContainer.Current.Resolve<IFeedbackService>();
        
        feedbackService.SendFeedback("Chat Bot Request Suggestion: " + _ukmsg.ResultRequest.NormalizedText);
        IsVisible = false;
    }

    public bool IsVisible
    {
        get => _isVisible;
        set => this.SetValue(ref _isVisible, value);
    }

    public ICommand Command { get; set; }
}
