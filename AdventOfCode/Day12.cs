public class Day12
{
    public static void Run()
    {
        Console.WriteLine("--- Day 12: Passage Pathing ---");

        //var test1 = File.ReadAllLines("Test12_1.txt");
        //var test2 = File.ReadAllLines("Test12_2.txt");
        //var test3 = File.ReadAllLines("Test12_3.txt");
        //Console.WriteLine($"Test1(1): {Part1(test1)}"); //10
        //Console.WriteLine($"Test1(2): {Part1(test2)}"); //19
        //Console.WriteLine($"Test1(3): {Part1(test3)}"); //226
        
        //Console.WriteLine($"Test2(1): {Part2(test1)}"); //36
        //Console.WriteLine($"Test2(2): {Part2(test2)}"); //103
        //Console.WriteLine($"Test2(3): {Part2(test3)}"); //3509

        var lines = File.ReadAllLines("Input12.txt");
        Console.WriteLine($"Part1: {Part1(lines)}"); //4104
        Console.WriteLine($"Part2: {Part2(lines)}"); //119760
    }

    public static long Part1(string[] lines)
    {
        //====================================================================================================
        //How many paths through this cave system are there that visit small caves at most once?
        //====================================================================================================

        var caves = LoadCaveSystem(lines);
        var routes = new List<string>();

        Navigate(caves, routes, "start", "", "n/a");

        return routes.Count;
    }

    public static long Part2(string[] lines)
    {
        //====================================================================================================
        //Given these new rules, how many paths through this cave system are there?
        //====================================================================================================

        var caves = LoadCaveSystem(lines);
        var routes = new List<string>();
        
        Navigate(caves, routes, "start", "", "");

        return routes.Count;
    }

    //Load the puzzle input energy values into a 2d array [rows][cols]
    private static Dictionary<string, Cave> LoadCaveSystem(string[] lines)
    {
        var caves = new Dictionary<string, Cave>();

        foreach (var line in lines)
        {
            var parts = line.Split('-', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var firstCave = parts[0];
            var secondCave = parts[1];

            //Add connections in both directions
            ConnectCaves(caves, firstCave, secondCave);
            ConnectCaves(caves, secondCave, firstCave);

            static void ConnectCaves(Dictionary<string, Cave> caves, string parent, string connected)
            {
                if (caves.ContainsKey(parent) == false)
                    caves[parent] = new Cave(parent);

                var conectedCaves = caves[parent];
                if (conectedCaves.Connected.ContainsKey(connected) == false)
                    conectedCaves.Connected[connected] = new Cave(connected);
            }
        }

        return caves;
    }

    private static void Navigate(Dictionary<string, Cave> caves, List<string> routes, string name, string route, string smallCaveVisitedTwice)
    {
        var cave = caves[name];

        //Apply cave state from route
        if (string.IsNullOrWhiteSpace(route) == false)
        {
            //Reset all caves as not visted...
            foreach (var k in caves)
                k.Value.Visited = 0;

            //...set visited caves
            foreach (var caveKey in route.Split(','))
                caves[caveKey].Visited++;
        }

        if (cave.CaveType == CaveTypes.START && cave.Visited == 1)
            return;

        if (cave.CaveType == CaveTypes.END && cave.Visited == 1)
            return;

        if (cave.CaveType == CaveTypes.SMALL)
        {
            if (cave.Visited == 1)
            {
                if (string.IsNullOrWhiteSpace(smallCaveVisitedTwice))
                    smallCaveVisitedTwice = name;

                if (smallCaveVisitedTwice != name)
                    return;
            }
            else if (cave.Visited == 2)
                return;
        }

        //This is a cave we can enter so append it to the current route
        route += (string.IsNullOrWhiteSpace(route) ? "" : ",") + name;

        //Flag the route as valid if we've reached the end cave
        if (cave.CaveType == CaveTypes.END)
        {
            routes.Add(route);
            return;
        }

        //Continue into caves that are connected to this cave...
        foreach (var connectedCave in cave.Connected)
            Navigate(caves, routes, connectedCave.Key, route, smallCaveVisitedTwice);
    }

    public enum CaveTypes { START, LARGE, SMALL, END };

    public class Cave
    {
        public readonly CaveTypes CaveType;
        public int Visited;
        public readonly Dictionary<string, Cave> Connected = new();

        public Cave(string name)
        {
            CaveType = ParseCaveType(name);
        }

        private static CaveTypes ParseCaveType(string name)
        {
            return name switch
            {
                "start" => CaveTypes.START,
                "end" => CaveTypes.END,
                _ => (name == name.ToUpper())
                    ? CaveTypes.LARGE
                    : CaveTypes.SMALL,
            };
        }
    }
}