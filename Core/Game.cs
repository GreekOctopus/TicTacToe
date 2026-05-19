using System;

namespace TicTacToe.Core
{
    public class Game
    {
        public Board Board { get; private set; }
        public CellState CurrentPlayer { get; private set; }
        public GameMode Mode { get; private set; }
        public Difficulty AiDifficulty { get; private set; }
        public CellState AiPlayer { get; private set; }
        public bool IsGameOver { get; private set; }
        public CellState Winner { get; private set; }

        public Game(GameMode mode = GameMode.HumanVsHuman, Difficulty aiDifficulty = Difficulty.Easy, CellState aiPlayer = CellState.O)
        {
            Board = new Board();
            Mode = mode;
            AiDifficulty = aiDifficulty;
            AiPlayer = aiPlayer;
            Reset();
        }

        public void Reset()
        {
            Board.Reset();
            CurrentPlayer = CellState.X;
            IsGameOver = false;
            Winner = CellState.Empty;
        }

        public bool MakeMove(int row, int col)
        {
            if (IsGameOver)
            {
                return false;
            }

            // Prevent human from moving if it's currently the AI's turn
            if (Mode == GameMode.HumanVsAi && CurrentPlayer == AiPlayer)
            {
                return false;
            }

            if (!ApplyMove(row, col))
            {
                return false;
            }

            // If game is not over and it's the AI's turn, run AI move immediately
            if (!IsGameOver && Mode == GameMode.HumanVsAi && CurrentPlayer == AiPlayer)
            {
                TriggerAiMove();
            }

            return true;
        }

        public bool TriggerAiMove()
        {
            if (IsGameOver || Mode != GameMode.HumanVsAi || CurrentPlayer != AiPlayer)
            {
                return false;
            }

            var (row, col) = AiOpponent.GetBestMove(Board, AiPlayer, AiDifficulty);
            if (row != -1 && col != -1)
            {
                return ApplyMove(row, col);
            }

            return false;
        }

        private bool ApplyMove(int row, int col)
        {
            if (!Board.SetCell(row, col, CurrentPlayer))
            {
                return false;
            }

            if (Board.CheckWin(out CellState winner))
            {
                Winner = winner;
                IsGameOver = true;
            }
            else if (Board.IsFull())
            {
                Winner = CellState.Empty;
                IsGameOver = true;
            }
            else
            {
                CurrentPlayer = (CurrentPlayer == CellState.X) ? CellState.O : CellState.X;
            }

            return true;
        }
    }
}
