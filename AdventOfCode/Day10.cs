using System.Collections.ObjectModel;

public class Day10
{
    public static void Run()
    {
        Console.WriteLine("--- Day 10: Syntax Scoring ---");

        var test = File.ReadAllLines("Test10.txt");
        //Console.WriteLine($"Test1: {Part1(test)}"); //26397
        //Console.WriteLine($"Test2: {Part2(test)}"); //

        var lines = File.ReadAllLines("Input10.txt");
        Console.WriteLine($"Part1: {Part1(lines)}");   //392421
        //Console.WriteLine($"Part2: {Part2(lines)}"); //
    }

    private static readonly ReadOnlyDictionary<char, char> _chunks = new(new Dictionary<char, char>() { { '(', ')' }, { '[', ']' }, { '{', '}' }, { '<', '>' } });
    private static readonly ReadOnlyDictionary<char, int> _errorCosts = new(new Dictionary<char, int> { { ')', 3 }, { ']', 57 }, { '}', 1197 }, { '>', 25137 } });

    private static long Part1(string[] lines)
    {
        //====================================================================================================
        //Find the first illegal character in each corrupted line of the navigation subsystem.
        //What is the total syntax error score for those errors?
        //====================================================================================================
        var errors = _chunks.Values.ToDictionary(key => key, value => 0);

        //Iterate the line, lookup the expected closing char from the opening char and add to the stack.
        foreach (var line in lines)
        {
            var expectedEndChars = new Stack<char>();
            foreach (var c in line)
            {
                //Determine if the char is opening or closing 
                if (_chunks.ContainsKey(c))
                {
                    //It's an opening char so expect the corresponding closing char
                    expectedEndChars.Push(_chunks[c]);
                }
                else
                {
                    //It's a closing char but is it the expected one
                    var expectedEndChar = expectedEndChars.Pop();
                    if (c != expectedEndChar)
                    {
                        //No! Found a corrupted char so log it as an error
                        errors[c] += 1;
                        break;
                    }
                }
            }
        }

        return errors
            .Select(error => error.Value * _errorCosts[error.Key])
            .Sum();
    }

    private static long Part2(string[] lines)
    {
        //====================================================================================================
        //
        //====================================================================================================

        return 0;
    }
}