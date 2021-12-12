public class Day12
{
    public static void Run()
    {
        Console.WriteLine("--- Day 12: Passage Pathing ---");

        var test1 = File.ReadAllLines("Test12_1.txt");
        var test2 = File.ReadAllLines("Test12_2.txt");
        var test3 = File.ReadAllLines("Test12_3.txt");
        Console.WriteLine($"Test1(1): {Part1(test1)}"); //10
        Console.WriteLine($"Test1(2): {Part1(test2)}"); //19
        Console.WriteLine($"Test1(3): {Part1(test3)}"); //226
        
        Console.WriteLine($"Test2(1): {Part2(test1)}"); //36
        Console.WriteLine($"Test2(2): {Part2(test2)}"); //103
        Console.WriteLine($"Test2(3): {Part2(test3)}"); //3509

        var lines = File.ReadAllLines("Input12.txt");
        Console.WriteLine($"Part1: {Part1(lines)}"); //4104
        Console.WriteLine($"Part2: {Part2(lines)}"); //119760
    }

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
                    k.Value.Visited = 0;

                //...set visited caves
                foreach (var caveKey in route.Split(','))
                    caves[caveKey].Visited++;
            }

            //Can only visit small caves once
            if (cave.CaveType != CaveTypes.LARGE && cave.Visited == 1)
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

        return routes.Count;
    }

    private static long Part2(string[] lines)
    {
        //====================================================================================================
        //Given these new rules, how many paths through this cave system are there?
        //====================================================================================================

        //====================================================================================================
        //How many paths through this cave system are there that visit small caves at most once?
        //====================================================================================================

        var routes = new List<string>();
        var caves = LoadCaveSystem(lines);

        Navigate("start", "", "");

        void Navigate(string name, string route, string smallCaveVisitedTwice)
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
                        smallCaveVisitedTwice = cave.Name;

                    if (smallCaveVisitedTwice != cave.Name)
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
                Navigate(connectedCave.Key, route, smallCaveVisitedTwice);
        }

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

    public enum CaveTypes { START, LARGE, SMALL, END };

    public class Cave
    {
        public readonly string Name;
        public readonly CaveTypes CaveType;
        public int Visited;
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