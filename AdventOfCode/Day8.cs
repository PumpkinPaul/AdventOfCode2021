using System.Text;

public class Day8
{
    public static void Run()
    {
        var lines = File.ReadAllLines("Input8.txt");

        Console.WriteLine("--- Day 8: Seven Segment Search ---");

        Console.WriteLine($"Part1: {Part1(lines)}");
        Console.WriteLine($"Part2: {Part2(lines)}");
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
            foreach (var value in signalValues)
            {
                //1, 4, 7, 8 are numbers with a unique number of segments
                //Using 1 we can work out what the top bit of 7 is

                if (value.Length == 2)
                    one = SortString(value);
                else if (value.Length == 3)
                    seven = SortString(value);
                else if (value.Length == 4)
                    four = SortString(value);
                else if (value.Length == 7)
                    eight = SortString(value);
            }

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

            //Now decode the output values
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