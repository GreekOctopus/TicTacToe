namespace TicTacToe.Core
{
    public static class AiOpponent
    {
        public static (int Row, int Col) GetBestMove(Board board, CellState aiPlayer, Difficulty difficulty, Random? random = null)
        {
            if (aiPlayer == CellState.Empty)
            {
                throw new ArgumentException("AI player state cannot be Empty.", nameof(aiPlayer));
            }

            if (difficulty == Difficulty.Easy)
            {
                return GetRandomMove(board, random ?? new Random());
            }
            else
            {
                // Search on a copy so the caller's board is never mutated
                return GetMinimaxMove(board.Clone(), aiPlayer);
            }
        }

        private static (int Row, int Col) GetRandomMove(Board board, Random random)
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

        private static (int Row, int Col) GetMinimaxMove(Board board, CellState aiPlayer)
        {
            CellState opponentPlayer = (aiPlayer == CellState.X) ? CellState.O : CellState.X;
            int bestVal = int.MinValue;
            (int Row, int Col) bestMove = (-1, -1);

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

                        // If the value of the current move is more than the best value, update best move
                        if (moveVal > bestVal)
                        {
                            bestVal = moveVal;
                            bestMove = (r, c);
                        }
                    }
                }
            }

            return bestMove;
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
