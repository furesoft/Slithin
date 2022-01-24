namespace Slithin.Scripting.Parsing;

public class SourceDocument
{
    public SourceDocument(string filename)
    {
        Filename = filename;
        Source = File.ReadAllText(filename);
    }

    public SourceDocument(string filename, string source) : this(filename)
    {
        Source = source;
    }

    public string Filename { get; set; }
    public string Source { get; set; }
}
