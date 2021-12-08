﻿public class Day4
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

        //1d array of numbers row0: col0-4 => row1: col0-4, etc
        private readonly int[] _numbers;
        private readonly HashSet<int> _markedNumbers = new();

        private readonly Dictionary<int, int> _markedRows = new();
        private readonly Dictionary<int, int> _markedCols = new();

        public bool IsWinner { get; private set; }

        public Card(IList<string> lines)
        {
            var numbers = new List<int>();

            foreach (var line in lines)
            {
                var lineNumbers = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
                numbers.AddRange(lineNumbers);
            }

            _numbers = numbers.ToArray();
        }

        public void MarkNumber(int number)
        {
            //Only mark numbers that actually exist on this card
            var index = Array.IndexOf(_numbers, number);
            if (index == -1)
                return;

            _markedNumbers.Add(number);

            //Get the coordinates of the number
            var row = index / ROWS;
            var col = index % COLS;

            if (_markedRows.ContainsKey(row) == false) 
                _markedRows[row] = 0;

            _markedRows[row] = _markedRows[row] + 1;

            if (_markedCols.ContainsKey(col) == false)
                _markedCols[col] = 0;

            _markedCols[col] = _markedCols[col] + 1;

            var winningRow = _markedRows.Where(r => r.Value == ROWS).Any();
            var winningCol = _markedCols.Where(c => c.Value == COLS).Any();

            if (winningRow || winningCol)
                IsWinner = true;
        }

        public int UnmarkedSum => _numbers.Where(n => !_markedNumbers.Contains(n)).Sum();
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