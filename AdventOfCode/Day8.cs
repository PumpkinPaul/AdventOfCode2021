public class Day8
{
    public static void Run()
    {
        var lines = File.ReadAllLines("Input8.txt");

        Console.WriteLine("--- Day 8: Seven Segment Search ---");

        Console.WriteLine($"Test1: {Solve(lines)}");
    }

    private static long Solve(string[] lines)
    {
        //A count of the number of segments for the numbers we are interested in counting (1, 4, 7, 8)
        var sevenSegmentDigits = new HashSet<int>{{ 2 }, { 4 }, { 3 }, { 7 }};

        return lines
            .SelectMany(ParseOutputValues)
            .Count(value => sevenSegmentDigits.Contains(value.Length));

        static IEnumerable<string>ParseOutputValues(string line) => 
            line.Split('|')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
}