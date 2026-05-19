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
        public void HumanVsAiGame_AiPlayerX_CanGoFirst()
        {
            // AI is X (goes first), Human is O
            var game = new Game(GameMode.HumanVsAi, Difficulty.Hard, CellState.X);
            
            // Trigger AI's first move
            bool aiMoved = game.TriggerAiMove();
            Assert.True(aiMoved);
            
            // CurrentPlayer should now be O (Human)
            Assert.Equal(CellState.O, game.CurrentPlayer);
            
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
    }
}
