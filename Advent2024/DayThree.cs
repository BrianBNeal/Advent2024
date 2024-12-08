using System.Text.RegularExpressions;

namespace Advent2024;
static class DayThree
{
    public static Answer Run()
    {
        var input = ReadInput();
        var pattern = new Regex(@"mul\(\d+,\d+\)");
        var matches = pattern.Matches(input);
        var partOne = matches
                        .Select(Parse)
                        .Select(Multiply)
                        .Sum();

        var partTwo = matches
                        .Select(Parse)
                        .Select(Multiply)
                        .Sum();

        return new Answer(partOne, partTwo);
    }

    static string ReadInput() =>
        File.ReadAllText(".\\Inputs\\DayThree.txt");

    static IEnumerable<int> Parse(this Match match) =>
        match.Value
            .Substring(4, match.Value.Length - 5)
            .Split(",")
            .Select(int.Parse);

    static int Multiply(this IEnumerable<int> nums) =>
        nums.First() * nums.Last();
}
