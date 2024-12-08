using System.Text.RegularExpressions;

namespace Advent2024;
static class DayThree
{
    public static Answer Run()
    {
        var input = ReadInput();
        var pattern = new Regex(@"mul\(\d+,\d+\)|do\(|don't\(");

        var enabled = true;
        (List<Match> all, List<Match> enabled) answers = pattern.Matches(input)
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

        var partOne = answers.all
            .Select(Parse)
            .Select(Multiply)
            .Sum();

        var partTwo = answers.enabled
            .Select(Parse)
            .Select(Multiply)
            .Sum();

        return new Answer(partOne.ToString(), partTwo.ToString());
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
