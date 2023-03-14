﻿using System.Windows.Input;
using Slithin.Controls.Navigation;
using Slithin.Core.MVVM;
using Slithin.Modules.Resources.UI.Pages;
using Slithin.Modules.Settings.Models;

namespace Slithin.Modules.Resources.UI.ViewModels;

public sealed class LoginModalViewModel : ModalBaseViewModel
{
    private readonly ISettingsService _settingsService;
    private string _password;
    private string _username;

    public LoginModalViewModel(ISettingsService settingsService)
    {
        ShowRegisterCommand = new DelegateCommand(_ =>
        {
            var frame = Frame.GetFrame("loginFrame");

            if (frame.CanGoBack)
            {
                frame.GoBack();
            }
            else if (frame.CanGoForward)
            {
                frame.GoForward();
            }
        });

        ConfirmCommand = new DelegateCommand(Confirm);
        _settingsService = settingsService;
    }

    public ICommand ConfirmCommand { get; set; }

    public string Password
    {
        get => _password;
        set => SetValue(ref _password, value);
    }

    public ICommand ShowRegisterCommand { get; set; }

    public string Username
    {
        get => _username;
        set => SetValue(ref _username, value);
    }

    protected override void OnLoad()
    {
        Frame.GetFrame("loginFrame").Navigate(typeof(RegisterFramePage));
        Frame.GetFrame("loginFrame").Navigate(typeof(LoginFramePage));
    }

    private void Confirm(object obj)
    {
        Task.Run(() =>
        {
  
            var settings = _settingsService.GetSettings();
            _settingsService.Save(settings);

            Frame.GetFrame("resourcesFrame").Navigate(typeof(ResourcesMainPage));
        });
    }
}
