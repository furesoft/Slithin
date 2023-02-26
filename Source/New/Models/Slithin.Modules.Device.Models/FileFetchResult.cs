namespace Slithin.Modules.Device.Models;

public struct FileFetchResult
{
    public string ShortPath { get; init; }
    public string FullPath { get; init; }
    public long LastModified { get; init; }
}
