namespace TicTacToe.Core
{
    // Wraps a Board so callers can read it but can't cast back to Board and change it.
    internal sealed class ReadOnlyBoardView : IReadOnlyBoard
    {
        private readonly Board _board;

        public ReadOnlyBoardView(Board board)
        {
            _board = board;
        }

        public CellState GetCell(int row, int col) => _board.GetCell(row, col);

        public bool IsFull() => _board.IsFull();

        public bool CheckWin(out CellState winner) => _board.CheckWin(out winner);

        public Board Clone() => _board.Clone();
    }
}
