public class Day9
{
    public static void Run()
    {
        Console.WriteLine("--- Day 9: Smoke Basin ---");

        //var test = File.ReadAllLines("Test9.txt");
        //Console.WriteLine($"Test1: {Part1(test)}"); //15
        //Console.WriteLine($"Test2: {Part2(test)}"); //1134

        var lines = File.ReadAllLines("Input9.txt");
        Console.WriteLine($"Part1: {Part1(lines)}"); //444
        Console.WriteLine($"Part2: {Part2(lines)}"); //1168440
    }

    private static int[][] LoadArray(string[] lines)
    {
        //We will pad the array with a border of 9's to make the adjacent lookups simpler
        var width = lines[0].Length;
        var lineOfNines = new string('9', width);

        lines = lines.Prepend(lineOfNines).Append(lineOfNines).ToArray();

        var heights = new int[lines.Length][];

        for (var i = 0; i < lines.Length; i++)
            heights[i] = $"9{lines[i]}9".ToCharArray().Select(c => (int)char.GetNumericValue(c)).ToArray();

        return heights;
    }

    private static IEnumerable<Location> GetAdjacentLocations(int[][] heights, int row, int col)
    {
        return new[] { new { r = 1, c = 0 }, new { r = -1, c = 0 }, new { r = 0, c = 1 }, new { r = 0, c = -1 } }
            .Select(p => new Location(row + p.r, col + p.c, heights[row + p.r][col + p.c]));
    }

    private static IEnumerable<Location> GetLowPoints(int[][] heights)
    {
        var lowPoints = new List<int>();

        for (var r = 1; r < heights.Length - 1; r++)
        {
            for (var c = 1; c < heights[r].Length - 1; c++)
            {
                //Check adjacent locations
                var adjacentLocations = GetAdjacentLocations(heights, r, c).Select(l => l.Height);

                var height = heights[r][c];

                if (height < adjacentLocations.Min())
                    yield return new Location(r, c, height);
            }
        }
    }

    private static long Part1(string[] lines)
    {
        //====================================================================================================
        //What is the sum of the risk levels of all low points on your heightmap?
        //====================================================================================================

        var heights = LoadArray(lines);

        return GetLowPoints(heights)
            .Select(Location => Location.Height)
            .Select(h => h + 1) //risk level is height + 1
            .Sum();
    }

    private static long Part2(string[] lines)
    {
        //====================================================================================================
        //What do you get if you multiply together the sizes of the three largest basins?
        //====================================================================================================

        var heights = LoadArray(lines);

        var lowPoints = GetLowPoints(heights);

        //Create a basin for each lowpoint
        var basins = new List<int>();

        //Start at the lowpoint and move up in heights
        foreach (var lowPoint in lowPoints)
        {
            var basin = 1;

            //Now flood fill from these starting points getting locations where the heights are one more
            var locationsToCheck = new Queue<Location>();
            locationsToCheck.Enqueue(lowPoint);

            //Note - during the flood fill we will get locations that we have already checked so keep a track of them
            var checkedLocations = new HashSet<Location> { lowPoint };

            do
            {
                var location = locationsToCheck.Dequeue();

                //Check adjacent height
                var adjacentLocations = GetAdjacentLocations(heights, location.Row, location.Col)
                    .Where(adjacent =>
                        checkedLocations.Contains(adjacent) == false
                        && adjacent.Height > location.Height
                        && adjacent.Height != 9);

                //These adjacent heights are in the basin!
                basin += adjacentLocations.Count();
                locationsToCheck.AddRange(adjacentLocations);
                checkedLocations.AddRange(adjacentLocations);

            } while (locationsToCheck.Count > 0);

            basins.Add(basin);
        }

        return basins
            .OrderByDescending(b => b)
            .Take(3)
            .Aggregate(1, (a, b) => a * b);
    }

    private readonly record struct Location(int Row, int Col, int Height);
}

public static class Extensions
{
    public static void AddRange<T>(this Queue<T> source, IEnumerable<T> items)
    {
        foreach (var item in items)
            source.Enqueue(item);
    }

    public static void AddRange<T>(this HashSet<T> source, IEnumerable<T> items)
    {
        foreach (var item in items)
            source.Add(item);
    }
}