using System.Text;

namespace Advent2024;
static class DayNine {
	static readonly string input;

	static DayNine() {
		var testData = true;
		input = ReadInput(testData);
	}


	public static Answer Run() {
		var partOne = input
			.ToIntegerArray()
			.Unpack()
			.Defragment()
			.CalculateCheckSum();

		var partTwo = input
			.ToIntegerArray()
			.Unpack()
			.DefragmentWhole()
			.CalculateCheckSumWithGaps();

		return new Answer(partOne.ToString(), partTwo.ToString());
	}

	static Dictionary<long, long> fileSizes = new();

	static long[] DefragmentWhole(this long[] input) {
		var map = input.ToArray();
		long start = 0;
		long end = map.Length - 1;

		while (start < end) {
			var first = map[start];
			var last = map[end];

			while (map[start] > -1L && start < end) {
				start++;
				first = map[start];
			}
			while (map[end] < 0L) {
				end--;
				last = map[end];
			}
			if (start <= end) {
				var fileSize = fileSizes[map[end]];
				var canFit = map.Skip((int)start).Take((int)fileSize).All(x => x < 0);
				if (canFit) {
					for (var i = 0; i < fileSize; i++) {
						map[start + i] = map[end - i];
						if (end > start) {
							map[end - i] = -1;
						}
					}
				} else {
					for (var i = 0; i < fileSize; i++) {
						end--;
					}
				}
			}
		}

		return map;
	}



	static long[] ToIntegerArray(this string input) =>
		input.Select(c => c == '.' ? -1 : long.Parse(c.ToString())).ToArray();

	static long[] Unpack(this long[] input) {
		var unpacked = new List<long>();
		for (int i = 0; i < input.Length; i++) {

			var val = (i % 2 == 0)
				? (i / 2)
				: -1;

			var fileId = (long)val;
			var fileSize = (long)input[i];
			for (int j = 0; j < fileSize; j++) {
				unpacked.Add(fileId);
			}
			fileSizes[fileId] = fileSize;
		}

		return [.. unpacked];
	}


	static long[] Defragment(this long[] input) {
		var map = input.ToArray();
		var start = 0;
		var end = map.Length - 1;

		while (start < end) {
			while (map[start] >= 0 && start < end) {
				start++;
			}
			while (map[end] < 0) {
				end--;
			}
			if (start <= end) {
				map[start] = map[end];
			}
			if (end > start) {
				map[end] = -1;
			}
		}

		return map;
	}

	static long CalculateCheckSum(this long[] input) {
		var nums = input.TakeWhile(c => c > -1).ToArray();
		long sum = 0;
		for (long i = 0; i < nums.Length; i++) {
			var num = nums[i];
			sum += num * i;
		}
		return sum;
	}

	static long CalculateCheckSumWithGaps(this long[] input) {
		long sum = 0;
		for (long i = 0; i < input.Length; i++) {
			var num = input[i];
			if (num > -1) {
				sum += num * i;
			}
		}
		return sum;
	}


	static string ReadInput(bool useTestData) =>
		useTestData
			? TestData
			: File.ReadAllText(".\\Inputs\\DayNine.txt");

	static string TestData =>
		"""
        2333133121414131402
        """;
}
