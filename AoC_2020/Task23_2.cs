using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AoC_2020
{
    [TestFixture]
    public class Task23_2
    {
        class Cup
        {
            public int Label { get; set; }
            public Cup Next { get; set; }
        }

        class CircleList
        {
            public Cup Current { get; private set; }

            public int MaxLabel { get; }

            private readonly Dictionary<int, Cup> cupsByLabel = new Dictionary<int, Cup>();

            public CircleList(int[] cups)
            {
                Cup prevCup = null;

                MaxLabel = cups.Max();

                foreach (var cupLabel in cups)
                {
                    var currentCup = new Cup {Label = cupLabel};

                    cupsByLabel[cupLabel] = currentCup;
                    
                    if (prevCup != null)
                    {
                        prevCup.Next = currentCup;
                    }
                    else
                    {
                        Current = currentCup;
                    }

                    prevCup = currentCup;
                }

                prevCup.Next = Current;
            }

            private Cup[] GetPickedUp()
            {
                var cups = new List<Cup>();
                var current = Current;
                for (var i = 0; i < 3; ++i)
                {
                    current = current.Next;
                    cups.Add(current);
                }

                return cups.ToArray();
            }

            public void Do()
            {
                var pickedUp = GetPickedUp();
                var destination = FindDestination(pickedUp);

                Current.Next = pickedUp.Last().Next;
                pickedUp.Last().Next = destination.Next;
                destination.Next = pickedUp.First();

                Current = Current.Next;
            }

            private Cup FindDestination(Cup[] pickedUp)
            {
                var currentLabel = Current.Label;

                while (true)
                {
                    --currentLabel;

                    if (currentLabel == 0) break;

                    if (pickedUp.Any(x => x.Label == currentLabel)) continue;

                    return cupsByLabel[currentLabel];
                }

                currentLabel = MaxLabel + 1;

                while (true)
                {
                    --currentLabel;

                    if (pickedUp.Any(x => x.Label == currentLabel)) continue;

                    return cupsByLabel[currentLabel];
                }
            }

            public IEnumerable<Cup> Cups()
            {
                var current = Current;
                while (true)
                {
                    current = current.Next;
                    yield return current;
                }
            }
        }


        [Test]
        [TestCaseSource(nameof(TestCases))]
        public string Task(string input, int movesCount, int length)
        {
            var cups = input.Select(c => c.ToString()).Select(int.Parse).ToArray();

            cups = cups.Concat(Enumerable.Range(10, length - cups.Length)).ToArray();

            var circleList = new CircleList(cups);

            for (var move = 0; move < movesCount; ++move)
            {
                circleList.Do();
            }

            return string.Join(string.Empty,
                circleList.Cups().SkipWhile(x => x.Label != 1).Skip(1).Select(x => x.Label).Take(cups.Length - 1));
        }

        [Test]
        [TestCaseSource(nameof(BigTestCases))]
        public long Task2(string input, int movesCount, int length)
        {
            var cups = input.Select(c => c.ToString()).Select(int.Parse).ToArray();

            cups = cups.Concat(Enumerable.Range(10, length - cups.Length)).ToArray();

            var circleList = new CircleList(cups);

            for (var move = 0; move < movesCount; ++move)
            {
                circleList.Do();
            }

            var firstCups = circleList.Cups().SkipWhile(x => x.Label != 1).Skip(1).Take(2).Select(x => x.Label)
                .ToArray();
            return (long) firstCups[0] * firstCups[1];
        }

        private static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData(@"389125467", 10, 9).Returns("92658374");
            yield return new TestCaseData(@"389125467", 100, 9).Returns("67384529");
            yield return new TestCaseData(@"962713854", 100, 9).Returns("65432978");
        }

        private static IEnumerable<TestCaseData> BigTestCases()
        {
            yield return new TestCaseData(@"389125467", 10000000, 1000000).Returns(149245887792);
            yield return new TestCaseData(@"962713854", 10000000, 1000000).Returns(287230227046);
        }
    }
}