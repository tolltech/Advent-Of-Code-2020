using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AoC_2020
{
    [TestFixture]
    public class Task17
    {
        [Test]
        [TestCaseSource(nameof(TestCases))]
        public long Task(string input)
        {
            var lines = input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

            var points = new Dictionary<int, Dictionary<int, Dictionary<int, char>>>();

            for (var y = 0; y < lines.Length; ++y)
            {
                for (var x = 0; x < lines[y].Length; ++x)
                {
                    NormalizeDIctionary(points, x, y);

                    points[x][y][0] = lines[y][x];
                }
            }

            var xRange = (From: 0, To: lines.Length - 1);
            var yRange = (From: 0, To: lines.Length - 1);
            var zRange = (From: 0, To: 0);

            for (var i = 0; i < 6; ++i)
            {
                var copy = points.ToDictionary(x => x.Key,
                    x => x.Value.ToDictionary(y => y.Key, y => y.Value.ToDictionary(z => z.Key, z => z.Value)));

                xRange.From -= 1;
                xRange.To += 1;

                yRange.From -= 1;
                yRange.To += 1;

                zRange.From -= 1;
                zRange.To += 1;

                for (var x = xRange.From; x <= xRange.To; ++x)
                for (var y = yRange.From; y <= yRange.To; ++y)
                for (var z = zRange.From; z <= zRange.To; ++z)
                {
                    NormalizeDIctionary(points, x, y);
                    NormalizeDIctionary(copy, x, y);

                    var point = points[x][y].TryGetValue(z, out var c) ? c : '.';

                    var activeNeighborsCount = GetActiveNeighbors(points, x, y, z);
                    switch (point)
                    {
                        case '#':
                            if (activeNeighborsCount != 2 && activeNeighborsCount != 3)
                            {
                                copy[x][y][z] = '.';
                            }

                            break;
                        case '.':
                            if (activeNeighborsCount == 3)
                            {
                                copy[x][y][z] = '#';
                            }

                            break;
                        default: throw new Exception("char");
                    }
                }

                points = copy;
            }

            return points.SelectMany(x => x.Value.SelectMany(y => y.Value.Select(z => z.Value))).Count(x => x == '#');
        }

        private static void NormalizeDIctionary(Dictionary<int, Dictionary<int, Dictionary<int, char>>> points, int x, int y)
        {
            if (!points.ContainsKey(x))
            {
                points[x] = new Dictionary<int, Dictionary<int, char>>();
            }

            if (!points[x].ContainsKey(y))
            {
                points[x][y] = new Dictionary<int, char>();
            }
        }

        private int GetActiveNeighbors(Dictionary<int, Dictionary<int, Dictionary<int, char>>> points, int x, int y,
            int z)
        {
            var result = 0;
            for (var xx = x - 1; xx <= x + 1; ++xx)
            for (var yy = y - 1; yy <= y + 1; ++yy)
            for (var zz = z - 1; zz <= z + 1; ++zz)
            {
                if (xx == x && yy == y && zz == z)
                {
                    continue;
                }

                var active = points.TryGetValue(xx, out var yd) && yd.TryGetValue(yy, out var zd) &&
                             zd.TryGetValue(zz, out var c) && c == '#';
                if (active)
                    ++result;
            }

            return result;
        }

        private static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData(@".#.
..#
###").Returns(112);
            yield return new TestCaseData(@"...#..#.
.....##.
##..##.#
#.#.##..
#..#.###
...##.#.
#..##..#
.#.#..#.").Returns(269);
        }
    }
}