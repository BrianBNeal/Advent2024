namespace Advent2024;

internal static class DayTwo
{
    public static List<List<int>> ReadLists()
    {
        return File.ReadAllLines(".\\Inputs\\DayTwo.txt")
            .Select(line =>
                line.Split()
                    .Select(x => int.Parse(x))
                    .ToList())
            .ToList();
    }

    public static Answer Run()
    {
        var reports = ReadLists();

        var partOne = reports.Count(IsValid);
        var partTwo = reports.Count(IsValidWithUpToOneRemoved);

        return new Answer(partOne, partTwo);
    }

    private static bool IsValidWithUpToOneRemoved(this List<int> values) =>
        new[] { values }
            .Concat(Enumerable.Range(0, values.Count)
                        .Select(i => values.Take(i)
                                        .Concat(values.Skip(i + 1))
                                        .ToList()))
            .Any(IsValid);

    private static bool IsValid(this (int prev, int next) pair, int diffsign) =>
        Math.Abs(pair.next - pair.prev) >= 1 &&
        Math.Abs(pair.next - pair.prev) <= 3 &&
        Math.Sign(pair.next - pair.prev) == diffsign;

    private static bool IsValid(this List<int> values) =>
        values.Count < 2 || values.IsValid(Math.Sign(values[1] - values[0]));

    private static bool IsValid(this List<int> values, int diffSign) =>
        values.Pairs().All(pair => pair.IsValid(diffSign));

    private static IEnumerable<(int prev, int next)> Pairs(this IEnumerable<int> values)
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