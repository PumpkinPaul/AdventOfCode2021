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
        Console.WriteLine($"Part1: {Part1(lines)}"); //1691
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

        for (int r = 0; r < rows; r++)
        {
            energy[r] = new int[cols];
            for (int c = 0; c < cols; c++)
                energy[r][c] = (int)char.GetNumericValue(lines[r].ToArray()[c]);
        }

        const int FLASH_THRESHOLD = 10;

        //Process the simulation for the desired number of steps
        for (var i = 0; i < 100; i++)
        {
            var flashes = new Stack<Point>();
            var allBounds = new AABB(new Point(0, 0), new Point(rows - 1, cols - 1));

            IncreaseEnergy(allBounds, flashes);

            //Reset the flashed octopuses back to 0
            for (int r = allBounds.Min.Row; r <= allBounds.Max.Row; r++)
            {
                for (int c = allBounds.Min.Col; c <= allBounds.Max.Col; c++)
                {
                    if (energy[r][c] >= FLASH_THRESHOLD)
                        energy[r][c] = 0;
                }
            }
        }

        void IncreaseEnergy(AABB bounds, Stack<Point> flashes)
        {
            //Increase energy by 1
            for (int r = bounds.Min.Row; r <= bounds.Max.Row; r++)
            {
                for (int c = bounds.Min.Col; c <= bounds.Max.Col; c++)
                {
                    energy[r][c] += 1;

                    if (energy[r][c] == FLASH_THRESHOLD)
                    {
                        flashes.Push(new Point(r, c));
                        total++;
                    }
                }
            }

            while (flashes.Count > 0)
            {
                var point = flashes.Pop();

                var adjacentBounds = GetAdjacentBoundsInclusive(point);

                IncreaseEnergy(adjacentBounds, flashes);
            }
        }

        AABB GetAdjacentBoundsInclusive(Point point)
        {
            return new AABB(
                new Point(
                    Math.Max(point.Row - 1, 0), 
                    Math.Max(point.Col - 1, 0)
                ),
                new Point(
                    Math.Min(point.Row + 1, rows - 1), 
                    Math.Min(point.Col + 1, cols - 1)
                )
            );
        }

        return total;
    }

    private record struct Point(int Row, int Col);

    private record struct AABB(Point Min, Point Max);

    private static long Part2(string[] lines)
    {
        //====================================================================================================
        //====================================================================================================

        return 0;
    }
}