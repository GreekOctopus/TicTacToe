using System;
using System.Collections.Generic;

namespace TicTacToe.Core
{
    public static class AiOpponent
    {
        // Corners and centre: the openings that give an imperfect opponent the most ways to go wrong
        private static readonly (int Row, int Col)[] StrongOpenings = { (0, 0), (0, 2), (1, 1), (2, 0), (2, 2) };

        public static (int Row, int Col) GetBestMove(IReadOnlyBoard board, CellState aiPlayer, Difficulty difficulty, Random? random = null)
        {
            if (aiPlayer != CellState.X && aiPlayer != CellState.O)
            {
                throw new ArgumentException("AI player must be X or O.", nameof(aiPlayer));
            }

            random ??= Random.Shared;

            if (difficulty == Difficulty.Easy)
            {
                return GetRandomMove(board, random);
            }

            // Every opening draws with perfect play, so skip the full search on an empty board
            if (IsEmpty(board))
            {
                return StrongOpenings[random.Next(StrongOpenings.Length)];
            }

            var bestMoves = GetBestMoves(board, aiPlayer);
            return bestMoves.Count == 0 ? (-1, -1) : bestMoves[random.Next(bestMoves.Count)];
        }

        // All moves that tie for the best minimax value; tests use this to cover every AI choice.
        internal static List<(int Row, int Col)> GetBestMoves(IReadOnlyBoard board, CellState aiPlayer)
        {
            // Search on a copy so the caller's board is never mutated
            return GetMinimaxMoves(board.Clone(), aiPlayer);
        }

        private static bool IsEmpty(IReadOnlyBoard board)
        {
            for (int r = 0; r < Board.Size; r++)
            {
                for (int c = 0; c < Board.Size; c++)
                {
                    if (board.GetCell(r, c) != CellState.Empty)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static (int Row, int Col) GetRandomMove(IReadOnlyBoard board, Random random)
        {
            var emptyCells = new List<(int Row, int Col)>();

            for (int r = 0; r < Board.Size; r++)
            {
                for (int c = 0; c < Board.Size; c++)
                {
                    if (board.GetCell(r, c) == CellState.Empty)
                    {
                        emptyCells.Add((r, c));
                    }
                }
            }

            if (emptyCells.Count == 0)
            {
                return (-1, -1);
            }

            return emptyCells[random.Next(emptyCells.Count)];
        }

        private static List<(int Row, int Col)> GetMinimaxMoves(Board board, CellState aiPlayer)
        {
            CellState opponentPlayer = (aiPlayer == CellState.X) ? CellState.O : CellState.X;
            int bestVal = int.MinValue;
            var bestMoves = new List<(int Row, int Col)>();

            for (int r = 0; r < Board.Size; r++)
            {
                for (int c = 0; c < Board.Size; c++)
                {
                    if (board.GetCell(r, c) == CellState.Empty)
                    {
                        // Make the move
                        board.SetCell(r, c, aiPlayer);

                        // Compute evaluation function for this move
                        int moveVal = Minimax(board, 0, false, aiPlayer, opponentPlayer);

                        // Undo the move
                        board.ClearCell(r, c);

                        // Keep every move that ties for the best value, so play isn't predictable
                        if (moveVal > bestVal)
                        {
                            bestVal = moveVal;
                            bestMoves.Clear();
                        }
                        if (moveVal == bestVal)
                        {
                            bestMoves.Add((r, c));
                        }
                    }
                }
            }

            return bestMoves;
        }

        private static int Minimax(Board board, int depth, bool isMax, CellState aiPlayer, CellState opponentPlayer)
        {
            if (board.CheckWin(out CellState winner))
            {
                if (winner == aiPlayer)
                {
                    return 10 - depth;
                }
                if (winner == opponentPlayer)
                {
                    return depth - 10;
                }
            }

            if (board.IsFull())
            {
                return 0;
            }

            if (isMax)
            {
                int best = int.MinValue;
                for (int r = 0; r < Board.Size; r++)
                {
                    for (int c = 0; c < Board.Size; c++)
                    {
                        if (board.GetCell(r, c) == CellState.Empty)
                        {
                            board.SetCell(r, c, aiPlayer);
                            best = Math.Max(best, Minimax(board, depth + 1, false, aiPlayer, opponentPlayer));
                            board.ClearCell(r, c);
                        }
                    }
                }
                return best;
            }
            else
            {
                int best = int.MaxValue;
                for (int r = 0; r < Board.Size; r++)
                {
                    for (int c = 0; c < Board.Size; c++)
                    {
                        if (board.GetCell(r, c) == CellState.Empty)
                        {
                            board.SetCell(r, c, opponentPlayer);
                            best = Math.Min(best, Minimax(board, depth + 1, true, aiPlayer, opponentPlayer));
                            board.ClearCell(r, c);
                        }
                    }
                }
                return best;
            }
        }
    }
}
