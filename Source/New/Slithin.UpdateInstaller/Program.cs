using System;
using System.Diagnostics;
using System.IO;

namespace Slithin.UpdateInstaller;

public static class Program
{
    public static void Main(string[] args)
    {
        var executable = args[0];
        var slithinPath = new FileInfo(executable).Directory.ToString();
        var basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SlithinUpdate");

        foreach (var file in Directory.GetFiles(basePath, "*.*", SearchOption.AllDirectories))
        {
            File.Copy(file, Path.Combine(slithinPath, Path.GetFileName(file)), true);
            
            File.Delete(file);
        }
        
        Process.Start(new ProcessStartInfo("dotnet", executable));

        Environment.Exit(0);
    }
}
