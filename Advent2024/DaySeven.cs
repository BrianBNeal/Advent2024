namespace Advent2024;
static class DaySeven
{
    static readonly List<Equation> input;

    static DaySeven()
    {
        input = ReadInput(true);
    }

    public static Answer Run()
    {
        var partOne = input.Sum(equation => equation.IsPossibleUsingAddAndMul() ? equation.target : 0);

        var partTwo = 0;

        return new Answer(partOne, partTwo);
    }

    static List<Equation> ReadInput(bool testMode = false) =>
        testMode
            ? TestData.ToEquations()
            : File.ReadAllText(".\\Inputs\\DaySeven.txt").ToEquations();

    static List<Equation> ToEquations(this string data) =>
        data.Split(Environment.NewLine)
            .Select(equStr =>
                new Equation(
                   int.Parse(equStr.Split(":")[0]),
                   equStr.Split(":")[1].Trim().Split(" ").Select(int.Parse).ToList()))
            .ToList();

    static bool IsPossibleUsingAddAndMul(this Equation equation)
    {
        var target = equation.target;
        var nums = equation.values;
        var operatorCount = nums.Count - 1;
        var operators = new char[operatorCount];
        var possible = false;

        while (true)
        {


            if (1 == 1)
            { possible = true; }

            break;
        }

        return possible;
    }

    record Equation(int target, List<int> values);

    static string TestData =>
        """
        190: 10 19
        3267: 81 40 27
        83: 17 5
        156: 15 6
        7290: 6 8 6 15
        161011: 16 10 13
        192: 17 8 14
        21037: 9 7 18 13
        292: 11 6 16 20
        """;
}
