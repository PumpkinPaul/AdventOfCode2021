public class Day5
{
    public static void Run()
    {
        var lines = File.ReadAllLines("Input5.txt");

        Console.WriteLine("--- Day 5: Hydrothermal Venture ---");
        Console.WriteLine($"Part1: {Part1(lines)}");
        Console.WriteLine($"Part2: {Part2(lines)}");
    }

    private static int Part1(string[] lines)
    {
        return Solve(lines, (lineSegment) => lineSegment.Start.X == lineSegment.End.X || lineSegment.Start.Y == lineSegment.End.Y);
    }

    private static int Part2(string[] lines)
    {
        return Solve(lines, (lineSegment) => true);
    }

    private static int Solve(string[] lines, Func<LineSegment, bool> predicate)
    {
        //Convert input into line segments
        var lineSegments = lines.Select(LineSegment.Parse);

        //Plot each point in each line segment
        var points = new Dictionary<Point, uint>();

        foreach (var lineSegment in lineSegments.Where(predicate))
        {
            var pointsOnLine = Bresenham.GetPoints(lineSegment.Start.X, lineSegment.Start.Y, lineSegment.End.X, lineSegment.End.Y);

            foreach (var point in pointsOnLine)
            {
                if (points.ContainsKey(point) == false)
                    points.Add(point, 1);
                else
                    points[point] = points[point] + 1;
            }
        }

        return points.Values.Count(count => count >= 2);
    }

    private readonly struct Point
    {
        public readonly int X;
        public readonly int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    private class LineSegment
    {
        public readonly Point Start;
        public readonly Point End;

        public LineSegment(Point start, Point end)
        {
            Start = start;
            End = end;
        }

        public static LineSegment Parse(string line)
        {
            //Line is in the following format
            //0,9 -> 5,9

            var parts = line.Split(' ');

            return new LineSegment(ParseCoordinate(parts[0]), ParseCoordinate(parts[2]));

            static Point ParseCoordinate(string coordinate)
            {
                var parts = coordinate.Split(',');
                return new Point(int.Parse(parts[0]), int.Parse(parts[1]));
            }
        }
    }

    private static class Bresenham
    {
        public static IEnumerable<Point> GetPoints(int x0, int y0, int x1, int y1)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                var t = x0; // swap x0 and y0
                x0 = y0;
                y0 = t;
                t = x1;     // swap x1 and y1
                x1 = y1;
                y1 = t;
            }

            if (x0 > x1)
            {
                var t = x0; // swap x0 and x1
                x0 = x1;
                x1 = t;
                t = y0;     // swap y0 and y1
                y0 = y1;
                y1 = t;
            }

            var dx = x1 - x0;
            var dy = Math.Abs(y1 - y0);
            var error = dx / 2;
            var ystep = (y0 < y1) ? 1 : -1;
            var y = y0;

            for (var x = x0; x <= x1; x++)
            {
                yield return new Point(steep ? y : x, steep ? x : y);
                error -= dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
            yield break;
        }
    }
}