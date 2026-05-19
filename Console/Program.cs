using System;
using TicTacToe.Core;

namespace TicTacToe.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            bool keepPlayingApp = true;

            while (keepPlayingApp)
            {
                SafeClear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=====================================");
                Console.WriteLine("     WELCOME TO TIC TAC TOE C#      ");
                Console.WriteLine("=====================================");
                Console.ResetColor();
                Console.WriteLine();

                GameMode mode = ChooseGameMode();
                Difficulty difficulty = Difficulty.Easy;
                CellState aiPlayer = CellState.O;

                if (mode == GameMode.HumanVsAi)
                {
                    difficulty = ChooseDifficulty();
                    aiPlayer = ChooseAiSymbol();
                }

                Game game = new Game(mode, difficulty, aiPlayer);
                
                // If AI goes first (X), trigger the first move
                if (mode == GameMode.HumanVsAi && aiPlayer == CellState.X)
                {
                    game.TriggerAiMove();
                }

                bool inGameLoop = true;
                string statusMessage = "";

                while (inGameLoop)
                {
                    DrawBoard(game.Board);
                    
                    if (!string.IsNullOrEmpty(statusMessage))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"[Info] {statusMessage}");
                        Console.ResetColor();
                        statusMessage = "";
                    }

                    if (game.IsGameOver)
                    {
                        Console.WriteLine();
                        if (game.Winner == CellState.Empty)
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("Game Over! It's a DRAW!");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Game Over! Winner is: {game.Winner}");
                        }
                        Console.ResetColor();
                        inGameLoop = false;
                        break;
                    }

                    Console.WriteLine();
                    Console.WriteLine($"Current Turn: {game.CurrentPlayer}");
                    Console.Write("Enter your move (row and col, e.g. '1 1' or '2 3'): ");
                    string? input = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(input))
                    {
                        statusMessage = "Input cannot be empty. Please enter row and column (e.g. '1 3').";
                        continue;
                    }

                    string[] parts = input.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 2 || 
                        !int.TryParse(parts[0], out int rowInput) || 
                        !int.TryParse(parts[1], out int colInput))
                    {
                        statusMessage = "Invalid input format. Use space or comma separated numbers (e.g. '1 2').";
                        continue;
                    }

                    int row = rowInput - 1;
                    int col = colInput - 1;

                    if (row < 0 || row >= 3 || col < 0 || col >= 3)
                    {
                        statusMessage = "Coordinates must be between 1 and 3.";
                        continue;
                    }

                    bool moveSuccess = game.MakeMove(row, col);
                    if (!moveSuccess)
                    {
                        statusMessage = "Cell is already occupied or move is invalid!";
                    }
                }

                Console.WriteLine();
                Console.Write("Would you like to play again? (y/n): ");
                string? playAgainInput = Console.ReadLine()?.Trim().ToLower();
                if (playAgainInput != "y" && playAgainInput != "yes")
                {
                    keepPlayingApp = false;
                }
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nThanks for playing Tic Tac Toe! Goodbye.");
            Console.ResetColor();
        }

        private static GameMode ChooseGameMode()
        {
            while (true)
            {
                Console.WriteLine("Select Game Mode:");
                Console.WriteLine("1. Human vs Human");
                Console.WriteLine("2. Human vs AI");
                Console.Write("Choice (1-2): ");
                string? choice = Console.ReadLine()?.Trim();
                if (choice == "1") return GameMode.HumanVsHuman;
                if (choice == "2") return GameMode.HumanVsAi;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid choice. Please select 1 or 2.");
                Console.ResetColor();
                Console.WriteLine();
            }
        }

        private static Difficulty ChooseDifficulty()
        {
            while (true)
            {
                Console.WriteLine("\nSelect AI Difficulty:");
                Console.WriteLine("1. Easy (AI plays random moves)");
                Console.WriteLine("2. Hard (AI uses Minimax - unbeatable)");
                Console.Write("Choice (1-2): ");
                string? choice = Console.ReadLine()?.Trim();
                if (choice == "1") return Difficulty.Easy;
                if (choice == "2") return Difficulty.Hard;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid choice. Please select 1 or 2.");
                Console.ResetColor();
                Console.WriteLine();
            }
        }

        private static CellState ChooseAiSymbol()
        {
            while (true)
            {
                Console.WriteLine("\nSelect AI Symbol:");
                Console.WriteLine("1. AI is O (Player is X, goes first)");
                Console.WriteLine("2. AI is X (Player is O, AI goes first)");
                Console.Write("Choice (1-2): ");
                string? choice = Console.ReadLine()?.Trim();
                if (choice == "1") return CellState.O;
                if (choice == "2") return CellState.X;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid choice. Please select 1 or 2.");
                Console.ResetColor();
                Console.WriteLine();
            }
        }

        private static void DrawBoard(Board board)
        {
            SafeClear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== TIC TAC TOE ===");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("     1   2   3");
            Console.WriteLine("   ┌───┬───┬───┐");

            for (int r = 0; r < 3; r++)
            {
                Console.Write($" {r + 1} │");
                for (int c = 0; c < 3; c++)
                {
                    CellState state = board.GetCell(r, c);
                    if (state == CellState.X)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" X ");
                    }
                    else if (state == CellState.O)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" O ");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" . ");
                    }
                    Console.ResetColor();
                    Console.Write("│");
                }
                Console.WriteLine();
                if (r < 2)
                {
                    Console.WriteLine("   ├───┼───┼───┤");
                }
            }
            Console.WriteLine("   └───┴───┴───┘");
            Console.WriteLine();
        }

        private static void SafeClear()
        {
            try
            {
                Console.Clear();
            }
            catch (System.IO.IOException)
            {
                // Safe fall-through when running in a redirected terminal environment
            }
        }
    }
}
