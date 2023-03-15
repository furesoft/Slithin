namespace Slithin.Modules.Device.Models;

/// <summary>
/// A data structure to hold the status of files for synchronizing from the device
/// </summary>
public readonly record struct FileFetchResult
{
    public string ShortPath { get; init; }
    public string FullPath { get; init; }
    public long LastModified { get; init; }
}
