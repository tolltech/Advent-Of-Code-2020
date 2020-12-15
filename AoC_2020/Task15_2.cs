using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AoC_2020
{
    [TestFixture]
    public class Task15_2
    {
        [Test]
        [TestCaseSource(nameof(TestCases))]
        public long Task(string input)
        {
            var numbers = input.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
            var lastIndices = numbers.Select(x => x).Reverse().Skip(1).Reverse().Select((x, i) => (x, i))
                .ToDictionary(x => x.x, x => x.i);

            for (var i = numbers.Count; i < 30000000; ++i)
            {
                var prev = numbers[i - 1];

                var currentNumber = lastIndices.TryGetValue(prev, out var prevIndex) ? i - prevIndex - 1 : 0;

                numbers.Add(currentNumber);
                lastIndices[numbers[i - 1]] = i - 1;
            }

            return numbers.Last();
        }

        private static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData(@"0,3,6").Returns(175594);
            yield return new TestCaseData(@"1,3,2").Returns(2578);
            yield return new TestCaseData(@"2,1,3").Returns(3544142);
            yield return new TestCaseData(@"1,2,3").Returns(261214);
            yield return new TestCaseData(@"2,3,1").Returns(6895259);
            yield return new TestCaseData(@"3,2,1").Returns(18);
            yield return new TestCaseData(@"3,1,2").Returns(362);
            yield return new TestCaseData(@"18,11,9,0,5,1").Returns(116590);
        }
    }
}