public class Day4
{
    public static void Run()
    {
        var lines = File.ReadAllLines("Input4.txt");

        Console.WriteLine("--- Day 4: Giant Squid ---");
        Console.WriteLine($"Part1: {Part1(lines)}");
        Console.WriteLine($"Part2: {Part2(lines)}");
    }

    private static int Part1(string[] lines)
    {
        //====================================================================================================
        //To guarantee victory against the giant squid, figure out which board will win first.
        //What will your final score be if you choose that board?
        //====================================================================================================

        var (numbers, cards) = SetupGame(lines);

        //Play bingo
        foreach (var number in numbers)
        {
            foreach (var card in cards)
            {
                if (card.MarkNumber(number))
                    return number * card.UnmarkedSum;
            }
        }

        return 0;
    }

    private static int Part2(string[] lines)
    {
        //====================================================================================================
        //Figure out which board will win last.
        //Once it wins, what would its final score be?
        //====================================================================================================

        var (numbers, cards) = SetupGame(lines);

        var remainingCards = new List<Card>(cards);
        var winningCards = new List<Card>();

        //Play bingo
        foreach (var number in numbers)
        {
            foreach (var card in remainingCards)
            {
                if (card.MarkNumber(number))
                    winningCards.Add(card);
            }

            remainingCards = remainingCards.Except(winningCards).ToList();

            if (remainingCards.Count == 0)
                return number * winningCards.Last().UnmarkedSum;

            winningCards.Clear();
        }

        return 0;
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

    class Card
    {
        public const int ROWS = 5;
        public const int COLS = 5;

        //1d array of numbers row0: col0-4 => row1: col0-4, etc
        private readonly int[] _numbers;

        //Initialise with all the numbers on the card and remove when they are marked
        private readonly HashSet<int> _unmarkedNumbers = new();

        //A map of rows/cols with a count of marked numbers in each one - if the count in either gets to 5 we have a winner
        private readonly Dictionary<int, int> _markedRows = new(); 
        private readonly Dictionary<int, int> _markedCols = new(); 

        public int UnmarkedSum => _unmarkedNumbers.Sum();

        public Card(IList<string> lines)
        {
            var numbers = new List<int>();

            foreach (var line in lines)
            {
                var lineNumbers = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
                numbers.AddRange(lineNumbers);
            }

            _numbers = numbers.ToArray();

            _unmarkedNumbers = _numbers.ToHashSet();
        }

        //Marks a number on the card if it exists - returns true if marking that number results in a winning card
        public bool MarkNumber(int number)
        {
            //Only mark numbers that actually exist on this card
            var index = Array.IndexOf(_numbers, number);
            if (index == -1)
                return false;

            _unmarkedNumbers.Remove(number);

            //Mark the row this number is in
            var row = index / ROWS;
            MarkRowOrColContainsNumber(_markedRows, row);

            if (_markedRows.Where(r => r.Value == ROWS).Any())
                return true;

            //Mark the column this number is in
            var col = index % COLS;
            MarkRowOrColContainsNumber(_markedCols, col);

            if (_markedCols.Where(c => c.Value == COLS).Any())
                return true;

            return false;

            static void MarkRowOrColContainsNumber(Dictionary<int, int> state, int key)
            {
                //Mark this row as containing another number
                if (state.ContainsKey(key) == false)
                    state[key] = 0;

                state[key] = state[key] + 1;
            }
        }
    }
}