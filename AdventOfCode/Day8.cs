public class Day8
{
    public static void Run()
    {
        var lines = File.ReadAllLines("Input8.txt");

        Console.WriteLine("--- Day 8: Seven Segment Search ---");

        Console.WriteLine($"Part1: {Part1(lines)}"); //274
        Console.WriteLine($"Part2: {Part2(lines)}"); //1012089
    }

    private static long Part1(string[] lines)
    {
        //A count of the number of segments for the numbers we are interested in counting (1, 4, 7, 8)
        var sevenSegmentDigits = new HashSet<int> { { 2 }, { 4 }, { 3 }, { 7 } };

        return lines
            .SelectMany(ParseOutputValues)
            .Count(value => sevenSegmentDigits.Contains(value.Length));

        static IEnumerable<string> ParseOutputValues(string line) =>
            line.Split('|')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    private static long Part2(string[] lines)
    {
        var sum = 0;

        //Process each line one at a time
        //First parse the line to get the signals and the output values
        //Next decode the signals so that we know what number each alpha block of chars is
        //Finally use the decoded signal values to decode the output values summing the results as we go

        foreach (var line in lines)
        {
            var parts = line.Split('|');
            var signalValues = parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(SortString);
            var outputValues = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(SortString);

            var segments = new Dictionary<string, int>();

            //Decode the signal to work out what number each alpha value is
            var decodedSignalValues = new string[10];

            //1, 4, 7, 8 are numbers with a unique number of segments
            foreach (var value in signalValues)
            {
                if (value.Length == 2)
                    //one = value;
                    decodedSignalValues[1] = value;
                else if (value.Length == 3)
                    decodedSignalValues[7] = value;
                else if (value.Length == 4)
                    decodedSignalValues[4] = value;
                else if (value.Length == 7)
                    decodedSignalValues[8] = value;
            }

            //0, 6, 9 all have 6 characters - we can compare the segments to known numbers above to work them out
            //I'm sure we could probably use a mask here
            foreach (var value in signalValues)
            {
                if (value.Length == 6)
                {
                    if (ContainsAllChars(value, decodedSignalValues[4]))
                        decodedSignalValues[9] = value;
                    else if (ContainsAllChars(value, decodedSignalValues[7]))
                        decodedSignalValues[0] = value;
                    else
                        decodedSignalValues[6] = value;
                }
            }

            //2, 3, 5 all have 5 characters - we can compare the segments to known numbers above to work them out
            //5 and 2 are tricky, however. There's not a unique mask
            //5 does look mostly like a 6 though, with one fewer segment - use that knowledge to set it - the remaining number must be 2 then
            foreach (var value in signalValues)
            {
                if (value.Length == 5)
                {
                    if (ContainsAllChars(value, decodedSignalValues[7]))
                        decodedSignalValues[3] = value;
                    else if (ContainsChars(value, decodedSignalValues[6], decodedSignalValues[6].Length - 1))
                        decodedSignalValues[5] = value;
                    else
                        decodedSignalValues[2] = value;
                }
            }

            //Now decode the output values for this line
            //Join each decoded alpha sequence number value to the result and simply convert to a number once the block of 4 values hass been processed
            //e.g. (using example data)
            //abc df abcefg abc
            //  4  1      6   4
            //= 4164
            var result = string.Join("", outputValues.Select(value => Array.IndexOf(decodedSignalValues, value).ToString()));

            //Add the total of this line to the grand total
            sum += int.Parse(result);

            //Sort the characters in string e.g. defga => adefg
            static string SortString(string input) => string.Concat(input.OrderBy(c => c));

            //Returns true if input contains all of the characters in the mask
            static bool ContainsAllChars(string input, string mask) => ContainsChars(input, mask, mask.Length);

            //Returns true if input contains the specified number of characters in the mask
            static bool ContainsChars(string input, string mask, int target) => target == mask.Count(c => input.Contains(c));
        }

        return sum;
    }
}