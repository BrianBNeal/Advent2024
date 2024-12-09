namespace Advent2024;

static class DaySix
{
    record PatrolPosition(Position Location, Direction Direction);

    enum Direction
    {
        North,
        East,
        South,
        West,
    }

    const char BLOCKED = '#';
    const char VISITED = 'X';

    static readonly PatrolPosition startingPosition;
    static readonly char[][] startingMap;

    static int yMax => startingMap.Length;
    static int xMax => startingMap[0].Length;

    static private PatrolPosition currentPosition;
    static char[][] currentMap;

    static DaySix()
    {
        startingMap = readInput();
        startingPosition = getStartingPosition();
        currentMap = cloneMap();
        currentPosition = startingPosition;
    }

    public static Answer Run()
    {
        goToStartingPositions();

        var originalPatrolPath = new HashSet<PatrolPosition>();
        while (currentPosition.Location.IsInBounds())
        {
            currentMap[currentPosition.Location.y][currentPosition.Location.x] = VISITED;
            originalPatrolPath.Add(currentPosition);
            currentPosition = currentPosition.TurnOrMove();
        }

        var partOne = currentMap.SelectMany(x => x).Count(x => x == VISITED);

        int partTwo = 0;
        var validObstacleLocations = originalPatrolPath.Select(x => x.Location).ToList();

        for (int i = 0; i < validObstacleLocations.Count; i++)
        {
            var obstacleLocation = validObstacleLocations[i];
            if (obstacleLocation == startingPosition.Location)
            { continue; }

            goToStartingPositions();
            currentMap[obstacleLocation.y][obstacleLocation.x] = BLOCKED;
            var patrolledPath = new HashSet<PatrolPosition>() { startingPosition };
            bool caughtInALoop = false;

            while (currentPosition.Location.IsInBounds() && !caughtInALoop)
            {
                currentMap[currentPosition.Location.y][currentPosition.Location.x] = VISITED;
                currentPosition = currentPosition.TurnOrMove();
                caughtInALoop = patrolledPath.Contains(currentPosition);
                patrolledPath.Add(currentPosition);
            }
            if (caughtInALoop)
            { partTwo++; }
        }

        return new(partOne.ToString(), partTwo.ToString());
    }

    static Direction ParseDirection(this char indicator) =>
        indicator switch
        {
            '^' => Direction.North,
            '>' => Direction.East,
            'v' => Direction.South,
            '<' => Direction.West,
            _ => throw new Exception("invalid indicator")
        };


    static bool IsInBounds(this Position position) =>
        position.x >= 0
        && position.y >= 0
        && position.x < xMax
        && position.y < yMax;

    static PatrolPosition TurnOrMove(this PatrolPosition patrolPosition)
    {
        var next = patrolPosition.NextLocation();

        return next.IsBlocked()
            ? new(patrolPosition.Location, patrolPosition.Direction.Turn())
            : new(next, patrolPosition.Direction);
    }

    static Direction Turn(this Direction facing)
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

    static bool IsBlocked(this Position next) =>
        next.IsInBounds() && currentMap[next.y][next.x] == '#';

    static Position NextLocation(this PatrolPosition position) =>
        position.Direction switch
        {
            Direction.North => new(position.Location.x, position.Location.y - 1),
            Direction.East => new(position.Location.x + 1, position.Location.y),
            Direction.South => new(position.Location.x, position.Location.y + 1),
            Direction.West => new(position.Location.x - 1, position.Location.y),
            _ => throw new Exception("invalid direction")
        };

    static PatrolPosition getStartingPosition()
    {
        char[] indicators = ['^', '>', 'v', '<'];
        var startingRow = startingMap.Single(line => line.Intersect(indicators).Any());
        var positionIndicator = startingRow.Single(indicators.Contains);
        return new PatrolPosition(
            new Position(Array.IndexOf([.. startingRow], positionIndicator), Array.IndexOf(startingMap, startingRow)),
            positionIndicator.ParseDirection());
    }

    static void goToStartingPositions()
    {
        currentMap = cloneMap();
        currentPosition = startingPosition;
    }

    static char[][] readInput() =>
       File.ReadAllLines(".\\Inputs\\DaySix.txt").Select(line => line.ToCharArray()).ToArray();

    static char[][] cloneMap() =>
        startingMap.Select(x => x.ToArray()).ToArray();
}
