using Microsoft.VisualStudio.TestTools.UnitTesting;
using Slithin.Core.Remarkable.Exporting;
using System.Diagnostics;

namespace Tests
{
    [TestClass]
    public class PageRangeTests
    {
        [TestMethod]
        public void Parse_Should_Pass()
        {
            var result = PageRange.Parse("1-3;7;9-");
            var indices = PageRange.ToIndices(result.AsT0, 15);

            Debug.WriteLine("1-3;7;9-");
            Debug.WriteLine(string.Join(",", indices));
        }

        [TestMethod]
        public void ParseSingle_Page_Should_Pass()
        {
            var result = PageRange.ParseSingle("3");
            var toTest = new PageRange(3, 3);
        }
    }
}