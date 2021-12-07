public class Day7
{
    public static void Run()
    {
        var test = new[] { "16,1,2,0,4,2,7,1,2,14" };
        var lines = File.ReadAllLines("Input7.txt");

        Console.WriteLine("--- Day 7: The Treachery of Whales ---");

        Console.WriteLine($"Test1: {Solve(test, SimpleCost)}");    //37
        Console.WriteLine($"Test2: {Solve(test, CompoundCost)}");  //168

        Console.WriteLine($"Part1: {Solve(lines, SimpleCost)}");   //342730
        Console.WriteLine($"Part2: {Solve(lines, CompoundCost)}"); //92335207

        //Simple cost is the differemce between thw two horizontal positions
        static int SimpleCost(int difference) => difference;

        //Compound cost the first step costs 1, the second step costs 2, the third step costs 3
        static int CompoundCost(int value) => (int)(value * ((value + 1) * 0.5f));
    }

    private static long Solve(string[] lines, Func<int, int> costFunc)
    {
        var positions = lines[0].Split(',').Select(int.Parse).ToArray();

        var minPosition = positions.Min();
        var maxPosition = positions.Max();
        var maxSteps = maxPosition - minPosition;

        var costs = new int[maxSteps];

        for (var x = 0; x < positions.Length; x++)
        {
            //For each position brute-force the cost of each step in the range
            for (var i = 0; i < maxSteps; i++)
            {
                var difference = Math.Abs(positions[x] - i);
                var cost = costFunc(difference);

                costs[i] += cost;
            }
        }

        Array.Sort(costs);
        return costs[0];
    }
}