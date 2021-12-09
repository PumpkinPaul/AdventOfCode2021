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
        //We assume the input data is good and all lines have the same length

        var heights = new int[lines.Length][];
        for (int i = 0; i < lines.Length; i++)
            heights[i] = lines[i].ToCharArray().Select(c => (int)char.GetNumericValue(c)).ToArray();

        return heights;
    }

    static IEnumerable<Location> GetAdjacentLocations(int[][] heights, int row, int col)
    {
        if (row > 0)
            yield return new Location(row - 1, col, heights[row - 1][col]); //above

        if (row < heights.Length - 1)
            yield return new Location(row + 1, col, heights[row + 1][col]); //below

        if (col > 0)
            yield return new Location(row, col - 1, heights[row][col - 1]); //left

        if (col < heights[0].Length - 1)
            yield return new Location(row, col + 1, heights[row][col + 1]); //right
    }

    private static IEnumerable<Location> GetLowPoints(int[][] heights)
    {
        var lowPoints = new List<int>();

        for (var r = 0; r < heights.Length; r++)
        {
            for (var c = 0; c < heights[r].Length; c++)
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
        var heights = LoadArray(lines);

        return GetLowPoints(heights)
            .Select(Location => Location.Height)
            .Select(h => h + 1) //risk level is height + 1
            .Sum();
    }

    private static long Part2(string[] lines)
    {
        var heights = LoadArray(lines);

        var lowPoints = GetLowPoints(heights);

        //Create a basin for each lowpoint
        var basins = new List<List<Location>>();

        //Start at the lowpoint and move up in heights
        foreach(var lowPoint in lowPoints)
        {
            var basin = new List<Location> { lowPoint };
            basins.Add(basin);

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
                basin.AddRange(adjacentLocations);
                locationsToCheck.AddRange(adjacentLocations);
                checkedLocations.AddRange(adjacentLocations);

            } while (locationsToCheck.Count > 0);
        }

        var total = 1;
        var b = basins.OrderByDescending(b => b.Count);

        foreach (var basin in basins.OrderByDescending(b => b.Count).Take(3))
            total *= basin.Count;

        return total;
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