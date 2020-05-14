using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;

namespace XAMLAssetCreator.Core
{
    public static class PathGeometryFactory
    {
        private static readonly IEnumerable<char> CommandPrefixes = new[]
        {'M', 'L', 'F', 'H', 'V', 'C', 'Q', 'S', 'T', 'A'};

        public static SKPath FromString(string data)
        {
            data = data.ToUpper();
            var currentPos = 0;
            var end = data.Length;
            var commands = new List<string>();
            var currentChars = new List<char>();
            while (currentPos < end)
            {
                currentChars.Add(data[currentPos]);
                currentPos++;
                if (currentPos == end)
                {
                    break;
                }
                if (CommandPrefixes.Contains(data[currentPos]))
                {
                    commands.Add(new string(currentChars.ToArray()).Trim());
                    currentChars.Clear();
                }
            }
            commands.Add(new string(currentChars.ToArray()).Trim());
            var path = new SKPath();
            path.FillType = SKPathFillType.EvenOdd;
            foreach (var command in commands)
            {
                var commandKey = command[0];
                if (commandKey == 'F')
                {
                    path.FillType = command == "F1" ? SKPathFillType.Winding : SKPathFillType.EvenOdd;
                    continue;
                }
                var closes = command[command.Length - 1] == 'Z';
                var length = command.Length - 1;
                if (closes)
                    length--;
                var commandParams = command.Substring(1, length);
                var doubles = commandParams
                    .Split(new[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(float.Parse)
                    .Select((x, i) => new {Index = i, Value = x})
                    .GroupBy(x => x.Index/2)
                    .Select(x => x
                        .Select(v => v.Value)
                        .ToList())
                    .ToList();
                switch (commandKey)
                {
                    case 'M':
                        if (doubles.Count != 1)
                        {
                            throw new ArgumentException("wrong M parameters");
                        }
                        path.MoveTo(doubles[0][0], doubles[0][1]);
                        break;
                    case 'L':
                        foreach (var set in doubles)
                        {
                            path.LineTo(set[0], set[1]);
                        }
                        break;
                    case 'C':
                        var sixes = doubles
                            .Select((x, i) => new {Index = i, Value = x})
                            .GroupBy(x => x.Index/3)
                            .Select(x => x
                                .Select(v => v.Value)
                                .ToList())
                            .ToList();
                        foreach (var six in sixes)
                        {
                            path.CubicTo(six[0][0], six[0][1], six[1][0], six[1][1], six[2][0], six[2][1]);
                        }
                        break;
                    default:
                        throw new ArgumentException($"wrong paramter {commandKey}");
                }
            }
            return path;
        }
    }
}