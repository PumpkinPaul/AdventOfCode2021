public class Day6
{
    public static void Run()
    {
        var lines = File.ReadAllLines("Input6.txt");

        Console.WriteLine("--- Day 6: Lanternfish ---");
        Console.WriteLine($"Part1: {Part1(lines)}");
        Console.WriteLine($"Part2: {Part2(lines)}");
    }

    private static int Part1(string[] lines)
    {
        var numbers = lines[0].Split(',').Select(int.Parse);

        //Just keep a track of the total number of fish at each age (we don't care about individual fish)
        var ages = new int[9];

        foreach(var number in numbers)
            ages[number]++;

        //Process the number of days required
        for (var day = 0; day < 80; day++)
        {
            //How many new fish are required today?
            var spawnCount = ages[0];
            
            //Age the fishes
            for (var n = 1; n < ages.Length; n++)
                ages[n - 1] = ages[n];

            ages[6] += spawnCount; //0s become 6s so bump the total
            ages[8] = spawnCount;  //Spawn the new 8s
        }

        return ages.Sum(a => a);
    }

    private static int Part2(string[] lines)
    {
        return 0;
    }
}