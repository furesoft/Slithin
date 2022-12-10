using Slithin.Core.Remarkable;
namespace Slithin.Core.Remarkable.Models;

public struct PageData
{
    public string[] Data { get; set; }

    public void Parse(string content)
    {
        Data = content.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
    }
}
