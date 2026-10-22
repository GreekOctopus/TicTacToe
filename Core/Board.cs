using System;

namespace TicTacToe.Core
{
    public class Board : IReadOnlyBoard
    {
        // Fixed at 3: CheckWin and the console grid are written for a 3x3 board.
        public const int Size = 3;

        private readonly CellState[,] _grid = new CellState[Size, Size];

        public Board()
        {
            Reset();
        }

        public void Reset()
        {
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    _grid[r, c] = CellState.Empty;
                }
            }
        }

        public bool SetCell(int row, int col, CellState state)
        {
            if (!IsInBounds(row, col))
            {
                return false;
            }

            if (state == CellState.Empty)
            {
                return false; // Use ClearCell to undo a move
            }

            if (_grid[row, col] != CellState.Empty)
            {
                return false; // Cell already occupied
            }

            _grid[row, col] = state;
            return true;
        }

        // Only the engine (e.g. AI search on a cloned board) may undo moves.
        internal void ClearCell(int row, int col)
        {
            _grid[row, col] = CellState.Empty;
        }

        public CellState GetCell(int row, int col)
        {
            if (row < 0 || row >= Size)
            {
                throw new ArgumentOutOfRangeException(nameof(row), row, $"Row must be between 0 and {Size - 1}.");
            }

            if (col < 0 || col >= Size)
            {
                throw new ArgumentOutOfRangeException(nameof(col), col, $"Column must be between 0 and {Size - 1}.");
            }

            return _grid[row, col];
        }

        public bool IsFull()
        {
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    if (_grid[r, c] == CellState.Empty)
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
            for (int r = 0; r < Size; r++)
            {
                if (_grid[r, 0] != CellState.Empty &&
                    _grid[r, 0] == _grid[r, 1] &&
                    _grid[r, 1] == _grid[r, 2])
                {
                    winner = _grid[r, 0];
                    return true;
                }
            }

            // Check columns
            for (int c = 0; c < Size; c++)
            {
                if (_grid[0, c] != CellState.Empty &&
                    _grid[0, c] == _grid[1, c] &&
                    _grid[1, c] == _grid[2, c])
                {
                    winner = _grid[0, c];
                    return true;
                }
            }

            // Check main diagonal
            if (_grid[0, 0] != CellState.Empty &&
                _grid[0, 0] == _grid[1, 1] &&
                _grid[1, 1] == _grid[2, 2])
            {
                winner = _grid[0, 0];
                return true;
            }

            // Check anti-diagonal
            if (_grid[0, 2] != CellState.Empty &&
                _grid[0, 2] == _grid[1, 1] &&
                _grid[1, 1] == _grid[2, 0])
            {
                winner = _grid[0, 2];
                return true;
            }

            return false;
        }

        public Board Clone()
        {
            var clone = new Board();
            Array.Copy(_grid, clone._grid, _grid.Length);
            return clone;
        }

        private static bool IsInBounds(int row, int col)
        {
            return row >= 0 && row < Size && col >= 0 && col < Size;
        }
    }
}
