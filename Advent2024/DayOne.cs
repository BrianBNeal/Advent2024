namespace Advent2024;
static class DayOne
{
    public static Answer Run()
    {
        var (col1, col2) = ReadInput();

        var partOne = col1.Order()
            .Zip(col2.Order(), (a, b) => Math.Abs(a - b))
            .Sum();

        var partTwo = col2
            .Where(new HashSet<int>(col1).Contains)
            .GroupBy(x => x)
            .Sum(x => x.Key * x.Count());

        return new Answer(partOne.ToString(), partTwo.ToString());
    }

    static (List<int> left, List<int> right) ReadInput() =>
        File.ReadAllLines(".\\Inputs\\DayOne.txt")
            .Select(x => x.Split("   "))
            .Aggregate((new List<int>(), new List<int>()), (acc, nums) =>
            {
                acc.Item1.Add(int.Parse(nums[0]));
                acc.Item2.Add(int.Parse(nums[1]));
                return acc;
            });
}
