﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace AoC_2020
{
    [TestFixture]
    public class Task19_2
    {
        [Test]
        [TestCaseSource(nameof(TestCases))]
        public long Task(string input)
        {
            var lines = input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

            var regexp = new Dictionary<int, (string RegEx, int[] OtherRules)>();
            var msgs = new List<string>();

            foreach (var line in lines)
            {
                if (line.Contains(":"))
                {
                    var splits = line.Split(":", StringSplitOptions.RemoveEmptyEntries);
                    regexp[int.Parse(splits[0])] = (RegEx: splits[1].Trim(),
                        OtherRules: splits[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)
                            .Where(x => int.TryParse(x, out _)).Select(int.Parse).ToArray());
                }
                else
                {
                    msgs.Add(line);
                }
            }

            var regEx42Str = GetRegEx(regexp[42], 42, regexp).Replace(" ", string.Empty);
            var regEx31Str = GetRegEx(regexp[31], 31, regexp).Replace(" ", string.Empty);

            var regex42 = new Regex($"^(?<g42>{regEx42Str})+");
            var regex31 = new Regex($"(?<g31>{regEx31Str})+$");

            return msgs.Count(x =>
            {
                var mathces42 = regex42.Matches(x).SingleOrDefault();
                var mathces31 = regex31.Matches(x).SingleOrDefault();

                if (mathces31 == null || mathces42 == null)
                {
                    return false;
                }

                if (mathces31.Groups["g31"].Captures.Count >= mathces42.Groups["g42"].Captures.Count)
                {
                    return false;
                }
                
                return mathces42.Value + mathces31.Value == x;
            });
        }

        private string GetRegEx((string RegEx, int[] OtherRules) rule, int ruleNumber,
            Dictionary<int, (string RegEx, int[] OtherRules)> rules)
        {
            if (rule.OtherRules.Length == 0)
            {
                return $"{rule.RegEx.Replace("\"", string.Empty)}";
            }

            var result = $" {rule.RegEx} ";
            foreach (var otherRule in rule.OtherRules.OrderByDescending(x => x))
            {
                if (otherRule == 8 && ruleNumber == 8)
                {
                    continue;
                }

                if (otherRule == 11 && ruleNumber == 11)
                {
                    continue;
                }

                var regEx = GetRegEx(rules[otherRule], otherRule, rules);
                result = result.Replace($" {otherRule} ",
                    $" {regEx} ");
            }
            
            result = $"({result})";
            return result;
        }

        private static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData(@"
1: ""a""
2: ""b""
3: 1 | 2 1
42: 1 1 | 2 2
31: 1 | 3
11: 42 11 31
8: 42 8
0: 8 11

aaaaa
aabbaaba
aaaabbaabbba
aaaabbaabbab
aaaabbaaaabbaabbabaaba").Returns(4);
            yield return new TestCaseData(@"42: 9 14 | 10 1
9: 14 27 | 1 26
10: 23 14 | 28 1
1: ""a""
11: 42 11 31
5: 1 14 | 15 1
19: 14 1 | 14 14
12: 24 14 | 19 1
16: 15 1 | 14 14
31: 14 17 | 1 13
6: 14 14 | 1 14
2: 1 24 | 14 4
0: 8 11
13: 14 3 | 1 12
15: 1 | 14
17: 14 2 | 1 7
23: 25 1 | 22 14
28: 16 1
4: 1 1
20: 14 14 | 1 15
3: 5 14 | 16 1
27: 1 6 | 14 18
14: ""b""
21: 14 1 | 1 14
25: 1 1 | 1 14
22: 14 14
8: 42 8
26: 14 22 | 1 20
18: 15 15
7: 14 5 | 1 21
24: 14 1

abbbbbabbbaaaababbaabbbbabababbbabbbbbbabaaaa
bbabbbbaabaabba
babbbbaabbbbbabbbbbbaabaaabaaa
aaabbbbbbaaaabaababaabababbabaaabbababababaaa
bbbbbbbaaaabbbbaaabbabaaa
bbbababbbbaaaaaaaabbababaaababaabab
ababaaaaaabaaab
ababaaaaabbbaba
baabbaaaabbaaaababbaababb
abbbbabbbbaaaababbbbbbaaaababb
aaaaabbaabaaaaababaa
aaaabbaaaabbaaa
aaaabbaabbaaaaaaabbbabbbaaabbaabaaa
babaaabbbaaabaababbaabababaaab
aabbbbbaabbbaaaaaabbbbbababaaaaabbaaabba").Returns(12);
            yield return new TestCaseData(@"62: 109 24 | 103 36
9: 75 36 | 67 24
66: 107 24 | 91 36
0: 8 11
44: 24 34 | 36 36
7: 36 24 | 24 36
74: 114 36 | 92 24
128: 36 103
52: 24 98 | 36 98
20: 108 24 | 26 36
75: 114 36 | 125 24
33: 24 125 | 36 98
110: 34 124
102: 24 78 | 36 97
27: 82 24 | 110 36
23: 36 100 | 24 3
43: 57 24 | 103 36
49: 24 5 | 36 61
85: 24 6 | 36 93
71: 60 36 | 90 24
55: 24 50 | 36 80
10: 24 65 | 36 64
16: 33 36 | 132 24
47: 48 36 | 112 24
125: 34 34
8: 42 8
14: 24 37 | 36 2
56: 97 36 | 15 24
25: 36 127 | 24 38
100: 24 114 | 36 68
101: 115 24 | 122 36
54: 36 98 | 24 92
60: 34 114
19: 24 36 | 36 36
113: 12 36 | 69 24
29: 109 24 | 125 36
59: 36 19 | 24 57
81: 24 109 | 36 124
119: 36 74 | 24 90
96: 36 76 | 24 4
109: 36 34 | 24 36
93: 36 36
11: 42 11 31
39: 17 24 | 129 36
132: 36 114 | 24 44
6: 36 34 | 24 24
127: 44 24 | 124 36
114: 24 24 | 24 36
73: 125 24
58: 36 51 | 24 40
21: 23 36 | 56 24
5: 98 36 | 19 24
105: 36 103 | 24 92
38: 93 36 | 92 24
82: 24 103 | 36 92
24: ""b""
15: 36 109 | 24 6
97: 36 44 | 24 7
50: 98 36 | 124 24
69: 98 36 | 57 24
63: 47 24 | 13 36
48: 24 120 | 36 116
17: 105 36 | 43 24
22: 94 36 | 66 24
95: 36 27 | 24 45
120: 24 103 | 36 125
57: 36 36 | 24 24
99: 24 101 | 36 119
53: 24 96 | 36 89
115: 36 98 | 24 114
103: 24 24
94: 24 83 | 36 79
111: 24 19 | 36 93
123: 36 114 | 24 98
18: 36 19
116: 36 125 | 24 93
79: 24 128 | 36 46
65: 58 24 | 77 36
45: 24 117 | 36 88
91: 24 52 | 36 110
90: 68 36 | 44 24
98: 24 36
106: 24 92
88: 24 103 | 36 6
84: 86 24 | 41 36
78: 24 44 | 36 125
26: 72 24 | 99 36
37: 128 24 | 54 36
130: 24 9 | 36 121
41: 36 32 | 24 81
122: 19 36 | 68 24
1: 24 19 | 36 92
28: 124 36 | 125 24
76: 24 92 | 36 93
77: 55 36 | 131 24
83: 24 28 | 36 73
86: 24 116 | 36 123
2: 104 36 | 33 24
92: 36 24
104: 98 24 | 109 36
32: 24 114 | 36 103
124: 36 36 | 36 24
61: 44 36 | 68 24
35: 36 63 | 24 87
129: 36 69 | 24 18
40: 24 29 | 36 30
31: 10 24 | 118 36
30: 109 24 | 6 36
117: 36 109 | 24 114
89: 36 52 | 24 54
108: 14 36 | 39 24
36: ""a""
112: 78 24 | 62 36
87: 24 53 | 36 95
51: 100 24 | 30 36
107: 1 24 | 85 36
34: 24 | 36
4: 24 68 | 36 109
12: 93 24 | 92 36
67: 36 93 | 24 109
46: 6 36 | 124 24
80: 24 57 | 36 68
13: 16 36 | 71 24
68: 36 24 | 24 34
42: 20 24 | 35 36
131: 24 74 | 36 111
3: 24 19
118: 70 24 | 22 36
64: 21 24 | 126 36
70: 130 24 | 84 36
72: 102 24 | 49 36
121: 106 24 | 59 36
126: 113 24 | 25 36

abbbabaaaabbbaaaaabbabbb
ababaaabaababbaaabbaabbababbbbbb
abaaaaaabbbaaaaabbbbbbbababaabab
bbabababbabaaabbabbababa
abaabaababaaaabbbabbbabaaaabbbbb
aabababbbabbbbaaaabbaaba
aabababbbaaaabbababbabba
bbabababbbabaaaababbbaaa
abaaabbbaaabbbbabaaaaaabbbbbbabb
bbbbbaabbabbaabaaabbbaaaabbaabbabbbabbbbababbbba
aaabababbabaaaabbbbaabababbaaabbbabababaaaaabaaaaaaaabbbabbbaaaaaaabbabbbbbaaabb
babbbbabaaaabbbabbbbaabb
babbaaabbbbbaaaababababababbabbaaabbbbaa
babbbbbabbaabbababaaabba
abbbbbbbaaabbaabbbaabababbbbaaabababbbabbabbabba
abbbbaaabaaaabbaaabbaaabbaabbbabbbbabaabbaabbababbbaabba
aababbabababbabaababbbba
babaaabbaabbabbabbbbbaaa
aaaaaababbbababaabbbbababbaaabbabaaaabab
baaaaaaaaaaabbaaabbbbbabbbbbaaaa
aabbaabaabaabbbbbbaaaabbbbbbabba
bbbbbbaabbbbbbbbbbabbaaa
abaabababbbbbbbabaabaaaaaaabbaabbaaabbbb
bababaaaaababaaaababaabbbaababbbbaaabaaaaababbbbbbaababbaaaabaaa
bbbbaaabbbabaaabababaaaaaaaaaabaabbbabba
bbabbbbbbabaaabaaabaaaabaaababbbbaaabbaa
aabaaababbbababaabaaabba
bababaababbbbaabbbbbbaabbbabaaabaabbabaaabaabbab
babaaaaababbbabbabaaabba
ababbabaababaabababaabbbbbaaabaababbabba
abbbbabababbaabaabbabaaababbaaaabbababbb
aabaaaaabbaaaaabbbbabbabbbbbabab
abababbaabbbaaababbbabbbbaaabbbbbaaabbbb
aaaaaababaabaaaaaaabbbbabbbbbbbbbaabbbbabaaabaaa
aabbbbabaabaaaaabbbaabbb
ababbabaaabbbbabbbbaaaab
aabbabaaabbbbbbabaaabbaa
abaabbbabbbabbababbbaaaaaabaaabababbbbaa
aabababbbabbbbaabaabbbbb
bbaaababaabbababbabaabbbbbbbabab
bbbbabbaaabbaabaaaaabaaaaabbaabbabababbabbbbaaaaaaaabbabbbbaabab
abaaaaaaabbbbaabbbbabaaa
ababaaabababaaabbbabbaaa
bbbbaaababbbabaaabbbabba
ababababaabababbababababaaaaaabababbabbbbabbbaaaabbaaabb
aababbaaaaababaabbabbaabbaaabbabaabbbbbbabababaabaaabaaa
bbbbaaaaaabbbbbbbbbabbabaaaaabbbbaabbaba
babbbbabbbbbbaabaabbbabb
aabbbbabababaaaababababa
bbbbbbbbabbaaaaababababb
bbabaaabbaabaaabbbbbaabbbaaaabbaaabbbbaaaabbabaabbaabaaaaaaaaaba
bbaabbaabababbbabababaaa
aaaabbaaabababaaaabbbaba
bbbbaaabbbaabbaaaaaabaabaaaaabbbaabbaabbbabababa
ababbabaabbbbbaaabaababaaabbabbabaaaaabbaaaaabbbbaaabbaa
baabababbabbaaababaaababbbaaababbbaababb
aaabbabaabbbaaaabaabaabb
aaababbbbbabababbabbbbabbbbbbabaaaabbbbbbabbabab
babaaabbbbbabababaabbaaa
abbabbababbbbaaaababbbba
bbaaabaabaaabababbbabbababaabbaaaaaabbaabbbbbaba
aaababbbababaabbbbaaabbb
abbaababaaabbbbabaababaaababaaaaabaaabbbaabbaaba
baaaababbbbbabbababaabbbaaabbaabbbbbabbbaabaabbbbaaaabaabaabbaba
aabaaaabaabbabababbbabaabbbabbbb
baaaaaaabbabababaabbbaaabbbbbbaaabbabbaabbaababa
baaaabbbabbbaaabbbabbbbbabaabbbb
babaabbabbbbaaabbbbabaabbbabbababbbbabab
aabbabbaaaaababaabaaabbbabaabbaaabababaa
bbbbbbbbabbaababaaaaaabaaabbabaababbbbbabbbaaabbbaabbabb
abababbaabaabaabbabaabab
bbabaabbbbabaaabbbaaabbb
abbbbabaabbababbbbbaaaab
aaabaaaaaabababbababaabaaaaaabbabbbababbaabbbabbaabbbbbbaabbbbab
abbbbbbaaabaaaabaaabbbab
ababaababaabbbbabaaababb
bbbbbbbaaaabaabababbbbabbbabababbabaabaabbaabaab
abbbbaabbbbbbbbaabbbabaaababbbbbbbaaaaba
bbbbbbbababbaaababbabaaabbbaaaaaaaabbbbababbabbabaabbabbbbabaaba
bbabbababbbbbaabbaaaaabb
bbaaababaabbabbabababaababbbbbbaabaaaabbbaababbbaaaaabba
aababbabbbbbaaaaaababaaaabaaaaaaabbbabaaaaabaaaa
aaaaababbbababaaaaaabbbabbaaabba
bbbbaaaaabbbbbbabbbaabab
aabaaaaabbabbababbaababbabbaabaabbaaaaabaaaaaabb
bbbaaaaaabbaabbabbabaaabaaabbbab
bbabababbbababaaababbaaa
bbaaabababbabbbbaaaaabaabaaababb
bababaabaababbabaabbbabb
aabbababababababbaabaaba
aaaaaabababaabbbaaabbaaa
abaababbaabaaaabbbabaaabababaabbbaabbbaababbbbbbaaabbabb
bbaaabaababaabbbabbaaaabbbaabaaabaabaaab
abbaaabbaaababaabaabbabb
abaababaaaabbababbaaaaabbaaabbbababaaaabaaaabbbbaaaababb
abbbaaaaaabaaabbbaabababbbbbabaabbbababb
babaaabbbbbbbbbbababaaababaabaabaaababaa
baaaaaaabbbabbabbabaabab
abbbababaabbaaabbaaabbaa
aababaaaabbaababaababaaaaababaaabbbbaabababaaababbbbbaaabaaabaaa
aaabbbaaaabbbbbbbaaaabbbbbabbbbbabbaababaabaabbaabbaaaba
babbbabbabaaababbaaaabbabbbabababaaaabbaaabaabab
baaaabaabbaaababbababbbabbaaaaabaaabbaaa
ababbabaaaabbbaababbbaab
abbabaababbabbbbabaabbaaabaabbaaaaaaabbb
abbabaabbaaabaaabaaabbaabaabaabbbbabbbbaabababbb
aaaaaabbabaabaabababababbbbaaaab
aaaabbabbababaabaabaaaaabbbaaaab
baaaabbaaabbbbbabaabbbabbabaaaaabaabbaab
aababbbaabaabaababbababbaabbbaab
bbbabbabaababbaaabbbabaabbabaaaa
abbaaaabaabaaaababaaabaa
ababaabbbaabbbabbabaaababbaabbba
abbbbabbbabaabbbbaaaaaaaabbbabbbbaaaabbbaaababaa
aabbaaaaaaaaabaabbabaaaabaabaaabababaabbabaababa
aabababbbaaabaabababbbbaabababbb
babaabbaababaababbabaaba
baaaabaabbbbbabaababaabbaabaababbbbaabbb
bbabaaababaaaababaabbaaa
bbababaabbbbbbaababaaabaaababaaaabbaaaba
bbaaabaabababbabbbbbaabbaaabaabb
abaabaabbabaaaaaabbaaaabbbabbababbbaaaab
abbabbbabaaaaaababbbbbbaaaabbbab
bbabababbbbbaaabbbabbbaa
aabababbbabbaaababbaaaaabaaababbaaaababb
aabaababbaaabbbbabbbbbbb
aabababbbabaabbbbabaabab
ababaaaaaabbabbabbbbaaaabababaaa
aabbbbbbabbaaaabbaaaabaaababbbbb
abaaaabaababaabbabbbbbab
aabbaaabbabbaaababaabaaabbbaabbb
abbbabababbabaabaaabbbbabbbaaaaababbaaab
baaabbababbabaaaababbbabbbaaaaba
baaaabaababbaaaabbbabaabbabaaaab
bbbbaaaaaaaabbbabbabbabababbabaabbabbabaababbaaaaaababaa
aaaaaabaabbbbaaabbbbaabb
abbaaaaaabaaababbbabbbab
bababbbbaaaabbabbbaabbbb
ababbabbbabaabaabbbbbaabaabbbaab
ababbabaabbaabaababbbbabababaabbabbbbbabbaaabbba
aaaaababbbbbbababaababababbbbbab
babbbbaaaaaaaabbbabbabba
aababbababbbaaabbbaababb
abaabbbaaabbbbbaabababbb
abbbaaabaabaabaabbabbaab
aabbbbabbbaaabaaaaabbbbbbaaaaaabababbaabaabbbbabbaaabbaabbabbababbabbbaabbaabbaaaaabaaba
babaaabbaaababbabbaabbba
abbbbabbbabbabaaaaabaabb
aaaaaabababbabaaababbbbb
abababbaabaababbbababbabababbbabbaabababbbabaaaabbbaabababbbbbbbbbaabbba
bbbbabbabbbaaaabaaaabbaaaaaabaaabbbabaaa
bbbbbbababbbbaaababbabaabaaabaaa
bbabaabbaaaaaabaabbbbbaabbbaaaba
aabbbbbabaaaabbbbaababaaaaaabbabbbbababaaababbabaaaababb
aaaaaabbabbbbababaabbbbabbabbababbabaabbabbbbbbb
abbbbababbbbbbaaaaabbaaa
bababaabaabababbbbaaaaabbaababba
abbaaaaaaaabbabbabbbaabaaabbabbabaabbaabababbbbbaababbaababbabba
aabbbbbbbbbbaaabbbabaaaa
babaaabbaabaabaabaaabbaa
abaaababaababbaaaabbbaabaabaaaabaabbbaaabbbbbbbbbbabbbabaabbaabbabbbbabb
aaaabbabaaaaababbabaaabbbbaabaaa
ababaababaaaaaabbaabbbbabbabbaab
abbbaaabbbbbbbbababaaabaaabbabaabbabaabbaabababbaaaabaabbaaabaab
aaabbbbaabbbbaaabaaabbababbbbabbbaaaaabb
abbaababbaaaabbbbbaabaaa
aabababbaabbabbabababaabbabbaaaaaaaababbaaaabbaa
bbbbaaaaabbabbbbabbbbababbababba
abbbaaababbaabbbbbbbaaaaabaaaababbbaaaabaaabbbabbabbaaabbbaabbab
bbaabbababbaaaaabbabaababbabbbba
aababbbaababaaabaaaababb
aaaababaaabaaaababbbbaabbaaaaababbabaaba
bbbbbbaababaaabbaaaabbabbabaabaababababb
aabaaabbbabaabbbbabbaabb
bbbabbabaabaaaabaabaaaababbaaaaaaababaaabaababbabbbbbabbbbbabbaababbbbbb
bbbbaabaabbbbaaabbbbaaba
bbbbbbababbbabaabaaaaabb
aabbabbaaaaaababaaababbbbbbbaaababbaabbabbbabaaa
abaaaaaaaaabbabaaababbababbbaaaaabababbbaaabaaaa
bbaaaaabbaaaaaaabaabaaaaaaaababaaaaaaabababaaaabbaaabbbaabaabbab
baaaabbaabbaaaaaababbabbababaabbbbabbabbbaabbabb
abbaabaaabaaababbaaaabbabaabbabababbbaaa
aabbbbbaaaabbaababbaabbb
aaabbbbabaaaabbbbbbbbaabaaababbbbbaababb
bbaaaaababaabbaaaabaabaaabbbaaabababbaab
abbbbaabbbbbaaaababababb
abbbaaababbaabbabbbbabbb
abbbabbbbbbbaabababaabbaabbaaaba
babaaaaaabbabbbbaaaaaaaa
abbbaaabaaababbbabbbbaabbabaabbaaabbaabaaaababaaaaaaabba
abbbaaaabaaaabbbaaaabaab
bbbbbbbaaabaaaaaaaabbbab
baaaaaabbaabababbaaaaaba
bbaabbababbbababbababaababbaabaababbbaba
abaaaabaabbabbbbbabbbbabbbbaaaaaaaaaabaa
babaaababbaabaabbabbabba
ababbbaabbbbbbabbaaabaaa
aaaabbbaabbabbabbaabbaba
abbbababababaababbbababaaabbbaabbbaaaabb
abaaababbaababaaaababbaabaaaabbaabbbabba
ababbabbaabbbbabbabbaabaaaabbaab
bbbbabaabababbbababbbabbaababaaabbbbbbabababbaaaaabbabbbbbaaaaba
bbbabaabbabbaabbbaabaababaabbbbbaaaaaaabbaaababb
aaaaaabaaabbababababaaaaaaaaabbabbbaaaba
bbabaabbbbaabaabbbaabbbbbaaaaabbbbbabaaa
aabbabababbabbaaaaaaaabbbabbbaaabaababbb
aababbaabbabababbaaaabaabbababbb
aabaabaaaaabaabaaaabbbab
abbabbaaaababbaaaabaabaaababababbabbaabbbaabaaabaababaab
baaaabbaabbababbaabaabab
aabaabaababaabbabbbaaababbbaaaab
abbaabaaaababbaabaababbb
abaaaababaabbbbaabbabaaabaaabbbbaababbbb
bbaaabaaaababaaaaaaababaabbbaabb
bbababaabbbabaabbbbabaaa
baababababaaabbbbaaaabbbababbbbb
abbbbabbbabbaaaaabbaabaaababaaabbbaabbbbaaaabaababbaabbb
bababaababbabbbabaabababaabbaaaa
abbababbaaabbaabaabbbbbbbabbaaaaabaabbaabbabababbaabbbaa
abaabbbaaaabbbaabaaaaaaabbbbaabbbaaaababbbaaabbb
aaababbabaaaaaaabaabaaba
aabbabbabababbbaaaabbbbaababababbaabbaabbbaaabba
aabbabaababbbbaabbabbbba
babbaaaabbbabababaaababa
abaaabbbaabaabababababbbababbbbaaaaaaaab
abbabbabbabbbabbbabababb
bbaaaaabbabaaabababbbaaa
bbaaababaaabbbbaabbbaaaaaabaabaabaaaaabbbaabbaaa
babbabbabbaaabaaabbbabababaaabba
aaaaababaabbababaaaaabba
bbaabbabaaaabbababbbbbbb
abaabaabbababaabbaaaabab
abaaaabaaaabbbaabbbbaaabababbabaabababbbaabbaaba
baabbbbabbbaaaaaabbbbababbabababbabaaaaaaaaaabbabbaaaabb
aaabbabaaabbabbabbabbaab
bbaaabbbbbabababaaaabaaaababbabbbabaaabaaaababaaaaabbbaa
abbbbabaabbaaaabbbbababb
abbbbabbbbabaaabbaabbbabbaababaabbabbbaa
babbabaaaabababbbbbbbababaabaaaabbabbaab
aabaaabaaabbabbaabaabaabaaaababb
babbabaaaaaaababbaababbaaabbaabbabbbaabbbbaabbaaaaabbbaa
aabbbbbbbbabaaabbbbbbaabaabbaabbbbbabaaa
aabbbbbbaabbbbbababaaaaababababbaabbaaaa
babaabbaababbabbbbaaaaba
aabaabaabbabaababaaaaabbaabababa
aabbbaaaabbaabaaaabaabababbbbbbaaabbaaabbbabaabbbbaabbbbbabbbaabbbabbabbaaabaaba
aabbaaabbbaaaabababaabaaaabbaabbabbababbabbbbbaabbbbbbaa
abbbbaaabbaabbaaabaabbaabbbababbbbaabbba
bbaabbbbaaabbbabbbbbbbabbaaabbbbaabbbaaaaababbaabbbbaabb
abbbaaabbababbbaaabbaaba
babbbbabababaabaaabaaabaaababbaababaaaabbabbbaab
bbbabbabbabaabbaabbbaaabaabbaaabaaaabbbbbaaababb
bbaabbabbabbbbaaabbabaaaabababaa
abbaababaaaaababbbaaaaba
baaaabbabbabbbbbaaabaabaabbabbbaabaaaabb
babaaabbbaaaaaabbaaaaabb
abbbbbbaabbabbbbaabaaabbabbababa
aaababbbaaaaabababbaaabb
aababbabaabaaaabbabbbbab
bbbbbabaabbbabababbababbaaaaaabbaabbbbaa
aaabbaababbabaaabbaabbaababaaaab
aabbabaabbbababaabbbbababbbabbab
abaabbaabaaaabbbbbaabbaaabbaaaabaabbaabb
baabaaaaabbabaaabbbaaabababbabab
baaaabbbbbbabaabbaaababa
abbabbababbbbbaaabaabbba
abbabbaaabaaaaabaabbbabbbbaaaaba
aabaabaaababaaaaabbbaabb
abbabbbabaaabbabbaabbbbb
aabbababaabaaabbaaababaa
bbabbababababbbaabbaaaaabababbbabbabaaaaababbbba
aaabbabaabaabbaabaabbbbb
bbbbbaabaaabbbaabbbbabaabbabaabbbbabbbaaabbaabbbbbaabbbb
abbabaabbbabaabbababbababbbbbabb
abaaaaaaababaabbaabbabbaaabbabbabbabbbaa
aaabbbbbaaaaabbbbbaaaababbaaabbabbababababbaaabaabbbabbbaabbbbaabbbaaababbbaaabbababaaab
aababbbbbbbaabaaaaabbbbababbabaabbbbabbaababbaabbbbbabba
abbabbaababaaaaaabaaaaab
abaabaabaabbababaabbaababaaababb
aaabbbbabaaabbabbaabababababaaabbbbbabbaaaaabaabbaaaaabb
abbbaaaabbaaaaababbabbabbbabaabbbbbaaabbaabbaaaa
abbababbaabbabaababbbabbbababaabaaabaabb
aababbaababbbabbaaaababb
abaaabbbbabaabbbabaabbbaaababbababbaabbbbbabaaaa
aaabbaabaabbbbabaabbabbaaaaabbbb
bbbbbbbbaaaaaabbbaaababb
aabbbaaabaabababaabbbbaa
aaaaaabbbbbbabaaaaabbababbaabbab
aabaaaabbabaabbabbaabaaa
bababbbbaabbaabbbbbbbabbbbaabbbbabbbaababaabaaababbbabababaabaaaaaabbaaabbaababa
baaaabbbbabbaaaaaabbbaab
bbbabbabaaaabbbaaaabbabb
abbbaaabbbbbbbabbabaabaaaaaabaabbaaaabab
bbbaabbaaabaabbbbbabbaab
bbabaabbbababbbabbaaaabb
aaaabbbaaaabbaabbbbbaaaaaababbabaaabbabb
abbaabaabaabbbbabababbbbaaabaabaabbaabbbbbbaaaabbbaaaaba
aabbbbbbaabaaaaaababbbbabbabaababababaab
babaaabbbabbaaabaababaaaaabbbbbbabaabbabaaaababbaababaab
bbbbaaababbbbbbaabaaaababbbbaaaababbbabaaaabaaaaaababbbb
bbbbbbbbaaabbbaaaaabbbab
aaaaababbaabbbababaaaabbabbaabbb
bbbbbbbbaaabbbaabaaabaaa
ababbabbabbabbaaabbbbabbabbbabbbbabbbbbb
baaaabbaaabbbbbbbbabaaba
bbbbbaababaabbbaabbbabba
aaabbbaabbabaaabbabbaabababaababaaaaaaabbaaaaababbaabbbb
abbaaaabababaaabbbbbbbbbbabaabab
bababbabaaaaaababbbabbaa
ababaabbbabaaabbbbbaabab
abaabaabbbbbbbbbbabbabaaaababaab
ababaabaaababbabbaaaaaba
abbaaaaaabaababbaababbabaaabbaabaabaabbbbbbaabaa
baaaabbbbbbbabaaabbbbababbaabbba
aabaaabaabaabaabaabbabbabbbbaabaabbbbabbbbaababb
bbbbbbababbbabaaabaaaaaaaaabbaabababaaaababbabbabaaabaaa
bbabbabaabaabaababbbabbbaabbbaaabaaaabbbbbaaaaaaabbbbbbb
babaabbaabbaaaaaaabbaaabbbbaaaaabaaababbbaabbaaa
abbbababbbaabbaaaaaabaaa
abaabaaababaaabbabaabaabbabaabbaabaabbbb
aabababbaaaabbbaababbbbb
abbaabababaaaabbbaaabbba
aaaababaabaabbaabaababaababbbaaa
abbbbaaabaaaabbbaabbbbabaabaaabbabbbbbbabbbabbba
aabbbbbbbabbabaabaabaaaababaaababbbabbba
baabbbbaababaaaaaabaaabbabbbaaba
abaaaabbabbaababbabbaaabbabbbbbb
bbbbaaaababbaabaaabbbbba
abbaabbaaaabbababaaaaaaaabbabbbaaabbbaababaaabbaaaababab
aaabaaaaabbbbbbbbabaabaaaaababaaaabbbabb
bbaaaaabaaabbbaaaaaaaaab
aabbbbbbaaabbaabbaaabaab
abbbbbbbbbbaabbbbabaabbbbabbababaababaaaaaaaaaaaaaabaaaa
aababbbaabaaabbbbaaaaaababababbb
baabababaabbbbbaaaaaaabbbaaabbaaababbaaaaaabbbbbbabbabababaabbabbbaaabba
babaaaaabbbbaabaabbbabbbbabababa
aaababbaababaabaabbbaaabbabababa
bbababaaabaaabbbbaaaabaaabbbbaababbababababbabbabbaaabbb
abbbababbabaabbbaaaabbbababaabaa
bbabbaaaaaabababaaababab
abaaaabbabbabaaaaabaabaaaaabbbbaabbabbaaabbbbbab
babbaaaaabaabababaaaabab
ababaaabbaabbbbbbaababbabbaabbababbabbbbbababbaababbbbba
aabbbbbabaaabbabababbabbbaaaaabb
abaaaaaabbaaaaabbabbaabb
abbabaababaabababababbbaabababbb
baaaabaaabbabbbaabbbaaabbaaabaaa
abbbbbabbbaaaabbabbbaabaaaabaaaabbabaaaaababaaba
aabbbbabbbaaabaabbababbabbbbaabaaaaababbbabbbbabbabbaabb
bbababaaababbbaaaabaabaabbbbabaaaabbbabb
abbbbbaaaabaabaababaabab
abbbaaaaabbaabbabaabbbaa
aaaaabababababbaaabaaaaaaabaaababbabaaabbababbaaabbbbbabaabbaaaa
abaabaabbbaaaaabaabbabaaaaabbbbbababaaaaabaabbbabaaabababbbaabaaaabbbbaaaababaababaabbaa
babbbbaabbabbabbbbbbbaaabaaababa
ababbabbbbbabbabbaabaaba
bbbabaababbaaaaaabaabaaaabaaabbbbbbaabba
bbbbbbbbbabaabbababaabaa
aaaabababababbababbaaabb
ababaabbbabaaababaababba
aababbaaababaabbbabaaaab
babbaababbbbaaabbbbababa
ababbabababbbbbaabbbabaabababbbbbbbaaabb
baabbbabaaabbaabbbbabbbb
aabbbbaaaaaaaabbabbaaaaababbbabbbababbbbabaabbba
baaaabaaaaaaaababaabbbbabbbabbbabababbaa
aababbaaaaaabababbaaaaabbbabbabaabbaaabbaaaabaab
ababaabaabaaabbbababbaab
bbaabbaaaaaababbabbaaabbaabbababababaabaabbaabaaaaabababaabbabbb
bbaaababbbabababbbbbaaaabbaabbbb
bababaabbabaaababbaabaab
baaaaaaababbbbbabbbbbabb
aababbabbbbbbbababababbb
bbbbbbababaaaabbbbaaaabb
babaaaaaababaabbbabaababbbabbbaabaaabbaababaabaababbbaba
abbbabababbbbbbababaabab
aabaabaabaaaabbabbaabbabbbabbaaaababbbbb
abaababbababaababaababba
abaaaabbbaababbabbbababbabbabababbbbbbbbaabbaaababbbababbbbabbaabaabbabaabbabbbb
abaaabbbbaabababaaaaaabababbbbbabbbababaabbbaaaababaabab
bbaabbaababaaabaaabaaabb
abbabaabbabaaaaaabbbbbbaaabbababbbbaaaab
aabbbbaaabbaababaaaaaabaaaababbabababbaabbaaaabaaaaababbabbbbbabbbabaabaababbbabbabbabaa
ababaaaaababaabbbababbbbbabbabaabbbabbabababbaabbaabaaabbbbbaabbbaabbbbbaaaaabaabbbbbabb
abbbbababbbbaaabaabbbbabaaabbaaabbbbbaaa
babbbbaababbbbbabbbbbabaabbababbababbaab
abbbbabbaababbaabbbbbbaaaaabaaaaaabbbbaa
aaababbbbaaaaabaaaabbbbaaabbbbaaabbaaaaaabbbbbbabbbbbbaabababbba
ababaaababaabababbbaaaaabbaaabbb
abaabbaabaaabbbbbbabbaabbbaaabba
ababaabbbaabaaaaababaaababababaabbabbbba
bbaaababaaaabbababbbbabaababababbbbaaabb
aababbaaaabbbbabbabaaababbbbabba
bbabaabbaababbbababaabbaabbbbbaababbbaba
aaabbaabbaabaaaaabbaaaba
abbaabbabaabababbbaaaaba
aaaabbabaababaaaabaaaabaaabbbbbbbabbabaabbbbbbabbabbbaaaabbaaaba
aaaababaabaabababaabaaab
babaaabbabaababaabaabbaaaaababbbbbbaaababaabbaab
aababbaabbbabbabbaaababa
bbbbbbbaabaabbbabababaabbabbbaabbbaabaab
bababbabbbbbaaabbaabaaba
aaaaabababbabaaaaaaaaaab
babbbbabaabbbababbbbaabaabaabbaaaaabaabaaaaababbbabaaabaababbaaabaaabbaaaabbbbaaaaabbbabbbaaaaba
aabbababaabaaaabbabbbbaabbbbbbabaabaabbaaaababaa
ababbabbabaaaabaaaabaaab
abbabbbbabaabbbaababbababbbbabbbaaabbabb
ababaaaabaabaababaaabbbaaabababbabbbabaaaabaaaaaaaaaabbb
baabbbabaaaababbaaaabbbbbbbbabba
bbabababbaaaabbbbabbaaabaabbaaba
babbbbabbbababaabbaaabbb
abbbabaaabbaaaabbbbbbbabababbbabababbaab
bbaaaaabbaaaaaabbbabbbaa
abbabbababbbbbbaabbabbabbabbbabbabaabaaaaaabbbabbbabaaba
ababaaaaaaaabaababbabaababaaabbbababbabbababababbbaaaaaabaababba
bbababaabaaaabaaabaaababababbbabaaabbaabbaaaaaabbaabbaaa
abbbaaabaaababbabaabaaaabaaabaaa
abbaaaabbaaaabbbbbbbbabaabbaaaaaabbabaabbbaaabbbbbaababa
bbaaaabaaabaababaabbaabbbbabbbbabbaabaab
baababaaabbbbbaaaabaaaabaaababaa
bbabbababbbbbbbbaaabbbab
abaababbabababaabbbabaaaabbaaababaaabaab
bbabbbbababaaaababbbaabbabbbaabb
bbbaaaaaabaaaabbbbabbabb
aaaaababaaaabbabbbbbbbabaabbababbbbaaaab
ababbbaabaababaabaaabaab
aabaaaaababbbbaaabbbaabb
bbbbbbbbaaabbbbbaaaabbbabaababbbabbaababaaaaaababbbaaabbbabbabbbbabaaaab
bbbbbbbbaabaaaababaaabababbbaaba
baabaaabaaababaaabbbbababbbababaaababbbaaaaabbbbaabbaaab
bababaababbababbbbbbbabababaabbbbaabbabbaaaababb
bbbabbababaabaaaaaaababaaabaabbabaabbbbb
ababbbabbbbbaababbaabbaababaaaaabaaabababbabaaaa
abbabbbbbaaaaaaaaaabbabb
abababbaababaaaaababbbaaabaabbaabbaaaaba
bbaaaaababbaabaabbbaaaba
baababaaaabaaabbbbaabaab
babbbbabaaaabbbaabbaabaabbabababaaabbbbb
babbbbaaabbbababaabbaaabbabbaabb
bbabaabbabaabaaabbabbbba
babbabaaabbbbbaaaabbbabb
aababbbabbaabbabababbbbb
aabaaaabbaabababaabababbabbbbaababbbbbaabaaabaaa
aabbbaaaabbabbbaabaaababbababbbababababababaaaab
abbbbbaabbaaabababbbabbb
abbaaaaaabaabbaabbbbaababaaabbbbbbbbbababbbbbbaabaaaaabababbbbbbbbaabbaaaaaaaaabbabaaaab
bbaaabaabbaaaaabaabbaabb
aababbbabaaabbabbbbaabba
abaabababbbbaaaabababbbabbbababaabbbababbabbaaabaaabbbab
baaaabbabbbbbaabbaababaaaaaaaabaaabbbbaabbbaaaabaaababab
abbababbbbbbaababaabbbababbaabbaabababbabbabaaaababbabbb
bbabbbbbbabbabaaabababbb
aaaabbabbabaaaaaaaaaababbabbabbb
aabaaaabbbabaabbababaabaaaaaaaaa
bbbbbaababbbabbbabbaaaabbbbbbbbaaabaabaa
aaaabbbaabbabbbbabbaaaaababbabaabbbbabaabbbabbaabbbbaabb
baaaaaaabbbaaaaaaaababbbbbaaabbbabaabbbb
aabbbbbbbaaabbababbaaaba
bbabaababbbababbaaabbaaa
bbbbabbaaabbbbaaaaaaabbaaaabbbaaabaababaabababbbbbabbbabbbbbabbb
abbaaaabaabbbaaabbbbabaaaaaabaabbabbaabb").Returns(363);
        }
    }
}