using System.Collections.ObjectModel;

public class Day11
{
    public static void Run()
    {
        Console.WriteLine("--- Day 11: Dumbo Octopus ---");

        var test = File.ReadAllLines("Test11.txt");
        Console.WriteLine($"Test1: {Part1(test)}"); //1656
        //Console.WriteLine($"Test2: {Part2(test)}"); //

        var lines = File.ReadAllLines("Input11.txt");
        Console.WriteLine($"Part1: {Part1(lines)}"); //
        //Console.WriteLine($"Part2: {Part2(lines)}"); //
    }

    private static long Part1(string[] lines)
    {
        //====================================================================================================
        //Given the starting energy levels of the dumbo octopuses in your cavern, simulate 100 steps.
        //How many total flashes are there after 100 steps?
        //====================================================================================================

        //Load the energy values into a 2d array [rows][cols]
        var rows = lines.Length;
        var cols = lines[0].Length;
        var total = 0;

        var energy = new int[rows][];
        var points = new List<Point>();

        for (int r = 0; r < rows; r++)
        {
            energy[r] = new int[cols];
            for (int c = 0; c < cols; c++)
            {
                energy[r][c] = (int)char.GetNumericValue(lines[r].ToArray()[c]);
                points.Add(new Point(r, c));
            }
        }

        //Process the simulation for the desired number of steps
        for (var i = 0; i < 100; i++)
        {
            var flashes = new Stack<Point>();
            IncreaseEnergy(points, flashes);

            //Reset the flashed octopuses back to 0
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (energy[r][c] >= 10)
                        energy[r][c] = 0;
                }
            }
        }

        void IncreaseEnergy(IEnumerable<Point> points, Stack<Point> flashes)
        {
            //Increase energy by 1
            foreach (var p in points)
            {
                energy[p.Row][p.Col] += 1;

                if (energy[p.Row][p.Col] == 10)
                {
                    flashes.Push(p);
                    total++;
                }
            }

            while (flashes.Count > 0)
            {
                var point = flashes.Pop();

                var adjacentPoints = GetAdjacentPoints(point);

                IncreaseEnergy(adjacentPoints, flashes);
            }
        }

        IEnumerable<Point> GetAdjacentPoints(Point point)
        {
            return new[]
            {
                new { r = -1, c = -1 }, new { r = -1, c = 0 }, new { r = -1, c = 1 },
                new { r =  0, c = -1 },      /* point */       new { r =  0, c = 1 },
                new { r =  1, c = -1 }, new { r =  1, c = 0 }, new { r =  1, c = 1 }
            }
            .Select(direction => new Point(point.Row + direction.r, point.Col + direction.c))
            .Where(InBounds);
        }

        bool InBounds(Point p) => p.Row >= 0 && p.Row < rows && p.Col >= 0 && p.Col < cols;

        return total;
    }

    private record struct Point(int Row, int Col);

    private static long Part2(string[] lines)
    {
        //====================================================================================================
        //====================================================================================================

        return 0;
    }
}