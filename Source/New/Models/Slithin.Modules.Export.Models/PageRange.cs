using DotNext;
using OneOf;

namespace Slithin.Modules.Export.Models;

public record PageRange(int From, int To)
{
    public static Result<List<PageRange>> Parse(string src)
    {
        if (string.IsNullOrEmpty(src) || string.IsNullOrWhiteSpace(src))
            return new(new Exception("Input is empty"));

        if (src == "-")
            return new(new Exception("Invalid Range"));

        src = src.Replace(" ", "").Replace("\t", "");
        var spl = src.Split(';', StringSplitOptions.RemoveEmptyEntries);
        var result = new List<PageRange>();

        foreach (var part in spl)
        {
            var match = ParseSingle(part);

            if (match)
            {
                result.Add(match.Value);
            }
            else
            {
                return new(match.Error);
            }
        }

        return result;
    }

    //"1-5" Sites 1,2,3,4,5
    //"2" Site 2
    //"3-" Sites 3,4,...,PageCount(-1)
    public static Result<PageRange> ParseSingle(string src)
    {
        //page starts at index 1, need to translate to coding index!

        if (!src.Contains('-'))
        {
            return int.TryParse(src, out var page) ? new PageRange(page, page) : new Result<PageRange>(new Exception("Invalid Index"));
        }

        var split = src.Split('-', StringSplitOptions.RemoveEmptyEntries);

        if (split.Length == 1)
        {
            if (int.TryParse(split[0], out var page))
            {
                return new PageRange(page, -1);//to the end of the document
            }
        }
        else
        {
            if (int.TryParse(split[0], out var leftPart) && int.TryParse(split[1], out var rightPart))
            {
                return new PageRange(leftPart, rightPart);
            }
        }
        
        return new(new Exception("Error Parsing Index"));
    }

    public static IEnumerable<int> ToIndices(List<PageRange> ranges, int max)
    {
        var result = new List<int>();

        foreach (var range in ranges)
        {
            if (range.From == range.To)
            {
                result.Add(range.From - 1);
            }
            else if (range.To == -1)
            {
                result.AddRange(Enumerable.Range(range.From - 1, max - range.From + 1));
            }
            else
            {
                result.AddRange(Enumerable.Range(range.From - 1, range.To));
            }
        }

        return result;
    }
}
