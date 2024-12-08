using System.Text.RegularExpressions;

namespace Advent2024;
static class DayThree
{
    public static Answer Run()
    {
        var input = ReadInput();
        var mulPattern = new Regex(@"mul\(\d+,\d+\)");
        var doPattern = new Regex(@"do\(|don't\(");
        var funcCalls = mulPattern.Matches(input);

        var enabled = true;
        var answers = funcCalls
                        .Concat(doPattern.Matches(input))
                        .OrderBy(x => x.Index)
                        .Aggregate((new List<Match>(), new List<Match>()), ((List<Match> all, List<Match> enabled) values, Match match) =>
                        {
                            switch (match.Value)
                            {
                                case "do(":
                                    enabled = true;
                                    break;
                                case "don't(":
                                    enabled = false;
                                    break;
                                default:
                                    values.all.Add(match);
                                    if (enabled)
                                    { values.enabled.Add(match); }
                                    break;
                            }
                            return values;
                        });
        var partOne = answers.Item1
            .Select(Parse)
            .Select(Multiply)
            .Sum();

        var partTwo = answers.Item2
            .Select(Parse)
            .Select(Multiply)
            .Sum();
        return new Answer(partOne, partTwo);
    }

    static bool IsEnabled(this IEnumerable<Match> values)
    {


        return values.Any();
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
