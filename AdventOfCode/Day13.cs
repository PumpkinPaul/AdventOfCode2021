public class Day13
{
    public static void Run()
    {
        Console.WriteLine("--- Day 13: Transparent Origami ---");

        //var test = File.ReadAllLines("Test13.txt");
        //Console.WriteLine($"Test1(1): {Part1(test)}"); //17
        //Part2(test);

        var lines = File.ReadAllLines("Input13.txt");
        //Console.WriteLine($"Part1: {Part1(lines)}"); //701
        Part2(lines);
    }

    public static long Part1(string[] lines)
    {
        //====================================================================================================
        //How many paths through this cave system are there that visit small caves at most once?
        //====================================================================================================

        var (grid, rows, cols, folds) = LoadInput(lines);

        var (r2, c2) = Fold1(grid, rows, cols, folds.Take(1));

        var dots = 0;
        for (var r = 0; r < r2; r++)
            for (var c = 0; c < c2; c++)
                if (grid[r][c] > 0) dots++;

        return dots;
    }

    public static long Part2(string[] lines)
    {
        //====================================================================================================
        //Given these new rules, how many paths through this cave system are there?
        //====================================================================================================

        var (grid, rows, cols, folds) = LoadInput(lines);

        var foldedGrid = Fold2(grid, folds);

        DisplayResults(foldedGrid);

        return 0;
    }

    private static (int[][], int, int, IEnumerable<string>) LoadInput(string[] lines)
    {
        var validLines = lines.Where(line => string.IsNullOrEmpty(line) == false);
        var coords = validLines.Where(line => line[0] <= '9');
        var folds = validLines.Except(coords);

        var points = coords.Select(Point.Parse).ToArray();

        var rows = points.Max(p => p.Y) + 1;
        var cols = points.Max(p => p.X) + 1;

        var grid = CreateGrid(rows, cols);

        foreach (var p in points)
            grid[p.Y][p.X] = 1;

        return (grid, rows, cols, folds);
    }

    private static int[][] CreateGrid(int rows, int cols)
    {
        var grid = new int[rows][];
        for (var r = 0; r < rows; r++)
            grid[r] = new int[cols];
     
        return grid;
    }

    private static (int, int) Fold1(
        int[][] grid,
        int rows,
        int cols,
        IEnumerable<string> folds)
    {
        var rows2 = rows;
        var cols2 = cols;

        foreach (var fold in folds)
        {
            var foldData = fold.Split('=');
            var foldDimension = foldData[0].Split(' ').Last();
            var foldValue = int.Parse(foldData[1]);

            if (foldDimension == "y")
            {
                rows2 = foldValue;
                for (var r = 0; r < rows2; r++)
                {
                    for (var c = 0; c < cols2; c++)
                        grid[r][c] += grid[rows - 1 - r][c];
                }
            }
            else
            {
                cols2 = foldValue;
                for (var r = 0; r < rows2; r++)
                {
                    for (var c = 0; c < cols2; c++)
                        grid[r][c] += grid[r][cols - 1 - c];
                }
            }
        }

        return (rows2, cols2);
    }

    private static int[][] Fold2(
        int[][] grid,
        IEnumerable<string> folds)
    {
        int[][] foldedGrid = null;
        var sourceGrid = grid;

        foreach (var fold in folds)
        {
            var foldData = fold.Split('=');
            var foldDimension = foldData[0].Split(' ').Last();
            var foldValue = int.Parse(foldData[1]);

            //There are asymetrical folds (sneaky part 2!)
            //Determine the size of the new grid
            var sourceRows = sourceGrid.Length;
            var sourceCols = sourceGrid[0].Length;
            var newRows = sourceRows;
            var newCols = sourceCols;

            if (foldDimension == "y")
            {
                var sTop = 0;
                var sBottom = 0;
                var dTop = 0;
                var dBottom = 0;

                //Get the sizes of the paper to the top and bottom of the fold
                var topRows = foldValue;
                var bottomRows = sourceRows - topRows - 1;

                newRows = Math.Max(topRows, bottomRows);

                //Create a new blank grid that will be large enough to contain the largest side of the fold
                foldedGrid = CreateGrid(newRows, newCols);

                if (bottomRows > topRows)
                {
                    sTop = 0;
                    sBottom = sourceRows - 1;

                    dTop = bottomRows + 1;
                    dBottom = 0;
                }
                else if (topRows > bottomRows)
                {
                    sTop = 0;
                    sBottom = sourceRows - 1;

                    dTop = 0;
                    dBottom = topRows - bottomRows;
                }
                else
                {
                    sTop = 0;
                    sBottom = sourceRows - 1;

                    dTop = 0;
                    dBottom = 0;
                }

                //Blit both sides from the source grid to the correct locations in the foldedGrid
                BlitGrid(sourceGrid, 0, sTop, foldedGrid, 0, dTop, topRows, newCols, 1, 1);
                BlitGrid(sourceGrid, 0, sBottom, foldedGrid, 0, dBottom, bottomRows, newCols, 1, -1);
            }
            else
            {
                var sLeft = 0;
                var sRight = 0;
                var dLeft = 0;
                var dRight = 0;

                //Get the sizes of the paper to the left and right of the fold
                var leftCols = foldValue;
                var rightCols = sourceCols - leftCols - 1;

                newCols = Math.Max(leftCols, rightCols);

                //Create a new blank grid that will be large enough to contain the largest side of the fold
                foldedGrid = CreateGrid(newRows, newCols);

                if (rightCols > leftCols)
                {
                    sLeft = 0;
                    sRight = sourceCols - 1;

                    dLeft = rightCols + 1;
                    dRight = 0;
                }
                if (leftCols > rightCols)
                {
                    sLeft = 0;
                    sRight = sourceCols - 1;

                    dLeft = 0;
                    dRight = leftCols - rightCols;
                }
                else
                {
                    sLeft = 0;
                    sRight = sourceCols - 1;

                    dLeft = 0;
                    dRight = 0;
                }

                //Blit both sides from the source grid to the correct locations in the foldedGrid
                BlitGrid(sourceGrid, sLeft, 0, foldedGrid, dLeft, 0, newRows, leftCols, 1, 1);
                BlitGrid(sourceGrid, sRight, 0, foldedGrid, dRight, 0, newRows, rightCols, -1, 1);
            }

            static void BlitGrid(int[][] source, int sx, int sy, int[][] dest, int dx, int dy, int rows, int cols, int stepX, int stepY)
            {
                var dx2 = dx;
                var sx2 = sx;

                for (var r = 0; r < rows; r++)
                {
                    dx = dx2;
                    sx = sx2;

                    for (var c = 0; c < cols; c++)
                    {
                        dest[dy][dx] += source[sy][sx];
                        dx++;
                        sx += stepX;
                    }

                    dy++;
                    sy += stepY;
                }
            }

            sourceGrid = foldedGrid;
        }

        return foldedGrid;
    }

    private static void DisplayResults(int[][] grid)
    {
        var rows = grid.Length;
        var cols = grid[0].Length;

        //Display the results
        for (var r = 0; r < rows; r++)
        {
            for (var c = 0; c < cols; c++)
            {
                if (grid[r][c] > 0)
                    Console.Write('#');
                else
                    Console.Write('.');
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    private readonly record struct Point(int X, int Y)
    {
        public static Point Parse(string input)
        {
            var data = input.Split(',');
            return new Point(int.Parse(data[0]), int.Parse(data[1]));
        }
    }
}