namespace Advent2024;
static class DayThree
{
    public static (List<int> left, List<int> right) ReadLists()
    {
        return File.ReadAllLines(".\\Inputs\\DayOnePartThree.txt")
            .Select(x => x.Split("   "))
            .Aggregate((new List<int>(), new List<int>()), (acc, nums) =>
            {
                acc.Item1.Add(int.Parse(nums[0]));
                acc.Item2.Add(int.Parse(nums[1]));
                return acc;
            });
    }

    public static Answer Run()
    {
        var (col1, col2) = ReadLists();

        var partOne = 0;

        var partTwo = 0;

        return new Answer(partOne, partTwo);
    }
}
