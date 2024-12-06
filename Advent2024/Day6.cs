using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Advent2024 {
	internal class Day6 {
		public record Position(int X, int Y);

		private readonly char blocked = '#';
		private readonly char visited = 'X';
		private Direction facing;
		private readonly char[][] input;
		private char[][] currentMap;
		private readonly char[] indicators = ['^', '>', 'v', '<'];
		private Position currentPosition;
		private int xMax => input[0].Length;
		private int yMax => input.Length;
		private Position startingPosition;
		private Direction startingDirection;

		private enum Direction {
			North,
			East,
			South,
			West,
		}

		private Direction getStartingDirection(char indicator) =>
			indicator switch {
				'^' => Direction.North,
				'>' => Direction.East,
				'v' => Direction.South,
				'<' => Direction.West,
				_ => throw new Exception("invalid indicator")
			};

		private Position getStartingPosition() {
			var startingRow = input.First(line => line.Intersect(indicators).Any());
			var positionIndicator = startingRow.First(c => indicators.Contains(c));
			return new Position(Array.IndexOf([.. startingRow], positionIndicator), Array.IndexOf(input, startingRow));
		}

		public Day6() {
			//input = ["....#.....".ToCharArray(),
			//".........#".ToCharArray(),
			//"..........".ToCharArray(),
			//"..#.......".ToCharArray(),
			//".......#..".ToCharArray(),
			//"..........".ToCharArray(),
			//".#..^.....".ToCharArray(),
			//"........#.".ToCharArray(),
			//"#.........".ToCharArray(),
			//"......#...".ToCharArray()];
			input= File.ReadAllLines(".\\Inputs\\DaySix.txt").Select(line => line.ToCharArray()).ToArray();
			startingPosition = getStartingPosition();
			startingDirection = getStartingDirection(input[startingPosition.Y][startingPosition.X]);
		}

		public int PartOne() {
			resetGuardPatrol();
			while (inBounds(currentPosition)) {
				turnOrMove(currentPosition);
			}

			return currentMap.SelectMany(x => x).Count(x => x == visited);
		}

		private bool inBounds(Position position) =>
			position.X >= 0
			&& position.Y >= 0
			&& position.X < xMax
			&& position.Y < yMax;


		private void turnOrMove(Position position) {
			var next = nextPosition(position);

			if (isBlocked(next)) {
				facing = turn(facing);
			} else {
				currentMap[position.Y][position.X] = visited;
				currentPosition = next;
			}
		}

		private Direction turn(Direction facing) {
			return facing switch {
				Direction.North => Direction.East,
				Direction.East => Direction.South,
				Direction.South => Direction.West,
				Direction.West => Direction.North,
				_ => throw new Exception("invalid direction")
			};
		}

		private bool isBlocked(Position next) =>
			inBounds(next) && currentMap[next.Y][next.X] == '#';

		private Position nextPosition(Position position) {
			return facing switch {
				Direction.North => new(position.X, position.Y - 1),
				Direction.East => new(position.X + 1, position.Y),
				Direction.South => new(position.X, position.Y + 1),
				Direction.West => new(position.X - 1, position.Y),
				_ => throw new Exception("that's not a direction")
			};
		}

		public int PartTwo() {
			var originalPath = new List<Position>();

			resetGuardPatrol();
			while (inBounds(currentPosition)) {
				originalPath.Add(currentPosition);
				turnOrMove(currentPosition);
			}

			int waysToFoolGuard = 0;
			resetGuardPatrol();
			var possibleObstacleLocations = originalPath.Distinct().ToList();
			for (int i = 0; i < possibleObstacleLocations.Count(); i++) {
				var newObstacle = possibleObstacleLocations[i];
				if (newObstacle == startingPosition) { continue; }
				resetGuardPatrol();
				currentMap[newObstacle.Y][newObstacle.X] = blocked;
				var currentPath = new List<(Position pos, Direction facing)>();
				bool caughtInALoop;
				while (inBounds(currentPosition) {
					turnOrMove(currentPosition);
					caughtInALoop = currentPath.Any(x => x.pos == currentPosition && x.facing == facing);
					if (!inBounds(currentPosition) || caughtInALoop) { break; }
				}
				if (caughtInALoop) { waysToFoolGuard++; }

			}

			return waysToFoolGuard;
		}

		private void resetGuardPatrol() {
			currentMap = (char[][])input.Clone();
			facing = startingDirection;
			currentPosition = startingPosition;
		}
	}
}
