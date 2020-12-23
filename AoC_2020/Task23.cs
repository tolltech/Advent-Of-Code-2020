using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AoC_2020
{
    [TestFixture]
    public class Task23
    {
        [Test]
        [TestCaseSource(nameof(TestCases))]
        public string Task(string input, int movesCount)
        {
            var cups = input.Select(c=>c.ToString()).Select(int.Parse).ToArray();

            var currentIndex = 0;

            for (var move = 0; move < movesCount; ++move)
            {
                var pickedUp = PickUp(cups, currentIndex, 3);
                var destination = FindDestination(cups, currentIndex);

                var currentLabel = cups[currentIndex];
                cups = ReplaceCups(cups, pickedUp, destination);

                currentIndex = (cups.Select((x, i) => (Index: i, Label: x)).First(x => x.Label == currentLabel).Index + 1) % cups.Length;
            }
            
            return string.Join(string.Empty, GetSnapshot(cups));
        }
        
        private static IEnumerable<int> GetSnapshot(int[] cups)
        {
            var index1 = cups.Select((x, i) => (Index: i, Label: x)).First(x => x.Label == 1).Index;

            for (var i = 0; i < cups.Length - 1; i++)
            {
                yield return cups[(index1 + i + 1) % cups.Length];
            }
        }

        private int[] ReplaceCups(int[] cups, int[] pickedUp, (int Index, int Label) destination)
        {
            var list = cups.ToList();
            list.InsertRange(destination.Index + 1, pickedUp);
            return list.Where(x => x != -1).ToArray();
        }

        private (int Index, int Label) FindDestination(int[] cups, int currentIndex)
        {
            var currentLabel = cups[currentIndex];
            while (--currentLabel > 0)
            {
                if (cups.Any(x => x == currentLabel))
                {
                    return cups.Select((x, i) => (Index: i, Label: x)).First(x => x.Label == currentLabel);
                }
            }

            return cups.Select((x, i) => (Index: i, Label: x)).OrderByDescending(x => x.Label).First();
        }

        private int[] PickUp(int[] cups, int current, int count)
        {
            var list = new List<int>();

            for (var i = 0; i < count; ++i)
            {
                var newI = i + current + 1;
                list.Add(cups[newI % cups.Length]);
                cups[newI % cups.Length] = -1;
            }

            return list.ToArray();
        }

        private static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData(@"389125467", 10).Returns("92658374");
            yield return new TestCaseData(@"389125467", 100).Returns("67384529");
            yield return new TestCaseData(@"962713854", 100).Returns("65432978");
        }
    }
}