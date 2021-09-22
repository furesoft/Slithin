using NUnit.Framework;
using OneOf;
using Slithin.Core.Remarkable.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slithin.Tests.Core.Remarkable.Exporting
{
    [TestFixture]
    [TestOf(typeof(PageRange))]
    public static class PageRangeTests
    {
        [TestOf(nameof(PageRange.Parse))]
        public class Parse : TestFixture
        {
            [TestCaseSource(nameof(Result_expect_value_source))]
            public void Result_expect_value(string input, OneOf<List<PageRange>, bool> expected)
            {
                var result
                    = PageRange
                    .Parse(input);

                Assert.Multiple(() =>
                {
                    Assert.That(result.IsT0, Is.EqualTo(expected.IsT0));
                    Assert.That(result.IsT1, Is.EqualTo(expected.IsT1));

                    switch (result.Index)
                    {
                        case 0:
                            Assert.That(result.AsT0, Is.EqualTo(expected.AsT0));
                            break;
                        case 1:
                            Assert.That(result.AsT1, Is.EqualTo(expected.AsT1));
                            break;
                        default:
                            Assert.Fail($"{result.Index} are not expected");
                            break;
                    }
                });
            }

            public static IEnumerable<TestCaseData> Result_expect_value_source()
            {
                yield return new TestCaseData(
                        "1-3",
                        Expected((from: 1, to: 3))
                    )
                    .SetName("Parse 1-3 to (from: 1, to: 3)");
                yield return new TestCaseData(
                        "7",
                        Expected((from: 7, to: 7))
                    )
                    .SetName("Parse 7 to (from: 7, to: 7)");
                yield return new TestCaseData(
                        "9-",
                        Expected((from: 9, to: -1))
                    )
                    .SetName("Parse 9- to (from: 9, to: -1)");
                yield return new TestCaseData(
                        "1-3;7;9-;5",
                        Expected((from: 1, to: 3), (from: 7, to: 7), (from: 9, to: -1), (from: 5, to: 5))
                    )
                    .SetName("Parse multiple values to multiple ranges");
                yield return new TestCaseData(
                        "              ",
                        Expected(true)
                    )
                    .SetName("Parse whitespace string");
                yield return new TestCaseData(
                        "",
                        Expected(true)
                    )
                    .SetName("Parse empty string");
                yield return new TestCaseData(
                        null,
                        Expected(true)
                    )
                    .SetName("Parse null");
                yield return new TestCaseData(
                        "🤣😂😅😎",
                        Expected(true)
                    )
                    .SetName("Parse random string");
                yield return new TestCaseData(
                        "1-3;🤣😂😅😎",
                        Expected(true)
                    )
                    .SetName("Parse random string with valid string");
                yield return new TestCaseData(
                        "-",
                        Expected(true)
                    )
                    .SetName("Parse only a -");
            }
        }

        [TestOf(nameof(PageRange.ToIndices))]
        public class ToIndices : TestFixture
        {
            [TestCaseSource(nameof(Result_expect_value_source))]
            public void Result_expect_value(List<PageRange> input, int max, int[] expected)
            {
                var result
                    = PageRange
                    .ToIndices(input, max)
                    .ToArray();

                Assert.That(result, Is.EqualTo(expected));
            }

            public static IEnumerable<TestCaseData> Result_expect_value_source()
            {
                yield return new TestCaseData(
                        RangesAsList((from: 1, to: 3)),
                        15,
                        new[] { 0, 1, 2 }
                    )
                    .SetName("Convert (from: 1, to: 3) to 0, 1, 2");
                yield return new TestCaseData(
                        RangesAsList((from: 7, to: 7)),
                        15,
                        new[] { 6 }
                    )
                    .SetName("Convert (from: 7, to: 7) to 6");
                yield return new TestCaseData(
                        RangesAsList((from: -7, to: -7)),
                        15,
                        new[] { -8 }
                    )
                    .SetName("Convert (from: -7, to: -7) to -8");
                yield return new TestCaseData(
                        RangesAsList((from: 9, to: -1)),
                        15,
                        new[] { 8, 9, 10, 11, 12, 13, 14 }
                    )
                    .SetName("Convert (from: 9, to: -1) to values from 8 to 14");
                yield return new TestCaseData(
                        RangesAsList((from: 1, to: 3), (from: 7, to: 7), (from: 9, to: -1), (from: 5, to: 5)),
                        15,
                        new[] { 0, 1, 2, 6, 8, 9, 10, 11, 12, 13, 14, 4 }
                    )
                    .SetName("Convert multiple ranges to correct range of values");
                yield return new TestCaseData(
                        RangesAsList((from: 1, to: 10)),
                        15,
                        new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }
                    )
                    .SetName("Convert (from: 1, to: 10) to values from 0 to 9");
                yield return new TestCaseData(
                        RangesAsList((from: 1, to: 10)),
                        5,
                        new[] { 0, 1, 2, 3, 4 }
                    )
                    .SetName("Convert (from: 1, to: 10) to values from 0 to 4");
                yield return new TestCaseData(
                        RangesAsList((from: 1, to: 10)),
                        -42,
                        Array.Empty<int>()
                    )
                    .SetName("Convert (from: 1, to: 10) with max -42");
                yield return new TestCaseData(
                        RangesAsList((from: 1, to: -1)),
                        5,
                        new[] { 0, 1, 2, 3, 4 }
                    )
                    .SetName("Convert (from: 1, to: -1) to values from 0 to 4");
                yield return new TestCaseData(
                        RangesAsList((from: 1, to: -1)),
                        -42,
                        Array.Empty<int>()
                    )
                    .SetName("Convert (from: 1, to: -1) with max -42");
                yield return new TestCaseData(
                        new List<PageRange>(),
                        15,
                        Array.Empty<int>()
                    )
                    .SetName("Convert empty list");
                yield return new TestCaseData(
                        null,
                        15,
                        Array.Empty<int>()
                    )
                    .SetName("Convert null");
            }
        }

        public abstract class TestFixture
        {
            protected static List<PageRange> RangesAsList(params (int from, int to)[] ranges)
                => ranges.Select(range => new PageRange(range.from, range.to)).ToList();

            protected static OneOf<List<PageRange>, bool> Expected(params (int from, int to)[] ranges)
                => RangesAsList(ranges);

            protected static OneOf<List<PageRange>, bool> Expected(bool value)
                => value;
        }
    }
}