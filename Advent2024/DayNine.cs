namespace Advent2024;
static class DayNine {
	static readonly char[] input;

	static DayNine() {
		var useTestData = true;
		input = ReadInput(useTestData);
	}


	public static Answer Run() {
		var partOne = input.Unpack().Defragment().CalculateCheckSum();

		var partTwo = 0;

		return new Answer(partOne.ToString(), partTwo.ToString());

	}

	static char[] Unpack(this char[] input) {
		return input;
	}

	static char[] Defragment(this char[] input) {
		return input;
	}

	static int CalculateCheckSum(this char[] input) {
		return 0;
	}

	static char[] ReadInput(bool useTestData) =>
		useTestData
			? TestData.ToCharArray()
			: File.ReadAllText(".\\Inputs\\DayNine.txt").ToCharArray();

	static string TestData =>
		"""
        2333133121414131402
        """;
}
