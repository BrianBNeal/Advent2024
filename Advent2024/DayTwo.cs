namespace Advent2024;
internal class DayTwo {
	public enum Direction {
		None,
		Increase,
		Decrease,
	}

	public int PartOne() {
		var reports = File.ReadAllLines(".\\Inputs\\DayTwo.txt");
		var safeReportCount = 0;

		foreach (var report in reports) {
			
			Direction? direction = null;
			var safe = true;
			var readingFirstLevel = true;
			var levels = report.Split(" ");

			for (int i = 0; i < levels.Length - 1; i++) {

				if (!safe) break;

				var first = int.Parse(levels[i]);
				var second = int.Parse(levels[i + 1]);

				if ((directionChanged(first, second, ref direction) && !readingFirstLevel) || diffIsOutOfBounds(first, second)) {
					safe = false;
				}
				readingFirstLevel = false;
			}

			if (safe) safeReportCount++;
		}

		return safeReportCount;
	}

	public int PartTwo() {
		var reports = File.ReadAllLines(".\\Inputs\\DayTwo.txt");
		var safeReportCount = 0;

		foreach (var report in reports) {

			Direction? direction = null;
			var safe = true;
			var readingFirstLevel = true;
			var levels = report.Split(" ");
			int? indexThatBroke = null;
			for (int i = 0; i < levels.Length - 1; i++) {

				if (!safe) break;

				var first = int.Parse(levels[i]);
				var second = int.Parse(levels[i + 1]);

				if ((directionChanged(first, second, ref direction) && !readingFirstLevel) || diffIsOutOfBounds(first, second)) {
					indexThatBroke = i;
					

					safe = false;
				}
				readingFirstLevel = false;
			}

			if (safe) safeReportCount++;
		}

		return safeReportCount;
	}

	private bool directionChanged(int first, int second, ref Direction? direction) {
		var newDirection = (first - second) switch {
			< 0 => Direction.Increase,
			0 => Direction.None,
			> 0 => Direction.Decrease,
		};
		var changed = newDirection == Direction.None
			|| newDirection != direction;
			
		direction = newDirection;
		return changed;
	}

	private bool diffIsOutOfBounds(int first, int second) {
		var diff = Math.Abs(first - second);
		return diff < 1 || diff > 3;
	}
}
