using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AoC_2020
{
    [TestFixture]
    public class Task10_2
    {
        [Test]
        [TestCaseSource(nameof(TestCases))]
        public long Task(string input)
        {
            var lines = input.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

            var numbers = lines.Select(int.Parse).Concat(new[] {0}).OrderBy(x => x).ToArray();

            var target = numbers.Last() + 3;

            numbers = numbers.Concat(new[] {target}).ToArray();

            var acc = 0L;
            var numberCnt = new Dictionary<int, long> {{target, 1}};

            for (var i = numbers.Length - 2; i >= 0; --i)
            {
                var current = numbers[i];

                var k = GetK(numberCnt, numbers, i);

                numberCnt[current] = k;
            }

            return numberCnt[0];
        }

        private long GetK(Dictionary<int, long> numberCnt, int[] numbers, int i)
        {
            var children = numbers.Skip(i + 1).Take(3).Where(x=> x - numbers[i] <= 3).ToArray();
            return children.Sum(x => numberCnt[x]);
        }

        private static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData(@"16
10
15
5
1
11
7
19
6
12
4").Returns(8);
            yield return new TestCaseData(@"28
33
18
42
31
14
46
20
48
47
24
23
49
45
19
38
39
11
1
32
25
35
8
17
7
9
4
2
34
10
3").Returns(19208);
            yield return new TestCaseData(@"47
61
131
15
98
123
32
6
137
111
25
28
107
20
99
36
2
97
88
124
138
75
112
52
122
78
46
110
41
64
63
16
93
104
105
91
27
45
119
14
1
65
62
118
37
79
77
19
71
35
130
69
5
44
9
48
125
136
103
140
53
126
106
55
129
139
87
68
21
85
76
31
113
12
100
24
96
82
13
70
72
86
26
117
58
132
114
40
54
133
51
92").Returns(12401793332096);
        }
    }
}