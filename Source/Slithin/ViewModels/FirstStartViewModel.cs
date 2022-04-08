using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Slithin.Controls.Ports.StepBar;
using Slithin.Core;
using Slithin.Core.MVVM;
using Slithin.Core.Services;
using Slithin.UI.FirstStartSteps;
using Slithin.UI.Views;
using Slithin.ViewModels.Pages;

namespace Slithin.ViewModels;

public class FirstStartViewModel : BaseViewModel
{
    private readonly ILocalisationService _localisationService;
    private readonly ILoginService _loginService;
    private readonly ISettingsService _settingsService;
    private string _buttonText;
    private int _index;

    public FirstStartViewModel(
        ILocalisationService localisationService,
        AddDeviceWindowViewModel deviceVm,
        SettingsPageViewModel settingsVm,
        ISettingsService settingsService,
        ILoginService loginService)
    {
        ButtonText = localisationService.GetString("Next");
        _localisationService = localisationService;

        DeviceVM = deviceVm;
        SettingsVM = settingsVm;
        _settingsService = settingsService;
        _loginService = loginService;
        AddStep("Welcome", new WelcomeStep());
        AddStep("Device", new DeviceStep(), DeviceVM);
        AddStep("Settings", new SettingsStep(), SettingsVM);
        AddStep("Finish", new FinishStep());

        NextCommand = new DelegateCommand(Next);
    }

    public string ButtonText
    {
        get { return _buttonText; }
        set { SetValue(ref _buttonText, value); }
    }

    public AddDeviceWindowViewModel DeviceVM { get; set; }

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

    public ICommand NextCommand { get; set; }
    public SettingsPageViewModel SettingsVM { get; set; }
    public ObservableCollection<UserControl> StepControls { get; set; } = new();

    public ObservableCollection<StepBarItem> StepTitles { get; set; } = new();

    public void AddStep(string title,
                        UserControl control,
                        BaseViewModel viewModel = null
                        )
    {
        control.DataContext = viewModel;

        StepTitles.Add(new StepBarItem() { Content = _localisationService.GetString(title), VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top });
        StepControls.Add(control);
    }

    private void Next(object obj)
    {
        if (Index < StepTitles.Count - 1)
        {
            StepManager.Next();

            var vm = StepTitles[Index].DataContext;
            if (vm is BaseViewModel bvm)
            {
                bvm.Load();
            }
        }
        else
        {
            RequestClose();

            var connectViewModel = ServiceLocator.Container.Resolve<ConnectionWindowViewModel>();

            _loginService.RememberLoginCredencials(DeviceVM.SelectedLogin);
            _loginService.SetLoginCredential(DeviceVM.SelectedLogin);

            connectViewModel.SelectedLogin = DeviceVM.SelectedLogin;
            connectViewModel.ConnectCommand.Execute(null);

            var settings = _settingsService.GetSettings();
            settings.IsFirstStart = false;

            _settingsService.Save(settings);

            if (App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
                desktop.MainWindow.Show();
            }
        }
    }
}
