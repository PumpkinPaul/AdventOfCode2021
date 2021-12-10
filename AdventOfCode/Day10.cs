using System.Collections.ObjectModel;

public class Day10
{
    public static void Run()
    {
        Console.WriteLine("--- Day 10: Syntax Scoring ---");

        var test = File.ReadAllLines("Test10.txt");
        Console.WriteLine($"Test1: {Part1(test)}"); //26397
        Console.WriteLine($"Test2: {Part2(test)}"); //288957

        var lines = File.ReadAllLines("Input10.txt");
        Console.WriteLine($"Part1: {Part1(lines)}"); //392421
        Console.WriteLine($"Part2: {Part2(lines)}"); //2769449099
    }

    private static readonly ReadOnlyDictionary<char, char> _chunks = new(new Dictionary<char, char>() {
        { '(', ')' },
        { '[', ']' },
        { '{', '}' },
        { '<', '>' }
    });

    private static long Part1(string[] lines)
    {
        //====================================================================================================
        //Find the first illegal character in each corrupted line of the navigation subsystem.
        //What is the total syntax error score for those errors?
        //====================================================================================================

        var _errorCosts = new Dictionary<char, int> {
            { ')',     3 },
            { ']',    57 },
            { '}',  1197 },
            { '>', 25137 }
        };

        var errors = _chunks.Values.ToDictionary(key => key, value => 0);

        ProcessLines(
            lines,
            //Line preprocessor: called at the start of each line process
            null,

            //Illegal character processor: called when an illegale end character is found
            (c) => errors[c] += 1,

            //Line postprocessor: called when all of the characters in a line have been processed
            null);

        return errors
            .Select(error => error.Value * _errorCosts[error.Key])
            .Sum();
    }

    private static long Part2(string[] lines)
    {
        //====================================================================================================
        //Find the completion string for each incomplete line, score the completion strings, and sort the scores.
        //What is the middle score?
        //====================================================================================================

        //The score for each missing closing character in incomplete lines
        var _charScores = new ReadOnlyDictionary<char, int>(new Dictionary<char, int>() {
            { ')', 1 },
            { ']', 2 },
            { '}', 3 },
            { '>', 4 }
        });

        var incompleteLineScores = new List<long>();

        var isLineCorrupted = false;

        ProcessLines(
            lines,
            //Line preprocessor: called at the start of each line process
            (line) => isLineCorrupted = false,

            //Illegal character processor: called when an illegale end character is found
            (c) => isLineCorrupted = true,
            
            //Line postprocessor: called when all of the characters in a line have been processed
            (line, expectedEndChars) => {
                //Any incomplete lines? These are lines that are not corrupted or complete
                if (isLineCorrupted || expectedEndChars.Count == 0) return;

                //This line is incomplete so calculate the score
                var lineScore = 0L;
                while (expectedEndChars.Count > 0)
                    lineScore = lineScore * 5 + _charScores[expectedEndChars.Pop()];

                incompleteLineScores.Add(lineScore);
            }
        ); 

        //Result is the middle score in the list of incomplete scores
        return incompleteLineScores
            .OrderBy(s => s)
            .ElementAt(incompleteLineScores.Count / 2);
    }

    private static void ProcessLines(
        string[] lines,
        Action<string>? linePreProcessor,
        Action<char> corruptedCharProcessor,
        Action<string, Stack<char>>? linePostProcessor)
    {
        //Iterate the line, lookup the expected closing char from the opening char and add to the stack.
        foreach (var line in lines)
        {
            linePreProcessor?.Invoke(line);

            var expectedEndChars = new Stack<char>();

            foreach (var c in line)
            {
                //Determine if the char is opening or closing 
                if (_chunks.ContainsKey(c))
                {
                    //It's an opening char so expect the corresponding closing char
                    expectedEndChars.Push(_chunks[c]);
                    continue;
                }

                //It's a closing char but is it the expected one
                if (c == expectedEndChars.Pop()) continue;

                //Found a corrupted char! Process it!
                corruptedCharProcessor(c);
                break;
            }

            linePostProcessor?.Invoke(line, expectedEndChars);
        }
    }
}