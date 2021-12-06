public class Day1
{
    public static void Run()
    {
        var lines = File.ReadAllLines("Input1.txt");
        var values = lines.Select(lines => int.Parse(lines)).ToArray();

        Console.WriteLine("RUNNING DAY #1");
        Console.WriteLine($"Part1: {Part1(values)}");
        Console.WriteLine($"Part2: {Part2(values)}");
    }

    private static int Part1(int[] values)
    {
        return CompareValues(values);
    }

    private static int Part2(int[] values)
    {
        var sums = new List<int>();
        for (var i = 2; i < values.Length; i++)
            sums.Add(values[i - 2] + values[i - 1] + values[i]);

        return CompareValues(sums.ToArray());
    }

    private static int CompareValues(int[] values)
    {
        var count = 0;
        for (var i = 1; i < values.Length; i++)
        {
            var previous = values[i - 1];
            var current = values[i];

            if (current > previous)
                count++;
        }

        return count;
    }
}