namespace Advent2024;

static class DayTwo
{
    public static Answer Run()
    {
        var reports = ReadInput();

        var partOne = reports.Count(IsValid);
        var partTwo = reports.Count(IsValidWithUpToOneRemoved);

        return new Answer(partOne.ToString(), partTwo.ToString());
    }

    static List<List<int>> ReadInput() =>
        File.ReadAllLines(".\\Inputs\\DayTwo.txt")
            .Select(line =>
                line.Split()
                    .Select(x => int.Parse(x))
                    .ToList())
            .ToList();

    static bool IsValidWithUpToOneRemoved(this List<int> values) =>
        new[] { values }
            .Concat(Enumerable.Range(0, values.Count)
                        .Select(i => values.Take(i)
                                        .Concat(values.Skip(i + 1))
                                        .ToList()))
            .Any(IsValid);

    static bool IsValid(this (int prev, int next) pair, int diffsign) =>
        Math.Abs(pair.next - pair.prev) >= 1 &&
        Math.Abs(pair.next - pair.prev) <= 3 &&
        Math.Sign(pair.next - pair.prev) == diffsign;

    static bool IsValid(this List<int> values) =>
        values.Count < 2 || values.IsValid(Math.Sign(values[1] - values[0]));

    static bool IsValid(this List<int> values, int diffSign) =>
        values.Pairs().All(pair => pair.IsValid(diffSign));

    //enumerator pattern!
    static IEnumerable<(int prev, int next)> Pairs(this IEnumerable<int> values)
    {
        using var enumerator = values.GetEnumerator();
        if (!enumerator.MoveNext())
            yield break;

        int prev = enumerator.Current;
        while (enumerator.MoveNext())
        {
            yield return (prev, enumerator.Current);
            prev = enumerator.Current;
        }

    }
}