using System.Collections.Generic;
using OneOf;

namespace Slithin.Core.Remarkable.Exporting
{
    public record PageRange(int From, int To)
    {
        public static OneOf<List<PageRange>, bool> Parse(string src)
        {
            if (!string.IsNullOrEmpty(src))
            {
                src = src.Replace(" ", "").Replace("\t", "");
                var spl = src.Split(';', System.StringSplitOptions.RemoveEmptyEntries);
                var result = new List<PageRange>();

                foreach (var part in spl)
                {
                    var match = PageRange.ParseSingle(part);

                    if (!match.AsT1)
                    {
                        result.Add(match.AsT0);
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            return true;
        }

        //"1-5" Sites 1,2,3,4,5
        //"2" Site 2
        //"3-" Sites 3,4,...,PageCount(-1)
        public static OneOf<PageRange, bool> ParseSingle(string src)
        {
            //page starts at index 1, need to translate to coding index!

            if (src.Contains("-"))
            {
                var split = src.Split('-', System.StringSplitOptions.RemoveEmptyEntries);

                if (split.Length == 1)
                {
                    if (int.TryParse(split[0], out var page))
                    {
                        return new PageRange(page, -1);//to the end of the document
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    if (int.TryParse(split[0], out var leftPart) && int.TryParse(split[1], out var rightPart))
                    {
                        return new PageRange(leftPart, rightPart);
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (int.TryParse(src, out var page))
                {
                    return new PageRange(page, page);
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
