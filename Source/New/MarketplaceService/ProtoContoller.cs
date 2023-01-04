namespace MarketplaceService;

public class ProtoController
{
    private readonly string _baseDirectory;

    public ProtoController(IWebHostEnvironment webHost)
    {
        _baseDirectory = webHost.ContentRootPath;
    }

    public string Get(string protoName)
    {
        var filePath = $"{_baseDirectory}\\Protos\\{protoName}";
        var exist = File.Exists(filePath);

        return exist ? filePath : null;
    }

    public IEnumerable<string> GetAvailable()
    {
        return Directory.GetFiles($"{_baseDirectory}\\Protos", "*.proto").Select(Path.GetFileName);
    }
}
