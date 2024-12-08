
using System.Text;

namespace Advent2024;

static class DaySix
{
    record PatrolPosition(Location Location, Direction Direction);
    record Location(int X, int Y);

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
            currentMap[currentPosition.Location.Y][currentPosition.Location.X] = VISITED;
            originalPatrolPath.Add(currentPosition);
            currentPosition = currentPosition.TurnOrMove();
        }

        var partOne = currentMap.SelectMany(x => x).Count(x => x == VISITED);
        print(currentMap.ToGraphic());

        int partTwo = 0;
        var validObstacleLocations = originalPatrolPath.Select(x => x.Location).ToList();

        for (int i = 0; i < validObstacleLocations.Count; i++)
        {
            var obstacleLocation = validObstacleLocations[i];
            if (obstacleLocation == startingPosition.Location)
            { continue; }

            goToStartingPositions();
            currentMap[obstacleLocation.Y][obstacleLocation.X] = BLOCKED;
            var patrolledPath = new HashSet<PatrolPosition>() { startingPosition };
            bool caughtInALoop = false;

            while (currentPosition.Location.IsInBounds() && !caughtInALoop)
            {
                currentMap[currentPosition.Location.Y][currentPosition.Location.X] = VISITED;
                currentPosition = currentPosition.TurnOrMove();
                caughtInALoop = patrolledPath.Contains(currentPosition);
                patrolledPath.Add(currentPosition);
            }
            if (caughtInALoop)
            { partTwo++; }
        }

        return new(partOne, partTwo);
    }

    static string ToGraphic(this char[][] map) =>
        map.Aggregate(new StringBuilder(),
            (sb, chars) => sb.AppendLine(new string(chars)))
            .ToString();

    static void print(string v)
    {
        Console.WriteLine(v);
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


    static bool IsInBounds(this Location position) =>
        position.X >= 0
        && position.Y >= 0
        && position.X < xMax
        && position.Y < yMax;

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

    static bool IsBlocked(this Location next) =>
        next.IsInBounds() && currentMap[next.Y][next.X] == '#';

    static Location NextLocation(this PatrolPosition position) =>
        position.Direction switch
        {
            Direction.North => new(position.Location.X, position.Location.Y - 1),
            Direction.East => new(position.Location.X + 1, position.Location.Y),
            Direction.South => new(position.Location.X, position.Location.Y + 1),
            Direction.West => new(position.Location.X - 1, position.Location.Y),
            _ => throw new Exception("invalid direction")
        };

    static PatrolPosition getStartingPosition()
    {
        char[] indicators = ['^', '>', 'v', '<'];
        var startingRow = startingMap.Single(line => line.Intersect(indicators).Any());
        var positionIndicator = startingRow.Single(indicators.Contains);
        print("found starting position");
        return new PatrolPosition(
            new Location(Array.IndexOf([.. startingRow], positionIndicator), Array.IndexOf(startingMap, startingRow)),
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
