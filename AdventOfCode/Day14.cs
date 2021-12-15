public class Day14
{
    public static void Run()
    {
        Console.WriteLine("--- Day 14: Extended Polymerization ---");

        var test = File.ReadAllLines("Test14.txt");
        Console.WriteLine($"Test1: {Part1(test)}"); //1588
        Console.WriteLine($"Test2: {Part2(test)}"); //2188189693529

        var lines = File.ReadAllLines("Input14.txt");
        Console.WriteLine($"Part1: {Part1(lines)}"); //2112
        Console.WriteLine($"Part2: {Part2(lines)}"); //3243771149914
    }

    public static long Part1(string[] lines) => Solve(lines, 10);

    public static long Part2(string[] lines) => Solve(lines, 40);

    //====================================================================================================
    //Apply X steps of pair insertion to the polymer template and find the most
    //and least common elements in the result. What do you get if you take the
    //quantity of the most common element and subtract the quantity of the least
    //common element?
    //====================================================================================================
    private static long Solve(string[] lines, int steps)
    { 
        var (polymer, rules) = Load(lines);

        var counts = new Counter<char>(polymer);
        var pairs = new Counter<(char, char)>(Pairwise(polymer));

        for (var i = 0; i < steps; i++)
        {
            foreach (var ((left, right), count) in pairs.Items.ToArray())
            {
                var result = rules[(left, right)];

                // These pairs are created in one step
                pairs[(left, result)] += count;

                pairs[(result, right)] += count;
                //The original pair is broken in one step
                pairs[(left, right)] -= count;

                //Add to current count of the new element
                counts[result] += count;
            }
        }

        var max = counts.Values.Max();
        var min = counts.Values.Min();

        return max - min;
    }

    private static (string, Dictionary<(char, char), char>) Load(string[] lines)
    {
        var polymer = lines[0];
        var rules = lines
            .Skip(2)
            .Select(line => line.Split(" -> "))
            .ToDictionary(parts => (parts[0][0], parts[0][1]), parts => char.Parse(parts[1]));

        return (polymer, rules);
    }

    private class Counter<T> where T : notnull
    {
        private readonly Dictionary<T, long> _data = new();

        public Counter(IEnumerable<T> source)
        {
            foreach (var item in source)
                this[item] += 1;
        }

        public long this[T key]
        {
            get { _data.TryGetValue(key, out var count); return count; }
            set => _data[key] = value;
        }

        public IEnumerable<T> Keys => _data.Keys;

        public IEnumerable<long> Values => _data.Values;

        public IEnumerable<(T, long)> Items => _data.Select(kvp => (kvp.Key, kvp.Value));
    }

    private static IEnumerable<(T, T)> Pairwise<T>(IEnumerable<T> source)
    {
        for (int i = 0; i < source.Count() - 1; i++)
            yield return (source.ElementAt(i), source.ElementAt(i + 1));
    }
}