using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Slithin.UpdateInstaller;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var executable = args[0];
        var slithinPath = new FileInfo(executable).Directory.ToString();
        var basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SlithinUpdate");

        await CloseSlithin();

        foreach (var file in Directory.GetFiles(basePath, "*.*", SearchOption.AllDirectories))
        {
            File.Copy(file, Path.Combine(slithinPath, Path.GetFileName(file)), true);

            File.Delete(file);
        }

        Process.Start(new ProcessStartInfo("dotnet", executable));

        Environment.Exit(0);
    }

    private static async Task CloseSlithin()
    {
        var processes = Process.GetProcessesByName("Slithin");

        foreach (var process in processes)
        {
            process.Kill();
            await process.WaitForExitAsync();
        }

        await Task.Delay(1000);
    }
}
