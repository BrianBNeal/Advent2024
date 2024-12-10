namespace Advent2024;
static class DayEight {

	record Data(Dictionary<char, List<Position>> dict, Position pos);
	record Antenna(Position pos, char freq);

	public static Answer Run() {
		var useTestData = true;
		var input = ReadInput(useTestData);
		//var antennasByFrequency = input.MapAntennaPositions().GroupBy(a => a.freq).ToDictionary(g => g.Key, g => g.ToList());

		//var partOne = antennasByFrequency
		//    .SelectMany(kvp =>
		//        kvp.Value
		//            .SelectMany((A, i) => kvp.Value
		//                .Skip(i + 1)
		//                .SelectMany(B =>
		//                    new[] { new Position(2 * A.pos.x - B.pos.x, 2 * A.pos.y - B.pos.y), new Position(2 * B.pos.x - A.pos.x, 2 * B.pos.y - A.pos.y) })))
		//    .Where(p => p.IsInBounds())
		//    .Distinct()
		//    .Count();

		var antennas = input.MapAntennaPositions().GroupBy(a => a.freq,(freq, list) => list.Select(a => a.pos).ToList());

		var partOne = antennas
			.SelectMany(tower => tower.GetAntinodes(intput, Antinodes));
		var partTwo = 0;

		return new Answer(partOne.ToString(), partTwo.ToString());

	}
	static IEnumerable<Antenna> MapAntennaPositions(this char[][] input) =>
		input
			.SelectMany((row, yIndex) =>
				row.Select((freq, xIndex) => (xIndex, yIndex, freq))
				.Where(a => a.freq != '.')
				.Select(tower => new Antenna(new(tower.xIndex, tower.yIndex),tower.freq)));

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
