using System.Text;

namespace Advent2024;
static class DayNine {
	static readonly string input;

	static DayNine() {
		var useTestData = true;
		input = ReadInput(useTestData);
	}


	public static Answer Run() {
		var partOne = input.Unpack().Defragment().CalculateCheckSum();

		var partTwo = 0;

		return new Answer(partOne.ToString(), partTwo.ToString());

	}

	static string Unpack(this string input) {
		var unpacked = new StringBuilder();
		for (int i = 0; i < input.Length; i++) {

			var c = (i % 2 == 0)
				? (i / 2).ToString()[0]
				: '.';
			var timesToRepeat = int.Parse(input.ToString().Substring(i, 1));

			unpacked.Append(new string(c, timesToRepeat));
		}

		return unpacked.ToString();
	}


	static char[] Defragment(this string input) {
		var map = input.ToArray();
		var start = 0;
		var end = map.Length - 1;

		while (start < end) {
			while (map[start] != '.') {
				start++;
			}
			while (map[end] == '.') {
				end--;
			}
			map[start] = map[end];
			map[end] = '.';
		}

		return map;
	}

	static int CalculateCheckSum(this char[] input) =>
		input.Aggregate((0,0), (acc, c) => {
			acc.Item1 += acc.Item2 * input[acc.Item2];
			acc.Item2++;
			return acc;
		}).Item1;

	static string ReadInput(bool useTestData) =>
		useTestData
			? TestData
			: File.ReadAllText(".\\Inputs\\DayNine.txt");

	static string TestData =>
		"""
        2333133121414131402
        """;
}
