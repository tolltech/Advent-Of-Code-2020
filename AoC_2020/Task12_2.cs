﻿using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AoC_2020
{
    [TestFixture]
    public class Task12_2
    {
        [Test]
        [TestCaseSource(nameof(TestCases))]
        public long Task(string input)
        {
            var lines = input.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

            var ship = (X: 0L, Y: 0L);
            var wayPoint = (X: -1L, Y: -10L);
            //var quaterNumber = 0;

            foreach (var line in lines)
            {
                var command = line.Substring(0, 1);
                var arg = int.Parse(line.Substring(1, line.Length - 1));

                var turnCount = (arg / 90) % 4;
                switch (command)
                {
                    case "N":
                        wayPoint.X -= arg;
                        break;
                    case "S":
                        wayPoint.X += arg;
                        break;
                    case "E":
                        wayPoint.Y -= arg;
                        break;
                    case "W":
                        wayPoint.Y += arg;
                        break;
                    case "L":
                        wayPoint = TurnLeft(wayPoint, turnCount);
                        break;
                    case "R":
                        wayPoint = TurnRight(wayPoint, turnCount);
                        break;
                    case "F":
                        ship.X += wayPoint.X * arg;
                        ship.Y += wayPoint.Y * arg;
                        break;
                    default: throw new Exception();
                }

                var s = WriteWayPoint(ship, wayPoint);
            }

            return Math.Abs(ship.X) + Math.Abs(ship.Y);
        }

        private string WriteWayPoint((long X, long Y) ship, (long X, long Y) wayPoint)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"{wayPoint}");

            wayPoint = (wayPoint.X + ship.X, wayPoint.Y + ship.Y);

            for (var y = Math.Min(ship.Y, wayPoint.Y); y <= Math.Max(ship.Y, wayPoint.Y); ++y)
            {
                for (var x = Math.Min(ship.X, wayPoint.X); x <= Math.Max(ship.X, wayPoint.X); ++x)
                {
                    if (y == ship.Y && x == ship.X) sb.Append("S");
                    else if (y == wayPoint.Y && x == wayPoint.X) sb.Append("W");
                    else sb.Append(".");
                }

                sb.AppendLine();
            }

            sb.AppendLine($"{ship}");

            return sb.ToString();
        }

        private (long X, long Y) TurnRight((long X, long Y) point, int turnCount)
        {
            var x = (turnCount % 2 == 1)
                ? Math.Abs(point.Y)
                : Math.Abs(point.X);
            var y = (turnCount % 2 == 1)
                ? Math.Abs(point.X)
                : Math.Abs(point.Y);

            var quaterNumber =
                point.X > 0
                    ? point.Y > 0
                        ? 2
                        : 1
                    : point.Y > 0
                        ? 3
                        : 0;

            quaterNumber = (quaterNumber + turnCount) % 4;

            x = quaterNumber == 0 || quaterNumber == 3 ? -x : x;
            y = quaterNumber == 0 || quaterNumber == 1 ? -y : y;

            return (x, y);
        }

        private (long X, long Y) TurnLeft((long X, long Y) point, int turnCount)
        {
            turnCount = (4 - turnCount) % 4;

            return TurnRight(point, turnCount);
        }

        private static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData(@"F10
N3
F7
R90
F11").Returns(286);
            yield return new TestCaseData(@"F10
N3
W5
R180
R180
E5
L180
L180
L270
L90
R270
R90
R360
L360
F7
R90
F11").Returns(286);
            yield return new TestCaseData(@"F99
L180
F99
F97
R90
F97
R90
F97
R90
F97").Returns(0);
            yield return new TestCaseData(@"F99
E10
N1
W30
S3
F99
").Returns(0);
            yield return new TestCaseData(@"W12
F1
R180
F1
").Returns(0);
            yield return new TestCaseData(@"W12
F1
R90
F1
R270
F1
").Returns(7);
            yield return new TestCaseData(@"W3
R180
S1
F2
R90
W3
F81
L270
W5
F30
R90
E5
S3
F44
R180
S4
F21
R270
F2
W3
F36
L90
S1
E3
F86
S2
F98
E4
F93
W5
R90
W5
F4
E4
F6
R90
N2
L90
F52
R90
W4
F30
E5
W5
N4
F1
N3
W2
F89
E1
L90
W1
R90
F67
N5
R90
F95
L90
E4
S4
E2
N4
E4
R90
R90
F99
W1
R90
E4
F26
W5
L90
F89
E2
F18
W3
L90
F64
N2
F69
L90
F81
R90
E4
L90
N5
E2
F25
W4
L180
N2
R90
F82
R90
S3
F64
E4
F11
R90
F93
E2
F18
L90
R180
F13
L90
F37
L90
F63
R90
W3
N2
E2
F90
E5
L90
F1
W1
S5
W2
E5
S1
L90
E3
N4
L180
F26
E1
L90
W3
L90
S5
R90
F40
N3
W2
F85
E1
R270
F87
W2
F87
N3
E1
S4
E3
W3
F55
S1
E2
N3
L90
F44
W4
L90
F5
S5
F97
S3
R90
S2
E4
N3
F39
W2
F43
N1
W1
N2
E1
L180
N2
W4
S2
W1
R180
E1
N1
F5
W2
S2
L90
E4
L90
N3
W4
L90
E3
E1
S5
E2
L180
F86
W1
L90
W5
L90
E4
F63
W5
R180
F98
E3
F67
W4
E2
N4
L90
E3
L90
F97
L90
F21
N1
R90
F34
L90
L180
F30
E4
N2
R180
N1
R180
N4
F77
L180
N5
S1
F81
W1
N5
F77
S2
W2
S4
E2
F19
R90
R90
N2
F57
S4
L90
W3
R90
N5
W3
F16
L90
E4
W2
S3
L90
N5
R90
F93
S3
W5
R90
F80
N4
R90
N1
F95
S3
R90
F38
R90
F81
E2
S1
E1
L90
N5
W2
L90
N1
R90
E5
F79
S3
E5
N3
F92
N2
W4
S3
L90
F79
W4
L90
F46
S4
L90
S5
R270
F33
W5
L90
F46
W5
R90
S3
R180
W4
E4
R180
F87
N5
F63
S2
E2
F35
W4
F19
R90
S3
E3
R180
E4
F14
E5
L270
F35
L180
F74
R90
S4
W1
F36
R90
S2
F1
L90
S4
F55
E4
N1
L90
S3
N3
W5
S4
L180
W1
S3
F30
S3
E2
F25
S4
F29
L90
W1
L90
F60
W2
S3
W1
S2
R90
F82
S5
L90
E4
R90
W1
F92
L90
F35
W4
F94
L90
F56
F52
L180
E2
S5
W1
L180
N3
E2
E1
F15
L90
F49
L180
F57
L180
F86
S1
R180
N3
F59
W4
F17
R90
W3
F7
L180
N3
L90
E3
F57
L180
W4
S1
L90
S3
F7
E2
S5
F36
E3
F87
L90
L180
F58
R90
N1
L270
F24
N5
F3
F65
S2
R90
F62
L270
F94
E5
N2
F79
R90
E4
F43
W5
N3
F81
N5
R90
F22
W2
R180
N2
L90
E5
F39
E4
F8
W4
N4
W4
S4
L90
S1
L180
W3
L90
N1
R90
W4
R90
F7
W4
F40
E5
L90
R270
E4
R90
F34
W2
R90
N2
L90
F45
R90
E5
R90
N2
F11
L90
F80
S2
E2
R180
E2
L180
S1
F39
L180
N4
F99
R270
W4
L90
R90
W1
E1
S5
R90
F91
W4
F93
L180
E1
F15
N2
R90
W4
F26
S2
R90
N5
L90
N4
W4
S3
F25
E4
F38
L90
W1
F86
E5
L90
W5
L90
F21
W2
F99
L90
N4
L180
F40
S2
W4
F70
W3
F32
N1
W1
L90
N5
R90
W3
F99
W3
N2
R90
S3
E2
L90
N5
F36
S1
L180
W4
S5
E4
F54
L90
S5
E2
L90
F14
W1
N1
R90
S5
E4
F32
R90
N1
F23
E4
N4
W4
F57
N2
R180
E2
S4
F70
L90
W4
S3
F57
L90
E3
N5
R90
N4
W4
N5
W5
R90
W4
R90
E3
L90
N2
F93
L90
R180
S4
F28
E3
R90
S2
E3
R90
L90
F49
R180
F12
L180
N4
R180
S4
R90
S5
R90
F96
N4
F14
L90
S1
E2
L90
N3
R90
E2
W2
L180
N4
R90
W3
L90
N4
F69
E1
L90
S3
F62
R180
N3
F64
W1
S4
E1
L90
S3
F85
E2
L90
W3
R90
F92
N5
W2
N1
W1
R90
F1
R90
E2
R90
F34
N5
E2
N2
F79
L180
F63
L90
W2
F16
F81
S1
R180
F52
N4
W1
S3
R90
F58
E5
F99
W4
N3
L90
W4
F39
S2
F12
S1
W5
S2
F30
E2
F21
E2
N2
W5
L90
N1
W1
W3
N5
F65
S1
L180
F94
W1
L90
S2
E5
N3
F46
L180
S3
F1
L90
F77
N3
F36
N2
R90
F54
F65
N4
F99
E5
L180
E4
S3
E1
F83
E4
F50
N2
W2
R180
W5
R90
F96
L90
F7
E4
W2
F11
L180
N5
L180
S1
F92
S3
F3").Returns(28591);
        }
    }
}