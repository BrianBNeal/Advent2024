namespace Advent2024;
static class DayNine
{
    

    static readonly char[][] input;
    static readonly int yBound;
    static readonly int xBound;

    static DayNine()
    {
        var useTestData = false;
        input = ReadInput(useTestData);
        yBound = input.Length;
        xBound = input[0].Length;
    }

    record Data(Dictionary<char, List<Position>> dict, Position pos);
    record Antennae(Position pos, char freq);

    public static Answer Run()
    {
        var antennasByFrequency = input.MapAntennaPositions().GroupBy(a => a.freq).ToDictionary(g => g.Key, g => g.ToList());

        var partOne = antennasByFrequency
            .SelectMany(kvp =>
                kvp.Value
                    .SelectMany((A, i) => kvp.Value
                        .Skip(i + 1)
                        .SelectMany(B =>
                            new[] { new Position(2 * A.pos.x - B.pos.x, 2 * A.pos.y - B.pos.y), new Position(2 * B.pos.x - A.pos.x, 2 * B.pos.y - A.pos.y) })))
            .Where(p => p.IsInBounds())
            .Distinct()
            .Count();

        var partTwo = 0;

        return new Answer(partOne.ToString(), partTwo.ToString());

    }
    static IEnumerable<Antennae> MapAntennaPositions(this char[][] input) =>
        Enumerable.Range(0, yBound)
            .SelectMany(y => Enumerable.Range(0, xBound)
                .Select(x => new Antennae(new Position(x, y), input[y][x]))
                .Where(a => a.freq != '.'));

    static bool IsInBounds(this Position position) =>
        position.x >= 0 &&
        position.y >= 0 &&
        position.x < xBound &&
        position.y < yBound;

    static char[][] ReadInput(bool useTestData) =>
        useTestData
            ? TestData.Split(Environment.NewLine).Select(line => line.ToCharArray()).ToArray()
            : File.ReadAllLines(".\\Inputs\\DayEight.txt").Select(line => line.ToCharArray()).ToArray();

    static string TestData =>
        """
        ............
        ........0...
        .....0......
        .......0....
        ....0.......
        ......A.....
        ............
        ............
        ........A...
        .........A..
        ............
        ............
        """;
}
