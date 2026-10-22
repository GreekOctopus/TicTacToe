using System;
using Xunit;
using TicTacToe.Core;

namespace TicTacToe.Tests
{
    public class BoardTests
    {
        [Fact]
        public void InitialBoard_IsEmpty()
        {
            var board = new Board();
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    Assert.Equal(CellState.Empty, board.GetCell(r, c));
                }
            }
            Assert.False(board.IsFull());
        }

        [Fact]
        public void SetCell_SetsStateCorrectly()
        {
            var board = new Board();
            bool result = board.SetCell(0, 0, CellState.X);
            Assert.True(result);
            Assert.Equal(CellState.X, board.GetCell(0, 0));
        }

        [Fact]
        public void SetCell_OnOccupiedCell_ReturnsFalse()
        {
            var board = new Board();
            board.SetCell(1, 1, CellState.X);
            bool result = board.SetCell(1, 1, CellState.O);
            Assert.False(result);
            Assert.Equal(CellState.X, board.GetCell(1, 1));
        }

        [Fact]
        public void SetCell_OutOfBounds_ReturnsFalse()
        {
            var board = new Board();
            Assert.False(board.SetCell(-1, 0, CellState.X));
            Assert.False(board.SetCell(3, 0, CellState.X));
            Assert.False(board.SetCell(0, -1, CellState.X));
            Assert.False(board.SetCell(0, 3, CellState.X));
        }

        [Fact]
        public void GetCell_OutOfBounds_ThrowsException()
        {
            var board = new Board();
            Assert.Throws<ArgumentOutOfRangeException>(() => board.GetCell(-1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => board.GetCell(0, 3));
        }

        [Fact]
        public void SetCell_WithEmpty_DoesNotClearCell()
        {
            var board = new Board();
            board.SetCell(1, 1, CellState.X);

            Assert.False(board.SetCell(1, 1, CellState.Empty));
            Assert.Equal(CellState.X, board.GetCell(1, 1));
        }

        [Fact]
        public void GetCell_OutOfBounds_ReportsOffendingParameter()
        {
            var board = new Board();
            var rowEx = Assert.Throws<ArgumentOutOfRangeException>(() => board.GetCell(3, 0));
            var colEx = Assert.Throws<ArgumentOutOfRangeException>(() => board.GetCell(0, -1));

            Assert.Equal("row", rowEx.ParamName);
            Assert.Equal("col", colEx.ParamName);
        }

        [Fact]
        public void Clone_CopiesStateAndIsIndependent()
        {
            var board = new Board();
            board.SetCell(0, 0, CellState.X);

            var clone = board.Clone();
            clone.SetCell(2, 2, CellState.O);

            Assert.Equal(CellState.X, clone.GetCell(0, 0));
            Assert.Equal(CellState.Empty, board.GetCell(2, 2));
        }

        [Fact]
        public void IsFull_ReturnsTrue_OnlyWhenFull()
        {
            var board = new Board();
            Assert.False(board.IsFull());

            // Fill all cells except one
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    if (r == 2 && c == 2) continue;
                    board.SetCell(r, c, CellState.X);
                }
            }
            Assert.False(board.IsFull());

            // Fill the last cell
            board.SetCell(2, 2, CellState.O);
            Assert.True(board.IsFull());
        }

        [Fact]
        public void CheckWin_DetectsRowWin()
        {
            var board = new Board();
            board.SetCell(0, 0, CellState.X);
            board.SetCell(0, 1, CellState.X);
            board.SetCell(0, 2, CellState.X);

            Assert.True(board.CheckWin(out CellState winner));
            Assert.Equal(CellState.X, winner);
        }

        [Fact]
        public void CheckWin_DetectsColumnWin()
        {
            var board = new Board();
            board.SetCell(0, 1, CellState.O);
            board.SetCell(1, 1, CellState.O);
            board.SetCell(2, 1, CellState.O);

            Assert.True(board.CheckWin(out CellState winner));
            Assert.Equal(CellState.O, winner);
        }

        [Fact]
        public void CheckWin_DetectsDiagonalWin()
        {
            var board = new Board();
            board.SetCell(0, 0, CellState.X);
            board.SetCell(1, 1, CellState.X);
            board.SetCell(2, 2, CellState.X);

            Assert.True(board.CheckWin(out CellState winner));
            Assert.Equal(CellState.X, winner);
        }

        [Fact]
        public void CheckWin_DetectsAntiDiagonalWin()
        {
            var board = new Board();
            board.SetCell(0, 2, CellState.O);
            board.SetCell(1, 1, CellState.O);
            board.SetCell(2, 0, CellState.O);

            Assert.True(board.CheckWin(out CellState winner));
            Assert.Equal(CellState.O, winner);
        }

        [Fact]
        public void CheckWin_NoWinner_ReturnsFalse()
        {
            var board = new Board();
            board.SetCell(0, 0, CellState.X);
            board.SetCell(0, 1, CellState.O);
            board.SetCell(0, 2, CellState.X);

            Assert.False(board.CheckWin(out CellState winner));
            Assert.Equal(CellState.Empty, winner);
        }
    }
}
