public class Day4
{
    public static void Run()
    {
        var lines = File.ReadAllLines("Input4.txt");

        Console.WriteLine($"RUNNING {nameof(Day4)}");
        Console.WriteLine($"Part1: {Part1(lines)}");
        Console.WriteLine($"Part2: {Part2(lines)}");
    }

    private static int Part1(string[] lines)
    {
        var (numbers, cards) = SetupGame(lines);

        //Play bingo
        foreach (var number in numbers)
        {
            foreach (var card in cards)
            {
                card.MarkNumber(number);

                if (card.IsWinner)
                    return number * card.UnmarkedSum;
            }
        }

        return 0;
    }

    private static int Part2(string[] lines)
    {
        var (numbers, cards) = SetupGame(lines);

        //Play bingo
        var remainingCards = new List<Card>(cards);
        var winningCards = new List<Card>();
        foreach (var number in numbers)
        {
            foreach (var card in remainingCards)
            {
                card.MarkNumber(number);

                if (card.IsWinner)
                    winningCards.Add(card);
            }

            remainingCards = remainingCards.Except(winningCards).ToList();

            if (remainingCards.Count == 0)
                return number * winningCards.Last().UnmarkedSum;

            winningCards.Clear();
        }

        return 0;
    }

    class Card
    {
        public const int ROWS = 5;
        public const int COLS = 5;

        private readonly int[][] _numbers; //[row][column]
        private readonly HashSet<int> _markedNumbers = new();

        public Card(IList<string> lines)
        {
            _numbers = new int[lines.Count][];

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
            _markedNumbers.Add(number);
        }

        public bool IsWinner
        {
            get
            {
                for (var row = 0; row < ROWS; row++)
                {
                    var markedCount = 0;
                    for (var col = 0; col < COLS; col++)
                    {
                        if (_markedNumbers.Contains(_numbers[row][col]))
                            markedCount++;
                    }

                    if (markedCount == COLS)
                        return true;
                }

                for (var col = 0; col < COLS; col++)
                {
                    var markedCount = 0;
                    for (var row = 0; row < ROWS; row++)
                    {
                        if (_markedNumbers.Contains(_numbers[row][col]))
                            markedCount++;
                    }

                    if (markedCount == ROWS)
                        return true;
                }


                return false;
            }
        }

        public int UnmarkedSum
        {
            get
            {
                var total = 0;
                for (var row = 0; row < ROWS; row++)
                {
                    for (var col = 0; col < COLS; col++)
                    {
                        var number = _numbers[row][col];
                        if (_markedNumbers.Contains(number) == false)
                            total += number;
                    }
                }

                return total;
            }
        }
    }

    private static (IEnumerable<int> numbers, IEnumerable<Card> cards) SetupGame(string[] lines)
    {
        var numbers = lines[0].Split(',').Select(number => int.Parse(number));
        var cards = new List<Card>();

        var cardLines = new List<string>();
        foreach (var line in lines.Skip(1))
        {
            if (string.IsNullOrEmpty(line))
                continue;

            cardLines.Add(line);
            if (cardLines.Count == Card.ROWS)
            {
                cards.Add(new Card(cardLines));
                cardLines.Clear();
            }
        }

        return (numbers, cards);
    }
}