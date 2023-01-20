using System.Text;
using System.Windows.Input;
using Slithin.Core.MVVM;
using Slithin.Modules.Diagnostics.Sentry.Models;

namespace Slithin.Modules.Settings.ViewModels;

internal class FeedbackViewModel : BaseViewModel
{
    private readonly IFeedbackService _feedbackService;
    private double _appearance;
    private string _comment;
    private double _compatibility;
    private double _features;
    private double _performance;
    private double _usability;

    public FeedbackViewModel(IFeedbackService feedbackService)
    {
        SendCommand = new DelegateCommand(Send);
        _feedbackService = feedbackService;
    }

    public double Appeareance
    {
        get { return _appearance; }
        set { SetValue(ref _appearance, value); }
    }

    public string Comment
    {
        get { return _comment; }
        set { SetValue(ref _comment, value); }
    }

    public double Compatibility
    {
        get { return _compatibility; }
        set { SetValue(ref _compatibility, value); }
    }

    public double Features
    {
        get { return _features; }
        set { SetValue(ref _features, value); }
    }

    public double Performance
    {
        get { return _performance; }
        set { SetValue(ref _performance, value); }
    }

    public ICommand SendCommand { get; set; }

    public double Usability
    {
        get { return _usability; }
        set { SetValue(ref _usability, value); }
    }

    private void Send(object obj)
    {
        var contentBuilder = new StringBuilder();
        contentBuilder.AppendLine($"Usability: {Usability}");
        contentBuilder.AppendLine($"Performance: {Performance}");
        contentBuilder.AppendLine($"Apearance: {Appeareance}");
        contentBuilder.AppendLine($"Features: {Features}");
        contentBuilder.AppendLine($"Compatiblity: {Compatibility}");
        contentBuilder.AppendLine();
        contentBuilder.AppendLine($"Comment: {Comment}");

        _feedbackService.SendFeedback(contentBuilder.ToString());

        RequestClose();
    }
}
