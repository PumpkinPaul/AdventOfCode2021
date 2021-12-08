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
            var signalValues = parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var outputValues = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var segments = new Dictionary<string, int>();

            //Decode the signal to work out what number each alpha value is
            var zero = "";
            var one = "";
            var two = "";
            var three = "";
            var four = "";
            var five = "";
            var six = "";
            var seven = "";
            var eight = "";
            var nine = "";

            //1, 4, 7, 8 are numbers with a unique number of segments
            foreach (var value in signalValues)
            {
                if (value.Length == 2)
                    one = SortString(value);
                else if (value.Length == 3)
                    seven = SortString(value);
                else if (value.Length == 4)
                    four = SortString(value);
                else if (value.Length == 7)
                    eight = SortString(value);
            }

            //0, 6, 9 all have 6 characters - we can compare the segments to known numbers above to work them out
            //I'm sure we could probably use a mask here
            foreach (var value in signalValues)
            {
                if (value.Length == 6)
                {
                    if (ContainsAllChars(value, four))
                        nine = SortString(value);
                    else if (ContainsAllChars(value, seven))
                        zero = SortString(value);
                    else
                        six = SortString(value);
                }
            }

            //2, 3, 5 all have 5 characters - we can compare the segments to known numbers above to work them out
            //5 and 2 are tricky, however. There's not a unique mask
            //5 does look mostly like a 6 though, with one fewer segment - use that knowledge to set it - the remaining number must be 2 then
            foreach (var value in signalValues)
            {
                if (value.Length == 5)
                {
                    if (ContainsAllChars(value, seven))
                        three = SortString(value);
                    else if (ContainsChars(value, six, six.Length - 1))
                        five = SortString(value);
                    else
                        two = SortString(value);
                }
            }

            //Now decode the output values for this line
            //Concatenate each decoded alpha sequence number to the result and simply convert to a number once the block of 4 values hass been processed
            //e.g. (using example data)
            //abc df abcefg abc
            //  4  1      6   4
            //= 4164
            var result = "";
            foreach (var value in outputValues)
            {
                var orderedValue = SortString(value);

                if (orderedValue == zero) result += "0";
                else if (orderedValue == one) result += "1";
                else if (orderedValue == two) result += "2";
                else if (orderedValue == three) result += "3";
                else if (orderedValue == four) result += "4";
                else if (orderedValue == five) result += "5";
                else if (orderedValue == six) result += "6";
                else if (orderedValue == seven) result += "7";
                else if (orderedValue == eight) result += "8";
                else if (orderedValue == nine) result += "9";
            }

            //Add the total of this line to the grand total
            sum += int.Parse(result);

            static string SortString(string input)
            {
                var characters = input.ToCharArray();
                Array.Sort(characters);
                return new string(characters);
            }

            static bool ContainsAllChars(string input, string mask)
            {
                foreach (var c in mask)
                {
                    if (input.Contains(c) == false)
                        return false;
                }

                return true;
            }

            static bool ContainsChars(string input, string mask, int target)
            {
                var matched = 0;
                foreach (var c in mask)
                {
                    if (input.Contains(c))
                        matched++;
                }

                return matched == target;
            }
        }

        return sum;
    }
}