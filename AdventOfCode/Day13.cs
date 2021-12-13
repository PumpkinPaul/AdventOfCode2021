public class Day13
{
    public static void Run()
    {
        Console.WriteLine("--- Day 13: Transparent Origami ---");

        //var test = File.ReadAllLines("Test13.txt");
        //Console.WriteLine($"Test1(1): {Part1(test)}"); //17
        //Part2(test);                                   //O

        var lines = File.ReadAllLines("Input13.txt");
        Console.WriteLine($"Part1: {Part1(lines)}"); //701
        Part2(lines);                                //FPEKBEJL 
    }

    public static long Part1(string[] lines)
    {
        //====================================================================================================
        //How many paths through this cave system are there that visit small caves at most once?
        //====================================================================================================

        var (grid, folds) = LoadInput(lines);

        var foldedGrid = FoldGrid(grid, folds.Take(1));

        var (rows, cols) = GetGridDimensions(foldedGrid);

        //Count the dots
        var dots = 0;
        ProcessGrid(foldedGrid, (r, c) => { if (foldedGrid[r][c] > 0) dots++; });

        return dots;
    }

    public static long Part2(string[] lines)
    {
        //====================================================================================================
        //Given these new rules, how many paths through this cave system are there?
        //====================================================================================================

        var (grid, folds) = LoadInput(lines);

        var foldedGrid = FoldGrid(grid, folds);

        ProcessGrid(
            foldedGrid, 
            (r, c) => Console.Write(foldedGrid[r][c] > 0 ? '#' : '.'),
            () => Console.WriteLine());

        return 0;
    }

    private static (int[][], IEnumerable<string>) LoadInput(string[] lines)
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

        return (grid, folds);
    }

    private static int[][] CreateGrid(int rows, int cols)
    {
        var grid = new int[rows][];
        for (var r = 0; r < rows; r++)
            grid[r] = new int[cols];
     
        return grid;
    }

    private static int[][] FoldGrid(
        int[][] grid,
        IEnumerable<string> folds)
    {
        int[][] foldedGrid = Array.Empty<int[]>();
        
        var sourceGrid = grid;

        foreach (var fold in folds)
        {
            var foldData = fold.Split('=');
            var foldDimension = foldData[0].Split(' ').Last();
            var foldValue = int.Parse(foldData[1]);

            //There are asymetrical folds (sneaky part 2!) - this is when the fold is not in the middle of the paper
            //Determine the size of a new grid - it will be the dimensions of the largest side
            //(e.g. if lhs > rhs it will be the size of the lhs of the fold - same deal with top and bottom)
            var (sourceRows, sourceCols) = GetGridDimensions(sourceGrid);
            var (newRows, newCols) = (sourceRows, sourceCols);

            if (foldDimension == "y")
            {
                var sourceTop = 0;
                var sourceBottom = sourceRows - 1;
                
                var destinationTop = 0;
                var destinationBottom = 0;

                //Get the sizes of the paper to the top and bottom of the fold
                var topRows = foldValue;
                var bottomRows = sourceRows - topRows - 1;

                newRows = Math.Max(topRows, bottomRows);

                //Create a new blank grid that will be large enough to contain the largest side of the fold
                foldedGrid = CreateGrid(newRows, newCols);

                if (bottomRows > topRows)
                {
                    destinationTop = bottomRows + 1;
                    destinationBottom = 0;
                }
                else
                {
                    destinationTop = 0;
                    destinationBottom = topRows - bottomRows;
                }

                //Blit both sides from the source grid to the correct locations in the foldedGrid
                BlitGrid(sourceGrid, 0, sourceTop, foldedGrid, 0, destinationTop, topRows, newCols, 1, 1);
                BlitGrid(sourceGrid, 0, sourceBottom, foldedGrid, 0, destinationBottom, bottomRows, newCols, 1, -1);
            }
            else
            {
                var sourceLeft = 0;
                var sourceRight = sourceCols - 1;

                var destinationLeft = 0;
                var destinationRight = 0;

                //Get the sizes of the paper to the left and right of the fold
                var leftCols = foldValue;
                var rightCols = sourceCols - leftCols - 1;

                newCols = Math.Max(leftCols, rightCols);

                //Create a new blank grid that will be large enough to contain the largest side of the fold
                foldedGrid = CreateGrid(newRows, newCols);

                if (rightCols > leftCols)
                {
                    destinationLeft = rightCols + 1;
                    destinationRight = 0;
                }
                else
                {
                    destinationLeft = 0;
                    destinationRight = leftCols - rightCols;
                }

                //Blit both sides from the source grid to the correct locations in the foldedGrid
                BlitGrid(sourceGrid, sourceLeft, 0, foldedGrid, destinationLeft, 0, newRows, leftCols, 1, 1);
                BlitGrid(sourceGrid, sourceRight, 0, foldedGrid, destinationRight, 0, newRows, rightCols, -1, 1);
            }

            sourceGrid = foldedGrid;
        }

        return foldedGrid;

        static void BlitGrid(int[][] source, int sx, int sy, int[][] dest, int dx, int dy, int rows, int cols, int stepX, int stepY)
        {
            var dx2 = dx;
            var sx2 = sx;

            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < cols; c++)
                {
                    dest[dy][dx] += source[sy][sx];
                    dx++;
                    sx += stepX;
                }

                dx = dx2;
                sx = sx2;
                dy++;
                sy += stepY;
            }
        }
    }

    private static (int, int) GetGridDimensions(int[][] grid) => (grid.Length, grid[0].Length);

    private static void ProcessGrid(int[][] grid, Action<int, int> cellProcessor, Action rowProcessor = null)
    {
        var (rows, cols) = GetGridDimensions(grid);

        for (var r = 0; r < rows; r++)
        {
            for (var c = 0; c < cols; c++)
                cellProcessor(r, c);

            rowProcessor?.Invoke();
        }
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