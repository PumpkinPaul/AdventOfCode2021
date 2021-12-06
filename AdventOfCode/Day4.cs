public class Day4
{
    public static void Run()
    {
        var lines = File.ReadAllLines("Input4.txt");

        Console.WriteLine($"RUNNING {nameof(Day4)}");
        Console.WriteLine($"Part1: {Part1(lines)}");
        Console.WriteLine($"Part2: {Part2(lines)}");
    }

    class Card
    {
        public const int CARD_ROWS = 5;
        public const int CARD_COLS = 5;

        private readonly int[][] _numbers; //[row][column]

        public Card(IList<string> lines)
        {
            _numbers = new int[lines.Count()][];

            var row = 0;
            foreach (var line in lines)
            {
                var lineNumbers = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(number => int.Parse(number));

                _numbers[row] = lineNumbers.ToArray();

                row++;
            }
        }

        public void MarkNumber(int number)
        {

        }

        public bool IsWinner
        {
            get
            {
                return false;
            }
        }

        public int SumUnmarked
        {
            get
            {
                return 0;
            }
        }
    }

    private static int Part1(string[] lines)
    {
        var numbers = lines[0].Split(',').Select(number => int.Parse(number));
        var cards = new List<Card>();

        var cardLines = new List<string>();
        foreach (var line in lines.Skip(1))
        {
            if (string.IsNullOrEmpty(line))
                continue;

            cardLines.Add(line);
            if (cardLines.Count == Card.CARD_ROWS)
            {
                cards.Add(new Card(cardLines));
                cardLines.Clear();
            }            
        }

        foreach(var number in numbers)
        {
            foreach (var card in cards)
            {
                card.MarkNumber(number);

                if (card.IsWinner)
                {
                    return number * card.SumUnmarked;
                }
            }
        }

        return 0;
    }

    private static int Part2(string[] lines)
    {
        return 0;
    }
}