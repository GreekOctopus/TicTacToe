namespace TicTacToe.Core
{
    // Read-only view handed to UI code so moves can only be made through Game.
    public interface IReadOnlyBoard
    {
        CellState GetCell(int row, int col);
        bool IsFull();
        bool CheckWin(out CellState winner);
        Board Clone();
    }
}
