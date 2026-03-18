using System;

namespace tictactoe
{
    public class PlayerInfo
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public int Wins { get; set; }
        public bool IsAI { get; private set; }

        public PlayerInfo(string name, string symbol, bool isAI = false)
        {
            Name = string.IsNullOrWhiteSpace(name) ? "Player" : name.Trim();
            Symbol = NormalizeSymbol(symbol);
            Wins = 0;
            IsAI = isAI;
        }

        private static string NormalizeSymbol(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException("Symbol must be provided and be 'X' or 'O'.", nameof(symbol));

            symbol = symbol.Trim().ToUpperInvariant();

            if (symbol.Length != 1 || (symbol != "X" && symbol != "O"))
                throw new ArgumentException("Symbol must be a single character: 'X' or 'O'.", nameof(symbol));

            return symbol;
        }

        public void IncrementWin()
        {
            Wins++;
        }

        public void ResetWins()
        {
            Wins = 0;
        }

        public override string ToString()
        {
            return Name + " - " + Wins;
        }
    }
}