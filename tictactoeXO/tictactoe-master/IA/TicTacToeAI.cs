using System;
using System.Collections.Generic;
using System.Drawing;

namespace tictactoe
{
    public class TicTacToeAI
    {
        private readonly Random rand;

        public TicTacToeAI()
        {
            rand = new Random();
        }

        public Point? GetBestMove(GameBoard board, string aiSymbol, string humanSymbol)
        {
            List<Point> availableMoves = board.GetAvailableMoves();

            if (availableMoves.Count == 0)
                return null;

            // 1. Tentar vencer
            foreach (Point move in availableMoves)
            {
                GameBoard testBoard = board.Clone();
                testBoard.SetCell(move.X, move.Y, aiSymbol);

                if (testBoard.CheckWinner() == aiSymbol)
                    return move;
            }

            // 2. Bloquear o jogador
            foreach (Point move in availableMoves)
            {
                GameBoard testBoard = board.Clone();
                testBoard.SetCell(move.X, move.Y, humanSymbol);

                if (testBoard.CheckWinner() == humanSymbol)
                    return move;
            }

            // 3. Centro
            if (board.IsCellEmpty(1, 1))
                return new Point(1, 1);

            // 4. Cantos
            List<Point> corners = new List<Point>
            {
                new Point(0, 0),
                new Point(0, 2),
                new Point(2, 0),
                new Point(2, 2)
            };

            List<Point> availableCorners = new List<Point>();

            foreach (Point corner in corners)
            {
                if (board.IsCellEmpty(corner.X, corner.Y))
                    availableCorners.Add(corner);
            }

            if (availableCorners.Count > 0)
                return availableCorners[rand.Next(availableCorners.Count)];

            // 5. Qualquer posição livre
            return availableMoves[rand.Next(availableMoves.Count)];
        }
    }
}