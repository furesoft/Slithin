using System.Diagnostics;
using System.Text;
using AuroraModularis.Core;
using Ipfs;
using Ipfs.Http;
using Newtonsoft.Json;
using OwlCore.Kubo;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.Peering.Models;

namespace Slithin.Modules.Peering;

public class PeerImpl : IPeer
{
    private IpfsClient _client;
    private PeerRoom _room;

    public async void Init()
    {
        var process = Process.GetProcessesByName("ipfs").FirstOrDefault();

        if (process is not null)
        {
            process.Kill();
        }

        var downloader = new KuboDownloader();

        //ToDo: copy ipfs binary to slithin execution directory
        var latestKuboBinary = await downloader.DownloadLatestBinaryAsync();
        
        var repoPath = Path.Combine(ServiceContainer.Current.Resolve<IPathManager>().SlithinDir, "Peering");
        var bootstrapper = new KuboBootstrapper(latestKuboBinary, repoPath);
        
        AppDomain.CurrentDomain.ProcessExit += (s, e) =>
        {
            bootstrapper.Dispose();
        };
        
        await bootstrapper.StartAsync();
        
        _client = new(bootstrapper.ApiUri.ToString());
        _room = new(await _client.IdAsync(), _client.PubSub, "Slithin");
        _room.MessageReceived += MessageReceived;
    }

    private void MessageReceived(object? sender, IPublishedMessage e)
    {
        var document = Encoding.Default.GetString(e.DataBytes);
        var msg = JsonConvert.DeserializeObject(document);
        
        OnMessageReceived?.Invoke(msg);
    }

    public void Broadcast<T>(T message)
    {
        var document = JsonConvert.SerializeObject(message, new JsonSerializerSettings(){ TypeNameHandling = TypeNameHandling.All });
        
        _room.PublishAsync(Encoding.Default.GetBytes(document));
    }

    public event Action<object>? OnMessageReceived;
}
