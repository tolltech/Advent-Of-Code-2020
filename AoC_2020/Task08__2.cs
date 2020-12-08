﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AoC_2020
{
    [TestFixture]
    public class Task08__2
    {
        public class Prog
        {
            public int Acc { get; private set; }

            private (string Op, int Arg)[] ops = Array.Empty<(string Op, int Arg)>();

            public Prog SetCode(string[] lines)
            {
                ops = lines
                    .Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries))
                    .Select(x => (Op: x[0], Arg: int.Parse(x[1])))
                    .ToArray();

                return this;
            }

            public void Fix()
            {
                var jmpTargets = new Dictionary<int, List<int>>();
                for (var i = 0; i < ops.Length; ++i)
                {
                    var op = ops[i];
                    if (op.Op != "jmp") continue;

                    var jnpTarget = i + op.Arg;
                    if (jmpTargets.TryGetValue(jnpTarget, out var from))
                    {
                        from.Add(i);
                    }
                    else
                    {
                        jmpTargets[jnpTarget] = new List<int> {i};
                    }
                }

                var nopTargets = new Dictionary<int, List<int>>();
                for (var i = 0; i < ops.Length; ++i)
                {
                    var op = ops[i];
                    if (op.Op != "nop") continue;

                    var jnpTarget = i + op.Arg;
                    if (nopTargets.TryGetValue(jnpTarget, out var from))
                    {
                        from.Add(i);
                    }
                    else
                    {
                        nopTargets[jnpTarget] = new List<int> {i};
                    }
                }

                var index = ops.Length - 1;
                while (true)
                {
                    var op = ops[index];
                }
            }

            public bool Execute()
            {
                var index = 0;
                var pastOps = new HashSet<(string Op, int Index)>();
                while (true)
                {
                    if (index >= ops.Length)
                    {
                        return true;
                    }

                    if (index < 0)
                    {
                        return false;
                    }

                    var op = ops[index];

                    var opHash = (op.Op, index);
                    if (pastOps.Contains(opHash))
                    {
                        return false;
                    }

                    pastOps.Add(opHash);

                    switch (op.Op)
                    {
                        case "acc":
                            Acc += op.Arg;
                            break;
                        case "jmp":
                            index += op.Arg;
                            continue;
                        case "nop": break;
                        default: throw new NotImplementedException("OpCode");
                    }

                    ++index;
                }
            }
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public int Task(string input)
        {
            var lines = input.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < lines.Length; ++i)
            {
                var copy = lines.ToArray();
                if (copy[i].Contains("jmp"))
                {
                    copy[i] = copy[i].Replace("jmp", "nop");
                }
                else if (copy[i].Contains("nop"))
                {
                    copy[i] = copy[i].Replace("nop", "jmp");
                }
                else
                {
                    continue;
                }

                var prog = new Prog();
                var result = prog.SetCode(copy).Execute();

                if (!result)
                {
                    continue;
                }

                return prog.Acc;
            }

            return -1;
        }

        private static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData(@"nop +0
acc +1
jmp +4
acc +3
jmp -3
acc -99
acc +1
jmp -4
acc +6").Returns(8);
            yield return new TestCaseData(@"jmp +1
acc -15
acc +14
acc +18
jmp +443
jmp +286
acc +27
jmp +522
jmp +1
acc -19
acc +22
acc +37
jmp +111
acc +28
acc +43
acc +18
nop +597
jmp +479
jmp +604
jmp +499
acc +0
acc +22
acc +13
jmp +566
acc -12
acc +0
nop +153
jmp +173
jmp +192
jmp +292
acc +36
acc +7
jmp +440
acc -17
acc +40
acc +24
acc -7
jmp +519
nop +16
acc +15
acc +42
jmp +445
jmp +350
acc +42
acc +12
acc +2
jmp +133
acc +12
acc +3
acc +27
jmp +186
acc +25
acc +46
jmp +285
acc +32
acc -11
acc -6
jmp +565
nop +215
acc +1
acc +35
jmp +1
jmp +502
acc +27
acc +19
acc -8
acc -8
jmp +531
jmp -21
nop +292
acc +8
acc -13
jmp +26
acc +1
acc +45
nop -42
jmp +323
jmp +39
jmp +336
acc +19
jmp -51
acc +45
acc +26
jmp +278
jmp +6
acc +40
nop +271
acc -10
nop -4
jmp +272
nop -61
acc +4
acc -14
acc +27
jmp -70
acc -9
acc +29
jmp +416
acc +25
acc +45
jmp +19
jmp +39
acc -19
acc +7
jmp +248
acc +11
acc +36
jmp +515
acc +45
acc +49
jmp +329
acc +30
acc +31
acc +28
acc +26
jmp +8
jmp +283
acc +32
jmp +127
acc +4
acc +20
jmp +92
jmp +50
jmp +133
acc +5
acc +8
jmp +313
acc +38
acc +34
jmp +395
acc +14
acc +29
jmp +392
nop +246
jmp +374
nop +429
nop +388
acc +3
acc +0
jmp +432
acc -1
acc +35
acc +35
jmp +148
acc +8
acc +11
acc +12
acc -10
jmp +434
acc -19
jmp +330
nop +329
acc +30
jmp +239
acc -6
jmp -136
jmp +418
nop +385
jmp +1
acc +34
acc +9
jmp +410
nop -13
acc +31
acc +15
acc +37
jmp -142
jmp +109
acc -16
nop +405
nop +343
jmp +8
acc +44
acc -15
acc +7
acc +9
jmp +185
acc +6
jmp +35
nop -25
jmp +93
acc +22
acc -17
acc +15
acc +39
jmp +41
nop -123
acc +15
acc +6
jmp -35
acc +48
jmp +422
acc -7
nop +67
nop +66
acc +48
jmp -29
acc -11
acc +16
jmp +92
acc +45
jmp +92
jmp +212
acc -3
acc -18
nop -186
nop +7
jmp -28
nop +292
acc +7
nop -120
acc +46
jmp +48
acc -3
acc -16
acc +50
jmp -44
acc -2
acc -11
jmp +236
jmp +344
acc +33
acc +44
acc +39
nop -45
jmp -53
acc -11
nop +380
acc +35
jmp +113
nop +203
acc +40
jmp +167
acc +44
jmp +394
jmp +229
jmp -167
jmp -204
acc +21
acc +49
jmp +25
acc -19
acc -17
acc +44
jmp -11
acc +40
acc +12
jmp +253
acc +21
jmp +349
jmp +285
acc +0
nop +261
acc +15
acc +38
jmp +10
acc +27
jmp +1
jmp +373
jmp -151
acc +6
jmp -48
acc +14
acc -8
jmp -61
acc +8
acc +20
jmp +1
jmp +1
jmp +208
acc -18
acc +32
jmp +94
jmp +262
acc +0
jmp -156
nop +188
nop +312
acc +21
acc +6
jmp -123
acc +47
jmp +316
acc +25
nop +290
jmp +62
acc -7
acc +36
nop +212
acc +14
jmp +332
jmp +291
jmp +226
acc +30
jmp -161
acc +39
acc +38
jmp +203
nop +63
nop -6
acc -15
nop -56
jmp +72
acc +1
acc +34
acc +22
acc +19
jmp -135
acc +27
jmp -303
acc +1
acc +48
acc -19
jmp +142
acc +50
jmp +298
acc +43
acc +0
acc +50
acc +12
jmp +137
acc +41
nop +252
jmp -310
acc +13
acc +34
acc -15
acc +43
jmp +236
acc +5
acc -8
acc +25
acc +45
jmp +153
acc -12
acc +31
acc -1
jmp +120
jmp +236
acc +38
nop -238
jmp -328
jmp +81
acc +48
acc +15
acc -9
jmp -73
nop -49
jmp -271
acc -17
acc -17
jmp +106
nop +212
jmp -290
acc +36
nop +109
jmp +186
jmp -310
acc +4
acc +16
jmp +117
jmp +1
acc +10
jmp +20
acc +12
jmp -311
acc +12
acc +30
nop +182
jmp -315
acc +25
acc +12
acc +30
jmp +50
acc -19
jmp -333
acc +30
nop +87
jmp -199
acc +8
jmp +112
acc -8
jmp -313
acc +7
acc +32
jmp +1
jmp +230
acc +25
acc +45
acc +20
acc +0
jmp -307
acc +30
nop -253
acc +7
acc +39
jmp -113
acc -12
jmp +209
acc +42
acc +17
acc -19
acc +24
jmp -170
acc +30
acc +9
acc -1
jmp -328
acc +19
acc +45
jmp +132
nop -244
nop +35
jmp +34
acc -10
acc +26
acc +35
nop -238
jmp +54
acc +15
nop -378
acc +42
jmp -43
acc -9
acc -5
acc -11
nop -307
jmp -129
nop -202
acc -9
nop -376
acc +11
jmp -75
jmp +14
acc -1
acc +32
acc -14
acc +16
jmp +39
acc +42
acc +32
jmp -133
acc +1
acc +17
nop +85
acc +35
jmp +83
acc +27
acc +0
acc -12
jmp -93
acc +48
acc +35
nop +154
jmp -287
jmp -347
jmp -348
acc +18
jmp -374
acc -15
jmp +36
jmp -123
acc -11
jmp +55
acc +19
acc +23
jmp -339
nop +5
acc +44
acc +2
jmp +1
jmp -417
acc +23
jmp -253
acc -9
acc -3
jmp -138
jmp -227
acc +12
jmp -437
acc +47
acc +19
acc -6
jmp -245
acc +2
jmp -328
acc -14
acc +25
acc +4
acc -2
jmp -411
jmp -351
jmp -459
acc +3
acc +48
jmp -134
nop +54
acc -14
jmp -298
jmp -401
acc -14
acc +25
nop -55
acc -10
jmp -312
acc -7
acc +45
jmp -74
acc +30
jmp -462
acc +5
acc -8
jmp -355
acc +9
acc +44
acc +44
jmp -150
jmp -484
acc +14
acc +19
acc -6
jmp -474
acc -18
jmp -166
jmp -264
acc -15
acc +17
acc +29
jmp -149
nop -273
acc +31
acc +0
acc -2
jmp -410
jmp -411
acc +47
acc -6
nop -287
jmp -436
acc +4
nop +88
jmp -158
acc +32
jmp +1
acc -15
jmp -319
acc -6
acc -18
acc +49
jmp -256
acc -18
acc +31
acc +27
acc +27
jmp -351
jmp +58
acc +12
jmp +1
acc +32
nop -151
jmp -411
acc +19
acc +7
jmp -287
acc +30
jmp -496
acc -11
acc +5
acc +42
acc +25
jmp -249
acc -1
jmp -243
jmp -190
acc +32
acc +32
acc +14
jmp +12
acc +5
acc +30
acc +34
jmp -46
acc -13
acc +5
acc +45
jmp -271
acc +29
acc +37
jmp -323
nop -18
acc -2
acc +21
acc -12
jmp -453
acc -14
acc +19
nop -173
jmp -411
acc +24
acc -7
nop -136
acc +6
jmp -357
acc -1
acc -1
acc +32
jmp -264
acc +26
jmp -175
acc +10
acc +35
nop -361
jmp -493
acc +14
jmp -206
jmp -138
acc -1
jmp -156
acc +3
acc +11
acc -2
jmp -213
acc +35
acc -13
acc +47
acc +45
jmp -376
jmp -543
jmp -479
acc +29
jmp -532
acc +28
acc +47
acc -11
acc -14
jmp +1").Returns(509);
        }
    }
}