using System.Text.RegularExpressions;

namespace Advent2024;
static class DayThree
{
    public static Answer Run()
    {
        var input = ReadInput();
        var instructions = input.SelectMany(Parse).ToList();
        var partOne = instructions.OfType<Multiply>().SumProducts();
        var partTwo = instructions.SumProducts();

        return new Answer(partOne.ToString(), partTwo.ToString());
    }

    static int SumProducts(this IEnumerable<Instruction> instructions) =>
        instructions.Aggregate(
            (sum: 0, include: true),
            (acc, instruction) => instruction switch
            {
                Pause => (sum: acc.sum, include: false),
                Resume => (sum: acc.sum, include: true),
                Multiply mul when acc.include => (sum: acc.sum + mul.A * mul.B, include: true),
                _ => acc
            }).sum;

    abstract record Instruction;
    record Multiply(int A, int B) : Instruction;
    record Pause : Instruction;
    record Resume : Instruction;

    static IEnumerable<Instruction> Parse(this string line) =>
        Regex.Matches(line, @"(?<mul>mul)\((?<a>\d+),(?<b>\d+)\)|(?<dont>don't\(\))|(?<do>do\(\))")
            .Select(match => match switch
            {
                _ when match.Groups["dont"].Success => (Instruction)new Pause(),
                _ when match.Groups["do"].Success => (Instruction)new Resume(),
                _ => (Instruction)new Multiply(int.Parse(match.Groups["a"].Value), int.Parse(match.Groups["b"].Value)),
            });

    static string[] ReadInput() =>
        File.ReadAllLines(".\\Inputs\\DayThree.txt");
}
