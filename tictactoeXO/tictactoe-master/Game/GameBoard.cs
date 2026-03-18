namespace tictactoe;

public class GameBoard
{
    private readonly string[,] cells;

    public GameBoard()
    {
        cells = new string[3, 3];
        Reset();
    }

    public void Reset()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                cells[row, col] = string.Empty;
            }
        }
    }

    public string GetCell(int row, int col)
    {
        if (!IsValidPosition(row, col))
            return string.Empty;

        return cells[row, col];
    }

    public bool SetCell(int row, int col, string symbol)
    {
        if (!IsValidPosition(row, col))
            return false;

        if (!string.IsNullOrEmpty(cells[row, col]))
            return false;

        cells[row, col] = symbol;
        return true;
    }

    public bool IsCellEmpty(int row, int col)
    {
        if (!IsValidPosition(row, col))
            return false;

        return string.IsNullOrEmpty(cells[row, col]);
    }

    public bool IsFull()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (string.IsNullOrEmpty(cells[row, col]))
                    return false;
            }
        }

        return true;
    }

    public string CheckWinner()
    {
        for (int row = 0; row < 3; row++)
        {
            if (!string.IsNullOrEmpty(cells[row, 0]) &&
                cells[row, 0] == cells[row, 1] &&
                cells[row, 1] == cells[row, 2])
            {
                return cells[row, 0];
            }
        }

        for (int col = 0; col < 3; col++)
        {
            if (!string.IsNullOrEmpty(cells[0, col]) &&
                cells[0, col] == cells[1, col] &&
                cells[1, col] == cells[2, col])
            {
                return cells[0, col];
            }
        }

        if (!string.IsNullOrEmpty(cells[0, 0]) &&
            cells[0, 0] == cells[1, 1] &&
            cells[1, 1] == cells[2, 2])
        {
            return cells[0, 0];
        }

        if (!string.IsNullOrEmpty(cells[0, 2]) &&
            cells[0, 2] == cells[1, 1] &&
            cells[1, 1] == cells[2, 0])
        {
            return cells[0, 2];
        }

        return string.Empty;
    }

    public List<Point> GetAvailableMoves()
    {
        List<Point> moves = new List<Point>();

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (string.IsNullOrEmpty(cells[row, col]))
                {
                    // Point.X = row | Point.Y = col
                    moves.Add(new Point(row, col));
                }
            }
        }

        return moves;
    }

    public GameBoard Clone()
    {
        GameBoard clonedBoard = new GameBoard();

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                clonedBoard.cells[row, col] = cells[row, col];
            }
        }

        return clonedBoard;
    }

    private bool IsValidPosition(int row, int col)
    {
        return row >= 0 && row < 3 && col >= 0 && col < 3;
    }
}
