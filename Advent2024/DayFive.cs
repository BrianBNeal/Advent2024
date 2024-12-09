namespace Advent2024;
static class DayFive
{
    record Update(List<int> pages);
    record Rule(int a, int b);

    private static readonly List<Update> updates;
    private static readonly List<Rule> rules;

    static DayFive()
    {
        (rules, updates) = ReadInput();
    }

    public static Answer Run()
    {
        var partOne = updates
            .Where(IsCorrectlyOrdered)
            .Select(GetCenterPage)
            .Sum();

        var partTwo = updates
            .Where(x => !x.IsCorrectlyOrdered())
            .Select(OrderByRules)
            .Select(GetCenterPage)
            .Sum();

        return new Answer(partOne.ToString(), partTwo.ToString());
    }

    static (List<Rule> Rules, List<Update> Updates) ReadInput(bool testMode = false) =>
        testMode
            ? (TestData.Split(Environment.NewLine).TakeWhile(s => !string.IsNullOrWhiteSpace(s)).ParseRuleSets(),
               TestData.Split(Environment.NewLine).SkipWhile(s => !string.IsNullOrWhiteSpace(s)).Skip(1).ParseUpdates())
            : (File.ReadAllLines(".\\Inputs\\DayFive.txt").TakeWhile(s => !string.IsNullOrWhiteSpace(s)).ParseRuleSets(),
               File.ReadAllLines(".\\Inputs\\DayFive.txt").SkipWhile(s => !string.IsNullOrWhiteSpace(s)).Skip(1).ParseUpdates());

    static List<Rule> ParseRuleSets(this IEnumerable<string> rules) =>
        rules.Select(ruleString => new Rule(int.Parse(ruleString.Split("|")[0]), int.Parse(ruleString.Split("|")[1]))).ToList();

    static List<Update> ParseUpdates(this IEnumerable<string> pages) =>
        pages.Select(list => new Update(list.Split(",").Select(int.Parse).ToList())).ToList();

    static bool IsCorrectlyOrdered(this Update update) =>
        rules.Where(rule => rule.IsApplicableToUpdate(update))
            .All(rule => rule.IsFollowed(update));

    static bool IsFollowed(this Rule rule, Update update) =>
        update.pages.IndexOf(rule.a) < update.pages.IndexOf(rule.b);

    static bool IsApplicableToUpdate(this Rule rule, Update update) =>
        update.pages.Intersect([rule.a, rule.b]).Count() == 2;

    static Update OrderByRules(this Update update)
    {
        var rulesToApply = rules.Where(rule => rule.IsApplicableToUpdate(update)).ToList();
        var adjacency = update.pages.ToDictionary(n => n, n => new List<int>());
        var inDegree = update.pages.ToDictionary(n => n, n => 0);

        foreach (var (a, b) in rulesToApply)
        {
            adjacency[a].Add(b);
            inDegree[b]++;
        }

        var queue = new Queue<int>(inDegree.Where(kvp => kvp.Value == 0).Select(kvp => kvp.Key));
        var result = new List<int>();

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            result.Add(node);

            foreach (var neighbor in adjacency[node])
            {
                inDegree[neighbor]--;
                if (inDegree[neighbor] == 0)
                    queue.Enqueue(neighbor);
            }
        }

        return update with { pages = result };
    }

    static int GetCenterPage(this Update update) =>
        update.pages[update.pages.Count / 2];

    static string TestData =>
        """
        47|53
        97|13
        97|61
        97|47
        75|29
        61|13
        75|53
        29|13
        97|29
        53|29
        61|53
        97|53
        61|29
        47|13
        75|47
        97|75
        47|61
        75|61
        47|29
        75|13
        53|13

        75,47,61,53,29
        97,61,53,29,13
        75,29,13
        75,97,47,61,53
        61,13,29
        97,13,75,29,47
        """;
}
