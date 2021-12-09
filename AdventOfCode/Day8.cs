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
        //We need to count instances of the following numbers 1, 4, 7, 8 - these are easy as they have a unique number of segments used to make them
        //2 segments make the number 1
        //4 segments make the number 4
        //3 segments make the number 7
        //7 segments make the number 8
        var sevenSegmentDigits = new HashSet<int> { { 2 }, { 4 }, { 3 }, { 7 } };

        return lines
            .SelectMany(ParseOutputValues)
            .Select(value => value.Length)
            .Count(sevenSegmentDigits.Contains);

        static IEnumerable<string> ParseOutputValues(string line) =>
            line.Split('|')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    private static long Part2(string[] lines)
    {
        //Process each line one at a time
        //First parse the line to get the signals and the output values
        //Next decode the signals so that we know what number each alpha block of chars is
        //Finally use the decoded signal values to decode the output values summing the results as we go

        var sum = 0;

        foreach (var line in lines)
        {
            var parts = line.Split('|');
            var signalValues = ParseCodedValues(parts[0]);
            var outputValues = ParseCodedValues(parts[1]);

            //Decode the signal to work out what number each alpha value is
            var decodedSignalValues = new string[10];

            //1, 4, 7, 8 are numbers with a unique number of segments - create a map of segment count to the corresponding number
            var segmentCountNumberMap = new Dictionary<int, int> { { 2, 1 }, { 3, 7 }, { 4, 4 }, { 7, 8 } };

            foreach (var value in signalValues)
            {
                if (segmentCountNumberMap.TryGetValue(value.Length, out int number))
                    decodedSignalValues[number] = value;
            }

            //0, 6, 9 all have 6 characters - we compare these characters to previously set numbers
            //e.g.
            //9 contains all the segments from a 4. 0 or 6 don't so that means it must be a match
            //0 contains all the segments from a 7. 6 doesn't so that means it must be a match
            //we are left with 6 as the only other letter with 6 chars
            foreach (var value in signalValues.Where(value => value.Length == 6))
            {
                if (ContainsAllChars(value, decodedSignalValues[4]))
                    decodedSignalValues[9] = value;
                else if (ContainsAllChars(value, decodedSignalValues[7]))
                    decodedSignalValues[0] = value;
                else
                    decodedSignalValues[6] = value;
            }

            //2, 3, 5 all have 5 characters - we compare these characters to previously set numbers
            //5 and 2 are tricky, however. There's not a unique mask
            //5 does look mostly like a 6 though, with one fewer segment - use that knowledge to set it - the remaining number must be 2 then
            foreach (var value in signalValues.Where(value => value.Length == 5))
            {
                if (ContainsAllChars(value, decodedSignalValues[7]))
                    decodedSignalValues[3] = value;
                else if (ContainsChars(value, decodedSignalValues[6], decodedSignalValues[6].Length - 1))
                    decodedSignalValues[5] = value;
                else
                    decodedSignalValues[2] = value;
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
        }

        //Returns a sequence of strings sorting each value in alphabetical order e.g. cda efa bad => adc aef abd
        static IEnumerable<string> ParseCodedValues(string input) => input.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(SortString);

        //Sort the characters in string e.g. defga => adefg
        static string SortString(string input) => string.Concat(input.OrderBy(c => c));

        //Returns true if input contains all of the characters in the mask
        static bool ContainsAllChars(string input, string mask) => ContainsChars(input, mask, mask.Length);

        //Returns true if input contains the specified number of characters in the mask
        static bool ContainsChars(string input, string mask, int target) => target == mask.Count(c => input.Contains(c));

        return sum;
    }
}