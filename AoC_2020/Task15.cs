using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AoC_2020
{
    [TestFixture]
    public class Task15
    {
        [Test]
        [TestCaseSource(nameof(TestCases))]
        public long Task(string input)
        {
            var numbers = input.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
            var lastIndices = numbers.Select(x => x).Reverse().Skip(1).Reverse().Select((x, i) => (x, i))
                .ToDictionary(x => x.x, x => x.i);

            for (var i = numbers.Count; i < 2020; ++i)
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
            yield return new TestCaseData(@"0,3,6").Returns(436);
            yield return new TestCaseData(@"1,3,2").Returns(1);
            yield return new TestCaseData(@"2,1,3").Returns(10);
            yield return new TestCaseData(@"1,2,3").Returns(27);
            yield return new TestCaseData(@"2,3,1").Returns(78);
            yield return new TestCaseData(@"3,2,1").Returns(438);
            yield return new TestCaseData(@"3,1,2").Returns(1836);
            yield return new TestCaseData(@"18,11,9,0,5,1").Returns(959);
        }
    }
}