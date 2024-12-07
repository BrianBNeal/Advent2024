//namespace Advent2024
//{
//    public static class DaySix
//    {
//        record Position(int X, int Y);
//        record PatrolPosition(Position position, Direction direction);

//        const char blocked = '#';
//        const char visited = 'X';

//        static readonly PatrolPosition startingPosition;
//        static readonly char[][] startingMap;

//        static int yMax => startingMap.Length;
//        static int xMax => startingMap[0].Length;

//        static char[][] currentMap;
//        static private PatrolPosition currentPosition;

//        static DaySix()
//        {
//            startingMap = ReadLists();
//            startingPosition = getStartingPositionAndDirection();

//            currentMap = startingMap.Select(x => x.ToArray()).ToArray();
//            currentPosition = startingPosition;
//        }

//        public static Answer Run()
//        {
//            goToStartingPositions();

//            while (currentPosition.position.IsInBounds())
//            {
//                currentPosition.TurnOrMove();
//            }

//            var partOne = currentMap.SelectMany(x => x).Count(x => x == visited);

//            var originalPath = new HashSet<Position>();

//            goToStartingPositions();
//            while (currentPosition.position.IsInBounds())
//            {
//                originalPath.Add(currentPosition);
//                (currentPosition, currentDirection) = turnOrMove(currentPosition, currentDirection);
//            }

//            int waysToFoolGuard = 0;
//            var validObstacleLocations = originalPath.ToList();

//            for (int i = 0; i < validObstacleLocations.Count; i++)
//            {
//                var obstacleLocation = validObstacleLocations[i];
//                if (obstacleLocation == startingPosition)
//                { continue; }

//                goToStartingPositions();
//                currentMap[obstacleLocation.Y][obstacleLocation.X] = blocked;
//                var patrolledPath = new HashSet<PatrolPosition>() { new(startingPosition, startingDirection) };
//                bool caughtInALoop = false;

//                while (inBounds(currentPosition))
//                {
//                    (currentPosition, currentDirection) = turnOrMove(currentPosition, currentDirection);

//                    if (patrolledPath.Contains(new PatrolPosition(currentPosition, currentDirection)))
//                    { caughtInALoop = true; }

//                    patrolledPath.Add(new(currentPosition, currentDirection));

//                    if (!inBounds(currentPosition) || caughtInALoop)
//                    { break; }
//                }
//                if (caughtInALoop)
//                { waysToFoolGuard++; }
//            }

//            var partTwo = waysToFoolGuard;

//            return new(partOne, partTwo);
//        }

//        private static char[][] ReadLists() =>
//            File.ReadAllLines(".\\Inputs\\DaySix.txt").Select(line => line.ToCharArray()).ToArray();

//        public enum Direction
//        {
//            North,
//            East,
//            South,
//            West,
//        }

//        static Direction getStartingDirection(char indicator) =>
//            indicator switch
//            {
//                '^' => Direction.North,
//                '>' => Direction.East,
//                'v' => Direction.South,
//                '<' => Direction.West,
//                _ => throw new Exception("invalid indicator")
//            };

//        static PatrolPosition getStartingPositionAndDirection()
//        {
//            char[] indicators = ['^', '>', 'v', '<'];
//            var startingRow = startingMap.First(line => line.Intersect(indicators).Any());
//            var positionIndicator = startingRow.First(c => indicators.Contains(c));
//            return new(new Position(Array.IndexOf([.. startingRow], positionIndicator), Array.IndexOf(startingMap, startingRow)), getStartingDirection(positionIndicator));
//        }

//        static bool IsInBounds(this Position position) =>
//            position.X >= 0
//            && position.Y >= 0
//            && position.X < xMax
//            && position.Y < yMax;


//        static PatrolPosition TurnOrMove(this PatrolPosition patrolPosition)
//        {
//            var next = nextPosition(patrolPosition.position);

//            if (isBlocked(next))
//            {
//                return new(patrolPosition.position, turn(patrolPosition.direction));
//            }
//            else
//            {
//                currentMap[patrolPosition.position.Y][patrolPosition.position.X] = visited;
//                return new(next, patrolPosition.direction);
//            }
//        }

//        static Direction turn(Direction facing)
//        {
//            return facing switch
//            {
//                Direction.North => Direction.East,
//                Direction.East => Direction.South,
//                Direction.South => Direction.West,
//                Direction.West => Direction.North,
//                _ => throw new Exception("invalid direction")
//            };
//        }

//        static bool isBlocked(Position next) =>
//            inBounds(next) && currentMap[next.Y][next.X] == '#';

//        static Position nextPosition(Position position)
//        {
//            return currentDirection switch
//            {
//                Direction.North => new(position.X, position.Y - 1),
//                Direction.East => new(position.X + 1, position.Y),
//                Direction.South => new(position.X, position.Y + 1),
//                Direction.West => new(position.X - 1, position.Y),
//                _ => throw new Exception("that's not a direction")
//            };
//        }

//        static void goToStartingPositions()
//        {
//            currentMap = startingMap.Select(x => x.ToArray()).ToArray();
//            currentPosition = startingPosition;
//        }
//    }
//}
