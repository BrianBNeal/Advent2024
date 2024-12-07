namespace Advent2024
{
    internal class Day6
    {
        public record Position(int X, int Y);
        public record PatrolStance(Position position, Direction direction);

        private readonly char[][] startingMap;
        private readonly Position startingPosition;
        private readonly Direction startingDirection;

        private char[][] currentMap;
        private Position currentPosition;
        private Direction currentDirection;

        private const char blocked = '#';
        private const char visited = 'X';
        private char[] indicators = ['^', '>', 'v', '<'];
        private int xMax => startingMap[0].Length;
        private int yMax => startingMap.Length;

        public Day6()
        {
            //startingMap = """
            //     ....#.....
            //     .........#
            //     ..........
            //     ..#.......
            //     .......#..
            //     ..........
            //     .#..^.....
            //     ........#.
            //     #.........
            //     ......#...
            //     """
            //    .Split(Environment.NewLine).Select(line => line.ToCharArray()).ToArray();
            startingMap = File.ReadAllLines(".\\Inputs\\DaySix.txt").Select(line => line.ToCharArray()).ToArray();
            currentMap = startingMap.Select(x => x.ToArray()).ToArray();
            (startingPosition, startingDirection) = getStartingPositionAndDirection();
            currentPosition = startingPosition;
        }

        public int PartOne()
        {
            goToStartingPositions();
            while (inBounds(currentPosition))
            {
                (currentPosition, currentDirection) = turnOrMove(currentPosition, currentDirection);
            }

            return currentMap.SelectMany(x => x).Count(x => x == visited);
        }

        public int PartTwo()
        {
            var originalPath = new HashSet<Position>();

            goToStartingPositions();
            while (inBounds(currentPosition))
            {
                originalPath.Add(currentPosition);
                (currentPosition, currentDirection) = turnOrMove(currentPosition, currentDirection);
            }

            int waysToFoolGuard = 0;
            goToStartingPositions();
            var validObstacleLocations = originalPath.ToList();
            for (int i = 0; i < validObstacleLocations.Count; i++)
            {
                var obstacleLocation = validObstacleLocations[i];
                if (obstacleLocation == startingPosition)
                { continue; }
                goToStartingPositions();
                currentMap[obstacleLocation.Y][obstacleLocation.X] = blocked;
                var patrolledPath = new HashSet<PatrolStance>() { new(startingPosition, startingDirection) };
                bool caughtInALoop = false;
                while (inBounds(currentPosition))
                {
                    (currentPosition, currentDirection) = turnOrMove(currentPosition, currentDirection);
                    if (patrolledPath.Contains(new PatrolStance(currentPosition, currentDirection)))
                    { caughtInALoop = true; }
                    patrolledPath.Add(new(currentPosition, currentDirection));
                    if (!inBounds(currentPosition) || caughtInALoop)
                    { break; }
                }
                if (caughtInALoop)
                { waysToFoolGuard++; }
            }

            return waysToFoolGuard;
        }

        public enum Direction
        {
            North,
            East,
            South,
            West,
        }

        private Direction getStartingDirection(char indicator) =>
            indicator switch
            {
                '^' => Direction.North,
                '>' => Direction.East,
                'v' => Direction.South,
                '<' => Direction.West,
                _ => throw new Exception("invalid indicator")
            };

        private (Position pos, Direction facing) getStartingPositionAndDirection()
        {
            var startingRow = startingMap.First(line => line.Intersect(indicators).Any());
            var positionIndicator = startingRow.First(c => indicators.Contains(c));
            return (new Position(Array.IndexOf([.. startingRow], positionIndicator), Array.IndexOf(startingMap, startingRow)), getStartingDirection(positionIndicator));
        }

        private bool inBounds(Position position) =>
            position.X >= 0
            && position.Y >= 0
            && position.X < xMax
            && position.Y < yMax;


        private (Position position, Direction facing) turnOrMove(Position position, Direction direction)
        {
            var next = nextPosition(position);

            if (isBlocked(next))
            {
                return (position, turn(direction));
            }
            else
            {
                currentMap[position.Y][position.X] = visited;
                return (next, direction);
            }
        }

        private Direction turn(Direction facing)
        {
            return facing switch
            {
                Direction.North => Direction.East,
                Direction.East => Direction.South,
                Direction.South => Direction.West,
                Direction.West => Direction.North,
                _ => throw new Exception("invalid direction")
            };
        }

        private bool isBlocked(Position next) =>
            inBounds(next) && currentMap[next.Y][next.X] == '#';

        private Position nextPosition(Position position)
        {
            return currentDirection switch
            {
                Direction.North => new(position.X, position.Y - 1),
                Direction.East => new(position.X + 1, position.Y),
                Direction.South => new(position.X, position.Y + 1),
                Direction.West => new(position.X - 1, position.Y),
                _ => throw new Exception("that's not a direction")
            };
        }

        private void goToStartingPositions()
        {
            currentMap = startingMap.Select(x => x.ToArray()).ToArray();
            currentDirection = startingDirection;
            currentPosition = startingPosition;
        }
    }
}
