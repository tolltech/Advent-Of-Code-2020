using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AoC_2020
{
    [TestFixture]
    public class Task13_2
    {
        [Test]
        [TestCaseSource(nameof(TestCases))]
        public long Task(string input)
        {
            var lines = input.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

            var buses = lines.Single().Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select((x, i) => long.TryParse(x, out var busId) ? (BusId: busId, Index: i) : default)
                .Where(x => x != default)
                .ToArray();

            var firstBus = buses.First();

            var noks = new List<(long FirstTimestamp, long Period)>();
            foreach (var bus in buses.Skip(1))
            {
                var firstTimestamp = NOK(firstBus.BusId, bus.BusId, bus.Index);
                var period = NOK(firstBus.BusId, bus.BusId);
                noks.Add((firstTimestamp, period));
            }

            return 0;
        }

        static long NOD(long a, long b)
        {
            if (b < 0)
                b = -b;
            if (a < 0)
                a = -a;
            while (b > 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }

        static long NOK(long a, long b)
        {
            return Math.Abs(a * b) / NOD(a, b);
        }

        static long NOK(long a, long b, long delta)
        {
            var ak = 1;
            var bk = 1;
            while (true)
            {
                var an = ak * a;
                var bn = bk * b - delta;
                if (bn < 0)
                {
                    ++bk;
                    continue;
                }

                if (an == bn)
                {
                    return an;
                }
                else if (an < bn)
                {
                    ++ak;
                }
                else
                {
                    ++bk;
                }
            }
        }

        long NOK(long[] longs)
        {
            var nok = NOK(longs[0], longs[1]);
            for (var i = 2; i < longs.Length; ++i)
            {
                nok = NOK(nok, longs[i]);
            }

            return nok;
        }

        [Test]
        [TestCase(3, 5, 0, ExpectedResult = 15)]
        [TestCase(3, 5, 1, ExpectedResult = 9)]
        [TestCase(3, 5, 2, ExpectedResult = 3)]
        [TestCase(3, 5, 3, ExpectedResult = 12)]
        [TestCase(3, 5, 4, ExpectedResult = 6)]
        public long TestNokDelta(int a, int b, int delta)
        {
            return NOK(a, b, delta);
        }

        private static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData(@"7,13,x,x,59,x,31,19").Returns(1068781);
            yield return new TestCaseData(@"3,5").Returns(9);
            yield return new TestCaseData(@"3,4").Returns(3);
            yield return new TestCaseData(@"3,x,4").Returns(6);
            yield return new TestCaseData(@"3,5,4").Returns(54);
            yield return new TestCaseData(@"3,5,x,4").Returns(9);
            yield return new TestCaseData(@"17,x,13,19").Returns(3417);
            yield return new TestCaseData(@"17,x,13").Returns(0);
            yield return new TestCaseData(@"67,7,59,61").Returns(754018);
            yield return new TestCaseData(@"67,x,7,59,61").Returns(779210);
            yield return new TestCaseData(@"67,7,x,59,61").Returns(1261476);
            yield return new TestCaseData(@"1789,37,47,1889").Returns(1202161486);
            yield return new TestCaseData(
                    @"29,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,37,x,x,x,x,x,409,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,17,13,19,x,x,x,23,x,x,x,x,x,x,x,353,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,41")
                .Returns(0);
        }
    }
}