using Xunit;
using TicTacToe.Core;

namespace TicTacToe.Tests
{
    public class GameTests
    {
        [Fact]
        public void InitialGame_IsCorrectlyConfigured()
        {
            var game = new Game();
            Assert.Equal(CellState.X, game.CurrentPlayer);
            Assert.False(game.IsGameOver);
            Assert.Equal(CellState.Empty, game.Winner);
            Assert.Equal(GameMode.HumanVsHuman, game.Mode);
        }

        [Fact]
        public void ValidMove_SwitchesTurn()
        {
            var game = new Game(GameMode.HumanVsHuman);
            Assert.Equal(CellState.X, game.CurrentPlayer);

            bool result = game.MakeMove(0, 0);
            Assert.True(result);
            Assert.Equal(CellState.O, game.CurrentPlayer);
        }

        [Fact]
        public void InvalidMove_DoesNotSwitchTurn()
        {
            var game = new Game(GameMode.HumanVsHuman);
            game.MakeMove(0, 0); // O's turn now

            bool result = game.MakeMove(0, 0); // Invalid move (occupied)
            Assert.False(result);
            Assert.Equal(CellState.O, game.CurrentPlayer);
        }

        [Fact]
        public void WinCondition_EndsGameAndSetsWinner()
        {
            // X wins in top row
            var game = new Game(GameMode.HumanVsHuman);
            game.MakeMove(0, 0); // X
            game.MakeMove(1, 0); // O
            game.MakeMove(0, 1); // X
            game.MakeMove(1, 1); // O
            game.MakeMove(0, 2); // X - Wins!

            Assert.True(game.IsGameOver);
            Assert.Equal(CellState.X, game.Winner);
        }

        [Fact]
        public void DrawCondition_EndsGameWithNoWinner()
        {
            // Grid layout:
            // X O X
            // X O O
            // O X X
            var game = new Game(GameMode.HumanVsHuman);
            game.MakeMove(0, 0); // X (0,0)
            game.MakeMove(0, 1); // O (0,1)
            game.MakeMove(0, 2); // X (0,2)
            game.MakeMove(1, 1); // O (1,1)
            game.MakeMove(1, 0); // X (1,0)
            game.MakeMove(1, 2); // O (1,2)
            game.MakeMove(2, 1); // X (2,1)
            game.MakeMove(2, 0); // O (2,0)
            game.MakeMove(2, 2); // X (2,2) - Board full!

            Assert.True(game.IsGameOver);
            Assert.Equal(CellState.Empty, game.Winner);
        }

        [Fact]
        public void MovesAfterGameOver_AreRejected()
        {
            var game = new Game(GameMode.HumanVsHuman);
            game.MakeMove(0, 0); // X
            game.MakeMove(1, 0); // O
            game.MakeMove(0, 1); // X
            game.MakeMove(1, 1); // O
            game.MakeMove(0, 2); // X - Wins!

            Assert.True(game.IsGameOver);
            bool extraMove = game.MakeMove(2, 2);
            Assert.False(extraMove);
        }

        [Fact]
        public void HumanVsAi_WithEmptyAiPlayer_Throws()
        {
            Assert.Throws<ArgumentException>(() => new Game(GameMode.HumanVsAi, Difficulty.Easy, CellState.Empty));
        }

        [Fact]
        public void Reset_WithAiAsX_AiOpensAgain()
        {
            var game = new Game(GameMode.HumanVsAi, Difficulty.Easy, CellState.X, new Random(7));
            // Human (O) plays the centre, or the top-middle if the AI already took the centre
            var (row, col) = game.Board.GetCell(1, 1) == CellState.Empty ? (1, 1) : (0, 1);
            Assert.True(game.MakeMove(row, col));

            game.Reset();

            int xCount = 0;
            for (int r = 0; r < Board.Size; r++)
            {
                for (int c = 0; c < Board.Size; c++)
                {
                    if (game.Board.GetCell(r, c) == CellState.X)
                    {
                        xCount++;
                    }
                }
            }
            Assert.Equal(1, xCount);
            Assert.Equal(CellState.O, game.CurrentPlayer);
        }

        [Fact]
        public void Reset_ClearsStateAndSetsTurnToX()
        {
            var game = new Game(GameMode.HumanVsHuman);
            game.MakeMove(0, 0);
            game.MakeMove(1, 1);
            
            game.Reset();
            
            Assert.Equal(CellState.X, game.CurrentPlayer);
            Assert.False(game.IsGameOver);
            Assert.Equal(CellState.Empty, game.Winner);
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    Assert.Equal(CellState.Empty, game.Board.GetCell(r, c));
                }
            }
        }
    }
}
