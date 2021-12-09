public class Day9
{
    public static void Run()
    {
        var lines = File.ReadAllLines("Input9.txt");

        Console.WriteLine("--- Day 9: Smoke Basin ---");

        Console.WriteLine($"Part1: {Part1(lines)}"); //274
        Console.WriteLine($"Part2: {Part2(lines)}"); //1012089
    }

    private static int[][] LoadArray(string[] lines)
    {
        //We assume the input data is good and all lines have the same length

        var heights = new int[lines.Length][];
        for (int i = 0; i < lines.Length; i++)
            heights[i] = lines[i].ToCharArray().Select(c => (int)char.GetNumericValue(c)).ToArray();

        return heights;
    }

    private static IEnumerable<int> GetAdjacentHeights(int[][] heights, int row, int col)
    {
        if (row > 0) yield return heights[row - 1][col];                     //above
        if (row < heights.Length - 1) yield return heights[row + 1][col];    //below
        if (col > 0) yield return heights[row][col - 1];                     //left
        if (col < heights[0].Length - 1) yield return heights[row][col + 1]; //right
    }

    private static long Part1(string[] lines)
    {
        var lowPoints = new List<int>();

        var heights = LoadArray(lines);
        for(var r = 0; r < heights.Length; r++)
        {
            for (var c = 0; c < heights[r].Length; c++)
            {
                var height = heights[r][c];

                //Check adjacent height
                var adjacentHeights = GetAdjacentHeights(heights, r, c);
                if (height < adjacentHeights.Min())
                    lowPoints.Add(height);
            }
        }

        return lowPoints.Select(h => h + 1).Sum();
    }

    private static long Part2(string[] lines)
    {
        return 0;
    }
}