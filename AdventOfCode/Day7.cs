using System.Diagnostics;

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

        //Simple cost is the difference between the two horizontal positions
        static int SimpleCost(int difference) => difference;

        //Compound cost, the first step costs 1, the second step costs 2, the third step costs 3, etc
        //Create a simple formula instead of storing the costs in an array
        static int CompoundCost(int value) => (int)(value * ((value + 1) * 0.5f));
    }

    private static long Solve(string[] lines, Func<int, int> costFunc)
    {
        var positions = lines[0].Split(',').Select(int.Parse).ToArray();

        var minPosition = positions.Min();
        var maxPosition = positions.Max();
        var maxSteps = maxPosition - minPosition;

        var costs = new int[maxSteps];

        //For each position brute-force the cost it would take for each step in the range
        //At the end we'll sort and take the smallest

        for (var x = 0; x < positions.Length; x++)
        {
            for (var i = 0; i < maxSteps; i++)
            {
                var difference = Math.Abs(positions[x] - i);                
                costs[i] += costFunc(difference);

                //Note - could store the cost for this difference as there may well be other crab submarines to
                //follow that have the same horizontal position. We currently choose to calculate it each time.
                //Measuring would tell us which approach would be faster if that is something we wanted to optimise
            }
        }

        Array.Sort(costs);
        return costs[0];
    }
}