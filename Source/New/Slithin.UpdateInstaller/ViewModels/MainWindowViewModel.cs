using System;
using System.Diagnostics;
using System.IO;
using Slithin.Core.MVVM;

namespace Slithin.UpdateInstaller.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    protected override void OnLoad()
    {
        var executable = Environment.GetCommandLineArgs()[1];
        var slithinPath = new FileInfo(executable).Directory.ToString();
        var basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SlithinUpdate");

        foreach (var file in Directory.GetFiles(basePath))
        {
            File.Copy(file, Path.Combine(slithinPath, Path.GetFileName(file)), true);
        }
        
        Process.Start(new ProcessStartInfo("dotnet", executable));

        Environment.Exit(0);
    }
}
