using System.Collections.ObjectModel;
using Avalonia.Controls;
using Slithin.Controls.Ports.StepBar;
using Slithin.Core.MVVM;
using Slithin.Core.Services;
using Slithin.UI.FirstStartSteps;

namespace Slithin.ViewModels;

public class FirstStartViewModel : BaseViewModel
{
    private readonly ILocalisationService _localisationService;
    private string _buttonText;
    private int _index;

    public FirstStartViewModel(ILocalisationService localisationService)
    {
        ButtonText = localisationService.GetString("Next");
        _localisationService = localisationService;

        AddStep("Welcome", new WelcomeStep());
        AddStep("Device", new DeviceStep());
        AddStep("Settings", new SettingsStep());
        AddStep("Finish", new FinishStep());
    }

    public string ButtonText
    {
        get { return _buttonText; }
        set { SetValue(ref _buttonText, value); }
    }

    public int Index
    {
        get { return _index; }
        set
        {
            SetValue(ref _index, value);

            if (_index >= StepTitles.Count - 1)
            {
                ButtonText = _localisationService.GetString("Start");
            }
        }
    }

    public ObservableCollection<UserControl> StepControls { get; set; } = new();
    public ObservableCollection<StepBarItem> StepTitles { get; set; } = new();

    public void AddStep(string title, UserControl control)
    {
        StepTitles.Add(new StepBarItem() { Content = title, VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top });
        StepControls.Add(control);
    }
}
