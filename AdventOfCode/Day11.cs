public class Day11
{
    public static void Run()
    {
        Console.WriteLine("--- Day 11: Dumbo Octopus ---");

        var test = File.ReadAllLines("Test11.txt");
        Console.WriteLine($"Test1: {Part1(test)}"); //1656
        Console.WriteLine($"Test2: {Part2(test)}"); //195

        var lines = File.ReadAllLines("Input11.txt");
        Console.WriteLine($"Part1: {Part1(lines)}"); //1691
        Console.WriteLine($"Part2: {Part2(lines)}"); //216
    }

    const int FLASH_THRESHOLD = 10;

    private static long Part1(string[] lines)
    {
        //====================================================================================================
        //Given the starting energy levels of the dumbo octopuses in your cavern, simulate 100 steps.
        //How many total flashes are there after 100 steps?
        //====================================================================================================

        var totalFlashes = 0;

        var (energy, rows, cols) = LoadEnergyGrid(lines);

        var allBounds = new Bounds(0, 0, rows - 1, cols - 1);

        //Process the simulation for the desired number of steps
        for (var i = 0; i < 100; i++)
        {
            //Increase the energy for each cell in the bounds...
            totalFlashes += IncreaseEnergy(energy, rows, cols, allBounds);

            //...reset the flashed octopuses back to 0
            ProcessEnergyGrid(ref allBounds, (r, c) => { if (energy[r][c] >= FLASH_THRESHOLD) energy[r][c] = 0; });
        }

        return totalFlashes;
    }

    private static long Part2(string[] lines)
    {
        //====================================================================================================
        //If you can calculate the exact moments when the octopuses will all flash
        //simultaneously, you should be able to navigate through the cavern.
        //What is the first step during which all octopuses flash?
        //====================================================================================================

        var (energy, rows, cols) = LoadEnergyGrid(lines);

        var allBounds = new Bounds(0, 0, rows - 1, cols - 1);

        //Keep processing the simulation until the desired energy array is reached
        var step = 0L;
        int simultaneousFlashCount;
        do
        {
            simultaneousFlashCount = 0;

            //Increase the energy for each cell in the bounds...
            IncreaseEnergy(energy, rows, cols, allBounds);

            //...reset the flashed octopuses back to 0
            ProcessEnergyGrid(ref allBounds, (r, c) => { if (energy[r][c] >= FLASH_THRESHOLD) energy[r][c] = 0; });

            //count the simultaneous flashed this step
            ProcessEnergyGrid(ref allBounds, (r, c) => { if (energy[r][c] == 0) simultaneousFlashCount++; });

            step++;

        } while (simultaneousFlashCount != rows * cols);

        return step;
    }

    private static (int[][], int, int) LoadEnergyGrid(string[] lines)
    {
        //Load the energy values into a 2d array [rows][cols]
        var rows = lines.Length;
        var cols = lines[0].Length;

        var energy = new int[rows][];

        for (int r = 0; r < rows; r++)
        {
            energy[r] = new int[cols];
            for (int c = 0; c < cols; c++)
                energy[r][c] = (int)char.GetNumericValue(lines[r].ToArray()[c]);
        }

        return (energy, rows, cols);
    }

    //Do something to each item in the grid using the passed bounds
    //Process A         Process B         Process C
    //. . . . . . .     . . . . . . .     . . . . . . .
    //. X X X . . .     . . . . . . .     . . . . . . X     
    //. X X X . . .     . . . . . . .     . . . . . . X
    //. X X X . . .     . . . . . . .     . . . . . . X
    //. . . . . . .     . . . . . X X     . . . . . . .
    //. . . . . . .     . . . . . X X     . . . . . . .
    private static void ProcessEnergyGrid(ref Bounds bounds, Action<int, int> itemAction)
    {
        for (int r = bounds.MinRow; r <= bounds.MaxRow; r++)
            for (int c = bounds.MinCol; c <= bounds.MaxCol; c++)
                itemAction(r, c);
    }

    //Increases the energy of all cells in the bounds - it's ok to exceed the flash threshold so no need to clamp
    private static int IncreaseEnergy(int[][] energy, int rows, int cols, Bounds bounds)
    {
        var flashes = 0;
        ProcessEnergyGrid(ref bounds, (r, c) =>
        {
            energy[r][c] += 1;

            if (energy[r][c] != FLASH_THRESHOLD) return;

            //Flash this octopus and process neighbours
            flashes += 1 + IncreaseEnergy(energy, rows, cols, GetAdjacentBoundsInclusive(rows, cols, r, c));
        });

        return flashes;
    }

    //Pass in a point and get the min and max points respecting the energy array bounds (no IndexOutOfBounds error surprises later)
    private static Bounds GetAdjacentBoundsInclusive(int rows, int cols, int row, int col) => new(
        Math.Max(row - 1, 0),
        Math.Max(col - 1, 0),
        Math.Min(row + 1, rows - 1),
        Math.Min(col + 1, cols - 1));

    private record struct Bounds(int MinRow, int MinCol, int MaxRow, int MaxCol);
}