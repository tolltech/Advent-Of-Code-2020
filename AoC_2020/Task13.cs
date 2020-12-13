using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AoC_2020
{
    [TestFixture]
    public class Task13
    {
        [Test]
        [TestCaseSource(nameof(TestCases))]
        public long Task(string input)
        {
            var lines = input.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

            var target = int.Parse(lines.First());
            var busIds = lines[1].Replace("x", string.Empty).Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            for (var currentId = target;; ++currentId)
            {
                foreach (var busId in busIds)
                {
                    if (currentId % busId == 0)
                    {
                        return busId * (currentId - target);
                    }
                }
            }

            return 0;
        }

        private static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData(@"939
7,13,x,x,59,x,31,19").Returns(295);
            yield return new TestCaseData(@"1000511
29,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,37,x,x,x,x,x,409,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,17,13,19,x,x,x,23,x,x,x,x,x,x,x,353,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,41")
                .Returns(222);
        }
    }
}