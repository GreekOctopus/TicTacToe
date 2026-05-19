using System;

namespace TicTacToe.Core
{
    public class Board
    {
        public CellState[,] Grid { get; private set; }

        public Board()
        {
            Grid = new CellState[3, 3];
            Reset();
        }

        public void Reset()
        {
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    Grid[r, c] = CellState.Empty;
                }
            }
        }

        public bool SetCell(int row, int col, CellState state)
        {
            if (row < 0 || row >= 3 || col < 0 || col >= 3)
            {
                return false;
            }

            if (Grid[row, col] != CellState.Empty && state != CellState.Empty)
            {
                return false; // Cell already occupied
            }

            Grid[row, col] = state;
            return true;
        }

        public CellState GetCell(int row, int col)
        {
            if (row < 0 || row >= 3 || col < 0 || col >= 3)
            {
                throw new ArgumentOutOfRangeException("Row or column is out of range.");
            }

            return Grid[row, col];
        }

        public bool IsFull()
        {
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    if (Grid[r, c] == CellState.Empty)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool CheckWin(out CellState winner)
        {
            winner = CellState.Empty;

            // Check rows
            for (int r = 0; r < 3; r++)
            {
                if (Grid[r, 0] != CellState.Empty &&
                    Grid[r, 0] == Grid[r, 1] &&
                    Grid[r, 1] == Grid[r, 2])
                {
                    winner = Grid[r, 0];
                    return true;
                }
            }

            // Check columns
            for (int c = 0; c < 3; c++)
            {
                if (Grid[0, c] != CellState.Empty &&
                    Grid[0, c] == Grid[1, c] &&
                    Grid[1, c] == Grid[2, c])
                {
                    winner = Grid[0, c];
                    return true;
                }
            }

            // Check main diagonal
            if (Grid[0, 0] != CellState.Empty &&
                Grid[0, 0] == Grid[1, 1] &&
                Grid[1, 1] == Grid[2, 2])
            {
                winner = Grid[0, 0];
                return true;
            }

            // Check anti-diagonal
            if (Grid[0, 2] != CellState.Empty &&
                Grid[0, 2] == Grid[1, 1] &&
                Grid[1, 1] == Grid[2, 0])
            {
                winner = Grid[0, 2];
                return true;
            }

            return false;
        }

        public Board Clone()
        {
            var clone = new Board();
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    clone.Grid[r, c] = this.Grid[r, c];
                }
            }
            return clone;
        }
    }
}
