public class Day3
{
    public static void Run()
    {
        var lines = File.ReadAllLines("Input3.txt");

        Console.WriteLine("RUNNING DAY #3");
        Console.WriteLine($"Part1: {Part1(lines)}");
        //Console.WriteLine($"Part2: {Part2(lines)}");
    }

    private static int Part1(string[] lines)
    {
        var bitCount = lines[0].Length;     //Total number of bits in each input - e.g. 10101 = 5 bits of data
        var maxValue = (1 << bitCount) - 1; //The maximum value of a line if all bits are 1 - e.g. 11111 (used to find epsilon from gamma)
        int gamma = 0;

        //Look at each bit (MSB to LSB)
        for (var i = 0; i < bitCount; i++)
        {
            int ones = 0;
            int zeros = 0;

            foreach (var line in lines)
            {
                if (line[i] == '0') zeros++;
                else ones++;
            }

            //Set the bit if there are more ones; otherwise leave it unset as 0
            //e.g.
            //16 8 4 2 1
            // 0 1 0 0 0 <- set the bit worth 8 to on
            gamma += ones > zeros 
                ? 1 << (bitCount - i - 1) 
                : 0;
        }

        var epsilon = maxValue - gamma; 
        return gamma * epsilon;
    }
}