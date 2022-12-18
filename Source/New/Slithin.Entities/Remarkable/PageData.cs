namespace Slithin.Entities.Remarkable;

public struct PageData
{
    public string[] Data { get; set; }

    public void Parse(string content)
    {
        Data = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    }
}
