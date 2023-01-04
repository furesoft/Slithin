namespace Slithin.Entities.Remarkable;

public struct PageData
{
    public string[] Data { get; set; }

    public static PageData Parse(string content)
    {
        return new()
        {
            Data = content.Split('\n', StringSplitOptions.RemoveEmptyEntries)
        };
    }
}
