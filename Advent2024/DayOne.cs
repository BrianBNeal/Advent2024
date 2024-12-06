namespace Advent2024; 
internal class DayOne {
	public int PartOne() {
        var (col1, col2) = File.ReadAllLines(".\\Inputs\\DayOnePartOne.txt")
            .Select(x => x.Split("   "))
            .Aggregate((new List<int>(), new List<int>()), (acc, nums) => {
                acc.Item1.Add(int.Parse(nums[0]));
                acc.Item2.Add(int.Parse(nums[1]));
                return acc;
            });
        
        col1.Sort();
        col2.Sort();

        return col1.Zip(col2, (a,b) => Math.Abs(a - b)).Sum();
	}

    public int PartTwo() {
		var (col1, col2) = File.ReadAllLines(".\\Inputs\\DayOnePartOne.txt")
			.Select(x => x.Split("   "))
			.Aggregate((new List<int>(), new List<int>()), (acc, nums) => {
				acc.Item1.Add(int.Parse(nums[0]));
				acc.Item2.Add(int.Parse(nums[1]));
				return acc;
			});

        var similarity = 0;
        foreach (var num in col1)
        {
            similarity += col2.Where(x => x == num).Count() * num;
        }

        return similarity;
    }
}
