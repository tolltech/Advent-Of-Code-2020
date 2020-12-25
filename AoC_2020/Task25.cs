using System.Collections.Generic;
using NUnit.Framework;

namespace AoC_2020
{
    [TestFixture]
    public class Task25
    {
        [Test]
        [TestCaseSource(nameof(TestCases))]
        public long Task(long cardPublicKey, long doorPublicKey, long subjectNumber)
        {
            var cardLoopSize = 0L;
            var doorLoopSize = 0L;
            var count = 0L;
            var currentValue = subjectNumber;
            while (cardLoopSize == 0 || doorLoopSize == 0)
            {
                ++count;
                currentValue *= subjectNumber;
                currentValue %= 20201227;

                if (currentValue == cardPublicKey)
                {
                    cardLoopSize = count;
                }

                if (currentValue == doorPublicKey)
                {
                    doorLoopSize = count;
                }
            }

            var doorKey = doorPublicKey;
            for (var i = 0; i < cardLoopSize; i++)
            {
                doorKey *= doorPublicKey;
                doorKey %= 20201227;
            }

            var cardKey = cardPublicKey;
            for (var i = 0; i < doorLoopSize; i++)
            {
                cardKey *= cardPublicKey;
                cardKey %= 20201227;
            }

            Assert.AreEqual(doorKey, cardKey);

            return doorKey;
        }

        private static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData(5764801, 17807724, 7).Returns(14897079);
            yield return new TestCaseData(17607508, 15065270, 7).Returns(12285001);
        }
    }
}