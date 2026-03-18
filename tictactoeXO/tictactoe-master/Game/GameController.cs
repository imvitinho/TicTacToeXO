using System.Drawing;

namespace tictactoe
{
    public class GameController
    {
        public GameBoard Board { get; private set; }

        public GameMode CurrentGameMode { get; private set; }

        public PlayerInfo PlayerXInfo { get; private set; }
        public PlayerInfo PlayerOInfo { get; private set; }

        public string CurrentSymbol { get; private set; }

        public bool IsGameOver { get; private set; }
        public bool IsDraw { get; private set; }
        public bool IsAITurning { get; private set; }

        public string WinnerSymbol { get; private set; }

        public GameController()
        {
            Board = new GameBoard();
            CurrentSymbol = "X";
            WinnerSymbol = string.Empty;
        }

        public void ConfigureGame(GameMode gameMode)
        {
            CurrentGameMode = gameMode;

            if (gameMode == GameMode.MultiPlayer)
            {
                PlayerXInfo = new PlayerInfo("Jogador X", "X");
                PlayerOInfo = new PlayerInfo("Jogador O", "O");
            }
            else
            {
                PlayerXInfo = new PlayerInfo("Player", "X");
                PlayerOInfo = new PlayerInfo("AI", "O", true);
            }

            ResetBoardOnly();
        }

        public void ResetBoardOnly()
        {
            Board.Reset();
            CurrentSymbol = "X";
            IsGameOver = false;
            IsDraw = false;
            IsAITurning = false;
            WinnerSymbol = string.Empty;
        }

        public void ResetEntireMatch()
        {
            if (PlayerXInfo != null)
                PlayerXInfo.ResetWins();

            if (PlayerOInfo != null)
                PlayerOInfo.ResetWins();

            ResetBoardOnly();
        }

        public bool TryPlayMove(int row, int col)
        {
            if (IsGameOver)
                return false;

            if (IsAITurning)
                return false;

            if (!Board.SetCell(row, col, CurrentSymbol))
                return false;

            ProcessTurnResult();
            return true;
        }

        public void StartAITurn()
        {
            if (CurrentGameMode == GameMode.SinglePlayer &&
                !IsGameOver &&
                CurrentSymbol == PlayerOInfo.Symbol)
            {
                IsAITurning = true;
            }
        }

        public Point? PlayAIMove(TicTacToeAI ai)
        {
            if (CurrentGameMode != GameMode.SinglePlayer || IsGameOver)
            {
                IsAITurning = false;
                return null;
            }

            if (CurrentSymbol != PlayerOInfo.Symbol)
            {
                IsAITurning = false;
                return null;
            }

            IsAITurning = true;

            Point? move = ai.GetBestMove(Board, PlayerOInfo.Symbol, PlayerXInfo.Symbol);

            if (!move.HasValue)
            {
                IsAITurning = false;
                return null;
            }

            bool played = Board.SetCell(move.Value.X, move.Value.Y, PlayerOInfo.Symbol);

            if (!played)
            {
                IsAITurning = false;
                return null;
            }

            ProcessTurnResult();
            IsAITurning = false;

            return move;
        }

        public string GetWinnerName()
        {
            if (string.IsNullOrEmpty(WinnerSymbol))
                return string.Empty;

            if (WinnerSymbol == PlayerXInfo.Symbol)
                return PlayerXInfo.Name;

            return PlayerOInfo.Name;
        }

        private void ProcessTurnResult()
        {
            WinnerSymbol = Board.CheckWinner();

            if (!string.IsNullOrEmpty(WinnerSymbol))
            {
                IsGameOver = true;
                IsDraw = false;

                if (WinnerSymbol == PlayerXInfo.Symbol)
                    PlayerXInfo.IncrementWin();
                else if (WinnerSymbol == PlayerOInfo.Symbol)
                    PlayerOInfo.IncrementWin();

                return;
            }

            if (Board.IsFull())
            {
                IsGameOver = true;
                IsDraw = true;
                return;
            }

            SwitchPlayer();
        }

        private void SwitchPlayer()
        {
            CurrentSymbol = CurrentSymbol == "X" ? "O" : "X";
        }
    }
}