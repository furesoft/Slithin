using System.Windows.Input;
using AuroraModularis.Core;
using Slithin.Core.MVVM;
using Slithin.Modules.Diagnostics.Sentry.Models;

namespace Slithin.Modules.Bot.MessageModels;

public class UnknownMessageModel : BaseViewModel
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
        
        feedbackService.SendFeedback("Request: " + _ukmsg.ResultRequest.Text, 
            "Bot Suggestion", "bot@slithin.de");
        
        IsVisible = false;
    }

    public bool IsVisible
    {
        get => _isVisible;
        set => this.SetValue(ref _isVisible, value);
    }

    public ICommand Command { get; set; }
}
