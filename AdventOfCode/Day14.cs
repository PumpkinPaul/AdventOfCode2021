public class Day14
{
    public static void Run()
    {
        Console.WriteLine("--- Day 14: Extended Polymerization ---");

        var test = File.ReadAllLines("Test14.txt");
        //Console.WriteLine($"Test1(1): {Part1(test)}"); //17
        //Part2(test);                                   //O

        var lines = File.ReadAllLines("Input14.txt");
        Console.WriteLine($"Part1: {Part1(lines)}"); //701
        //Console.WriteLine($"Part1: {Part2(lines)}"); //701
    }

    public static long Part1(string[] lines)
    {
        //====================================================================================================
        //Apply 10 steps of pair insertion to the polymer template and find the most
        //and least common elements in the result. What do you get if you take the
        //quantity of the most common element and subtract the quantity of the least
        //common element?
        //====================================================================================================

        var (polymer, rules) = Load(lines);

        var p = polymer;
        for (var i= 0; i < 10; i++)
        {
            p = polymer;
            while (p.Next != null)
            {
                var n = p.Next;
                var pair = $"{p.Value}{p.Next.Value}";
                var rule = rules[pair];

                p.List?.AddAfter(p, rule);
                p = n;
            }

        }

        p = polymer;
        while (p != null)
        {
            Console.Write(p.Value);
            p = p.Next;
        }

        var counts = new Dictionary<char, int>();

        p = polymer;
        while (p != null)
        {
            if (counts.ContainsKey(p.Value) == false)
                counts[p.Value] = 0;

            counts[p.Value]++;

            p = p.Next;
        }

        var max = counts.Values.Max();
        var min = counts.Values.Min();

        return max - min;
    }

    public static long Part2(string[] lines)
    {
        //====================================================================================================
        //
        //====================================================================================================

        return 0;
    }

    private static (LinkedListNode<char>, Dictionary<string, char>) Load(string[] lines)
    {
        var polymer = new LinkedList<char>();
        var p = lines[0];
        polymer.AddFirst(p[0]);
        for (int i = 1; i < p.Length; i++)
            polymer.AddLast(p[i]);

        var rules = lines.Skip(2).ToDictionary(line => line.Split(" -> ")[0], line => char.Parse(line.Split(" -> ")[1]));

        return (polymer.First, rules);
    }
}