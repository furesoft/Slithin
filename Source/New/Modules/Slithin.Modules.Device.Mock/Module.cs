using AuroraModularis.Core;
using Slithin.Modules.Device.Models;

namespace Slithin.Modules.Device.Mock;

internal class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        if (File.Exists("device.zip"))
        {
            return Task.CompletedTask;
        }

        var stream = GetType().Assembly.GetManifestResourceStream("Slithin.Modules.Device.Mock.device.zip");
        using var fileStrm = File.OpenWrite(Path.Combine(Environment.CurrentDirectory, "device.zip"));
            
        stream.CopyTo(fileStrm);
        
        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register<IRemarkableDevice>(new MockDevice(container)).AsSingleton();
        container.Register<IXochitlService>(new XochitlImpl(container)).AsSingleton();
        container.Register<PathList>(new PathList()).AsSingleton();
    }
}
