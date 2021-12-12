public class Day12
{
    public static void Run()
    {
        Console.WriteLine("--- Day 12: Passage Pathing ---");

        var test1 = File.ReadAllLines("Test12_1.txt");
        var test2 = File.ReadAllLines("Test12_2.txt");
        var test3 = File.ReadAllLines("Test12_3.txt");
        Console.WriteLine($"Test1: {Part1(test1)}"); //10
        Console.WriteLine($"Test1: {Part1(test2)}"); //19
        Console.WriteLine($"Test1: {Part1(test3)}"); //226
        //Console.WriteLine($"Test2: {Part2(test)}"); //

        var lines = File.ReadAllLines("Input12.txt");
        Console.WriteLine($"Part1: {Part1(lines)}"); //4104
        //Console.WriteLine($"Part2: {Part2(lines)}"); //
    }


    private enum RouteStatuses { FOUND_VALID_ROUTE, CONTINUE }

    private static long Part1(string[] lines)
    {
        //====================================================================================================
        //How many paths through this cave system are there that visit small caves at most once?
        //====================================================================================================

        var routes = new List<string>();
        var caves = LoadCaveSystem(lines);

        Navigate("start", "");

        void Navigate(string name, string route)
        {
            var cave = caves[name];

            //Apply cave state from route
            if (string.IsNullOrWhiteSpace(route) == false)
            {
                //Reset all caves as not visted...
                foreach (var k in caves)
                    k.Value.Visited = false;

                //...set visited caves
                foreach (var caveKey in route.Split(','))
                    caves[caveKey].Visited = true;
            }

            //Can only visit small caves once
            if (cave.CaveType != CaveTypes.LARGE && cave.Visited)
                return;

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
                Navigate(connectedCave.Key, route);
        }

        //foreach (var route in routes)
        //    Console.WriteLine(route);

        return routes.Count;
    }

    private static long Part2(string[] lines)
    {
        //====================================================================================================
        //
        //====================================================================================================

        return 0;
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

    public enum CaveTypes { START, LARGE, SMALL, END };

    public class Cave
    {
        public readonly string Name;
        public readonly CaveTypes CaveType;
        public bool Visited;
        public readonly Dictionary<string, Cave> Connected = new();

        public Cave(string name)
        {
            Name = name;
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