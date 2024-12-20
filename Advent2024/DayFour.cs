﻿using System.Text.RegularExpressions;

namespace Advent2024;
static class DayFour
{
    static readonly string[] inputLines;
    static readonly int xMaxIndex;
    static readonly int yMaxIndex;

    static DayFour()
    {
        inputLines = ReadInput();
        xMaxIndex = inputLines[0].Length - 1;
        yMaxIndex = inputLines.Length - 1;
    }

    public static Answer Run()
    {
        var partOne = inputLines.GetPositions("X").Sum(XmasCountStemmingFromThisPosition);
        var partTwo = inputLines.GetPositions("A").Count(IsCenterOfMasX);

        return new Answer(partOne.ToString(), partTwo.ToString());
    }

    static string[] ReadInput(bool testMode = false) =>
        (testMode)
            ? TestData.Split(Environment.NewLine)
            : File.ReadAllLines(".\\Inputs\\DayFour.txt");

    static IEnumerable<Position> GetPositions(this IEnumerable<string> inputLines, string c) =>
        inputLines.Aggregate(
            (list: new List<Position>(), i: 0),
            (acc, line) =>
            {
                acc.list.AddRange(Regex.Matches(line, c).Select(x => new Position(x.Index, acc.i)));
                return acc;
            })
            .list;

    static int XmasCountStemmingFromThisPosition(this Position position)
    {
        (var x, var y) = position;
        var canGoRight = x + 3 < xMaxIndex;
        var canGoLeft = x - 3 >= 0;
        var canGoUp = y - 3 >= 0;
        var canGoDown = y + 3 < yMaxIndex;

        string[] words = [
            canGoRight ? $"{inputLines[y][x]}{inputLines[y][x+1]}{inputLines[y][x+2]}{inputLines[y][x+3]}" : "",
            canGoRight && canGoDown ? $"{inputLines[y][x]}{inputLines[y+1][x+1]}{inputLines[y+2][x+2]}{inputLines[y+3][x+3]}" : "",
            canGoDown ? $"{inputLines[y][x]}{inputLines[y+1][x]}{inputLines[y+2][x]}{inputLines[y+3][x]}" : "",
            canGoDown && canGoLeft ? $"{inputLines[y][x]}{inputLines[y+1][x-1]}{inputLines[y+2][x-2]}{inputLines[y+3][x-3]}" : "",
            canGoLeft ? $"{inputLines[y][x]}{inputLines[y][x-1]}{inputLines[y][x-2]}{inputLines[y][x-3]}" : "",
            canGoLeft && canGoUp ? $"{inputLines[y][x]}{inputLines[y-1][x-1]}{inputLines[y-2][x-2]}{inputLines[y-3][x-3]}" : "",
            canGoUp ? $"{inputLines[y][x]}{inputLines[y-1][x]}{inputLines[y-2][x]}{inputLines[y-3][x]}" : "",
            canGoUp && canGoRight ? $"{inputLines[y][x]}{inputLines[y-1][x+1]}{inputLines[y-2][x+2]}{inputLines[y-3][x+3]}" : "",
            ];

        return words.Count(w => w == "XMAS");
    }

    static bool IsCenterOfMasX(this Position position)
    {
        (var x, var y) = position;

        if (x == 0 ||
            y == 0 ||
            x >= xMaxIndex ||
            y >= yMaxIndex)
        { return false; }

        string[] words = [
            $"{inputLines[y-1][x-1]}{inputLines[y][x]}{inputLines[y+1][x+1]}",
            $"{inputLines[y-1][x+1]}{inputLines[y][x]}{inputLines[y+1][x-1]}",
            ];

        return words.All(word => word is "MAS" or "SAM");
    }

    static string TestData =>
        """
        MMMSXXMASM
        MSAMXMSMSA
        AMXSXMAAMM
        MSAMASMSMX
        XMASAMXAMM
        XXAMMXXAMA
        SMSMSASXSS
        SAXAMASAAA
        MAMMMXMMMM
        MXMXAXMASX
        """;
}
