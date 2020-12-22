using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AoC_2020
{
    [TestFixture]
    public class Task22_2
    {
        [Test]
        [TestCaseSource(nameof(TestCases))]
        public long Task(string input)
        {
            var players = input.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);

            var cards1 = players[0].Split(":", StringSplitOptions.RemoveEmptyEntries)[1].Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            var cards2 = players[1].Split(":", StringSplitOptions.RemoveEmptyEntries)[1].Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            var winner = PlayGame(cards1, cards2);

            return winner.WinnerDeck.Reverse().Select((x, i) => x * (i + 1)).Sum();
        }

        private static (int WinnerNumber, int[] WinnerDeck) PlayGame(int[] cards1, int[] cards2,  HashSet<string> snapshots = null)
        {
            snapshots ??= new HashSet<string>();
            var player1 = new Queue<int>(cards1);
            var player2 = new Queue<int>(cards2);

            while (player1.Count > 0 && player2.Count > 0)
            {
                var currentSnapshot = GetSnapshot(player1, player2);
                if (snapshots.Contains(currentSnapshot))
                {
                    return (1, player1.ToArray());
                }

                snapshots.Add(currentSnapshot);
                
                var card1 = player1.Dequeue();
                var card2 = player2.Dequeue();

                int winner = 0;
                if (card1 <= player1.Count && card2 <= player2.Count)
                {
                    winner = PlayGame(player1.Take(card1).ToArray(), player2.Take(card2).ToArray()).WinnerNumber;
                }
                else
                {
                    winner = card1 > card2 ? 1 : 2;
                }

                if (winner == 1)
                {
                    player1.Enqueue(card1);
                    player1.Enqueue(card2);
                }
                else
                {
                    player2.Enqueue(card2);
                    player2.Enqueue(card1);
                }
            }

            return player1.Count > 0 ? (1, player1.ToArray()) : (2, player2.ToArray());
        }

        private static string GetSnapshot(Queue<int> player1, Queue<int> player2)
        {
            return string.Join(",", player1) + "|" + string.Join(",", player2);
        }

        private static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData(@"Player 1:
9
2
6
3
1

Player 2:
5
8
4
7
10").Returns(291);
            yield return new TestCaseData(@"Player 1:
43
19

Player 2:
2
29
14").Returns(105);
            yield return new TestCaseData(@"Player 1:
29
21
38
30
25
7
2
36
16
44
20
12
45
4
31
34
33
42
50
14
39
37
11
43
18

Player 2:
32
24
10
41
13
3
6
5
9
8
48
49
46
17
22
35
1
19
23
28
40
26
47
15
27").Returns(33661);
        }
    }
}