using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Controls;
using Slithin.Controls.Ports.StepBar;
using Slithin.Core.MVVM;
using Slithin.Modules.FirstStart.Steps;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Settings.Models;

namespace Slithin.Modules.FirstStart.ViewModels;

internal class FirstStartViewModel : BaseViewModel
{
    private readonly ILocalisationService _localisationService;
    private readonly IPathManager _pathManager;
    private readonly ILoginService _loginService;
    private readonly ISettingsService _settingsService;
    private string _buttonText;
    private int _index;

    public FirstStartViewModel(
        ILocalisationService localisationService,
        IPathManager pathManager,
        ISettingsService settingsService,
        ILoginService loginService)
    {
        ButtonText = localisationService.GetString("Next");
        _localisationService = localisationService;
        _pathManager = pathManager;
        _settingsService = settingsService;
        _loginService = loginService;

        AddStep("Welcome", new WelcomeStep());
        AddStep("Device", new DeviceStep(), LoginInfoViewModel);
        AddStep("Settings", new SettingsStep(), SettingsViewModel);
        AddStep("Finish", new FinishStep());

        NextCommand = new DelegateCommand(Next);
    }

    public SettingsViewModel SettingsViewModel { get; set; } = new();
    public LoginInfoViewModel LoginInfoViewModel { get; set; } = new();

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

    public ICommand NextCommand { get; set; }
    public ObservableCollection<UserControl> StepControls { get; set; } = new();

    public ObservableCollection<StepBarItem> StepTitles { get; set; } = new();

    public void AddStep(string title,
                        UserControl control,
                        BaseViewModel viewModel = null)
    {
        control.DataContext = viewModel;

        StepTitles.Add(new StepBarItem()
        {
            Content = _localisationService.GetString(title),
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top
        });
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
            SettingsViewModel.Settings.IsFirstStart = false;
            SettingsViewModel.Settings._id = _settingsService.GetSettings()._id;

            _settingsService.Save(SettingsViewModel.Settings);

            _loginService.RememberLoginCredencials(LoginInfoViewModel.SelectedLogin);
            //_loginService.SetLoginCredential(DeviceVM.SelectedLogin);

            _pathManager.ReLink(LoginInfoViewModel.SelectedLogin.Name);
            _pathManager.Init();

            System.Diagnostics.Process.Start(Environment.ProcessPath);
            Environment.Exit(0);
        }
    }
}
