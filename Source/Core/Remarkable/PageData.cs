namespace Slithin.Core.Remarkable
{
    public class PageData
    {
        public string[] Data { get; set; }

        public void Parse(string content)
        {
            Data = content.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
