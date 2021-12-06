public class Day2
{
    public static void Run()
    {
        var lines = File.ReadAllLines("Input2.txt");
        var commands = lines.Select(line => new Command(line));

        Console.WriteLine("RUNNING DAY #2");
        Console.WriteLine($"Part1: {Part1(commands)}");
        Console.WriteLine($"Part2: {Part2(commands)}");
    }

    private class Command
    {
        public readonly string Action;
        public readonly int Value;

        public Command(string line)
        {
            var parts = line.Split(' ');

            Action = parts[0];
            Value = int.Parse(parts[1]);
        }
    }

    private static int Part1(IEnumerable<Command> commands)
    {
        var position = 0;
        var depth = 0;

        foreach(var command in commands)
        {
            var change = Execute(command);
            position += change.position;
            depth += change.depth;
        }

        static (int position, int depth) Execute(Command command)
        {
            return command.Action switch
            {
                "forward" => (command.Value, 0),
                "down" => (0, command.Value),
                "up" => (0, -command.Value),
                _ => (0, 0),
            };
        }

        return position * depth;
    }

    private static int Part2(IEnumerable<Command> commands)
    {
        var position = 0;
        var depth = 0;
        var aim = 0;

        foreach (var command in commands)
        {
            var change = Execute(command, aim);

            position += change.position;
            depth += change.depth;
            aim += change.aim;
        }

        static (int position, int depth, int aim) Execute(Command command, int aim)
        {
            return command.Action switch
            {
                "forward" => (command.Value, aim * command.Value, 0),
                "down" => (0, 0, command.Value),
                "up" => (0, 0, -command.Value),
                _ => (0, 0, 0),
            };
        }

        return position * depth;
    }
}