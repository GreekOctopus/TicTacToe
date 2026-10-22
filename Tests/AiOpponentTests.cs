using Xunit;
using TicTacToe.Core;

namespace TicTacToe.Tests
{
    public class AiOpponentTests
    {
        [Fact]
        public void EasyDifficulty_ReturnsValidEmptyCell()
        {
            var board = new Board();
            // Fill 8 cells
            board.SetCell(0, 0, CellState.X);
            board.SetCell(0, 1, CellState.O);
            board.SetCell(0, 2, CellState.X);
            board.SetCell(1, 0, CellState.O);
            board.SetCell(1, 1, CellState.X);
            board.SetCell(1, 2, CellState.O);
            board.SetCell(2, 0, CellState.X);
            board.SetCell(2, 1, CellState.O);

            // Last remaining cell is (2,2)
            var move = AiOpponent.GetBestMove(board, CellState.X, Difficulty.Easy);
            Assert.Equal(2, move.Row);
            Assert.Equal(2, move.Col);
        }

        [Fact]
        public void HardDifficulty_TakesWinningMove()
        {
            var board = new Board();
            // AI is 'O'. AI can win on Row 0.
            board.SetCell(0, 0, CellState.O);
            board.SetCell(0, 1, CellState.O);
            
            // X is elsewhere
            board.SetCell(1, 0, CellState.X);
            board.SetCell(2, 2, CellState.X);

            var move = AiOpponent.GetBestMove(board, CellState.O, Difficulty.Hard);
            Assert.Equal(0, move.Row);
            Assert.Equal(2, move.Col);
        }

        [Fact]
        public void HardDifficulty_BlocksOpponentWinningMove()
        {
            var board = new Board();
            // AI is 'O'. Opponent 'X' is about to win on Col 1.
            board.SetCell(0, 1, CellState.X);
            board.SetCell(1, 1, CellState.X);

            // AI must block at (2, 1)
            var move = AiOpponent.GetBestMove(board, CellState.O, Difficulty.Hard);
            Assert.Equal(2, move.Row);
            Assert.Equal(1, move.Col);
        }

        [Fact]
        public void HumanVsAiGame_AiPlayerO_AutomaticallyResponds()
        {
            // Human is X, AI is O
            var game = new Game(GameMode.HumanVsAi, Difficulty.Hard, CellState.O);
            
            // Human moves at (0,0)
            bool result = game.MakeMove(0, 0);
            Assert.True(result);
            
            // Turn should be back to Human (X) because AI (O) responded automatically
            Assert.Equal(CellState.X, game.CurrentPlayer);
            
            // Verify O has placed a move somewhere
            int oCount = 0;
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    if (game.Board.GetCell(r, c) == CellState.O)
                    {
                        oCount++;
                    }
                }
            }
            Assert.Equal(1, oCount);
        }

        [Fact]
        public void HumanVsAiGame_AiPlayerX_OpensAutomatically()
        {
            // AI is X (goes first), Human is O
            var game = new Game(GameMode.HumanVsAi, Difficulty.Hard, CellState.X);

            // AI has already moved, so it is Human's (O) turn and AI cannot move again
            Assert.Equal(CellState.O, game.CurrentPlayer);
            Assert.False(game.TriggerAiMove());
            
            // Make sure X has exactly 1 move
            int xCount = 0;
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    if (game.Board.GetCell(r, c) == CellState.X)
                    {
                        xCount++;
                    }
                }
            }
            Assert.Equal(1, xCount);
        }

        [Fact]
        public void HardDifficulty_DoesNotMutateBoard()
        {
            var board = new Board();
            board.SetCell(1, 1, CellState.X);

            AiOpponent.GetBestMove(board, CellState.O, Difficulty.Hard);

            for (int r = 0; r < Board.Size; r++)
            {
                for (int c = 0; c < Board.Size; c++)
                {
                    var expected = (r == 1 && c == 1) ? CellState.X : CellState.Empty;
                    Assert.Equal(expected, board.GetCell(r, c));
                }
            }
        }

        [Fact]
        public void EasyDifficulty_SameSeed_PicksSameMove()
        {
            var board = new Board();

            var first = AiOpponent.GetBestMove(board, CellState.O, Difficulty.Easy, new Random(42));
            var second = AiOpponent.GetBestMove(board, CellState.O, Difficulty.Easy, new Random(42));

            Assert.Equal(first, second);
        }

        [Theory]
        [InlineData(CellState.O)]
        [InlineData(CellState.X)]
        public void HardDifficulty_NeverLoses(CellState aiPlayer)
        {
            CellState humanPlayer = (aiPlayer == CellState.X) ? CellState.O : CellState.X;
            var board = new Board();

            // X always moves first
            if (aiPlayer == CellState.X)
            {
                PlayAiTurn(board, aiPlayer, humanPlayer);
            }
            else
            {
                PlayEveryHumanReply(board, aiPlayer, humanPlayer);
            }
        }

        // Tries every legal human move from this position, letting the AI answer each one.
        private static void PlayEveryHumanReply(Board board, CellState aiPlayer, CellState humanPlayer)
        {
            for (int r = 0; r < Board.Size; r++)
            {
                for (int c = 0; c < Board.Size; c++)
                {
                    if (board.GetCell(r, c) != CellState.Empty)
                    {
                        continue;
                    }

                    var next = board.Clone();
                    next.SetCell(r, c, humanPlayer);

                    Assert.False(next.CheckWin(out _), "Human found a winning line against the Hard AI.");
                    if (!next.IsFull())
                    {
                        PlayAiTurn(next, aiPlayer, humanPlayer);
                    }
                }
            }
        }

        private static void PlayAiTurn(Board board, CellState aiPlayer, CellState humanPlayer)
        {
            var move = AiOpponent.GetBestMove(board, aiPlayer, Difficulty.Hard);
            Assert.True(board.SetCell(move.Row, move.Col, aiPlayer));

            if (!board.CheckWin(out _) && !board.IsFull())
            {
                PlayEveryHumanReply(board, aiPlayer, humanPlayer);
            }
        }
    }
}
