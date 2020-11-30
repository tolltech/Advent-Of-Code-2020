using FluentAssertions;
using NUnit.Framework;

namespace AoC_2020
{
    [TestFixture]
    public class Task01
    {
        [Test]
        [TestCase(1, 2, 3)]
        [TestCase(1, 3, 4)]
        [TestCase(1, 3, 5)]
        public void Task(int a, int b, int expected)
        {
            (a + b).Should().Be(expected);
        }
    }
}