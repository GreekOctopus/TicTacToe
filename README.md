# Tic Tac Toe C# Engine

A clean, modular C# Tic Tac Toe game engine featuring an interactive console client, an unbeatable Minimax AI opponent, and a comprehensive xUnit test suite. The core engine is designed with SOLID principles and is fully decoupled from the UI, making it ready to be dropped into a game framework like Unity.

## Project Structure

The solution contains three projects:

1. **`TicTacToe.Core` (Class Library / DLL)**:
   - Houses the core game logic, board state representation, and win/draw checkers.
   - Contains the `AiOpponent` class supporting **Easy** (random selection) and **Hard** (Minimax search tree) difficulties.
2. **`TicTacToe.Console` (Console Application)**:
   - An interactive command-line interface to play the game.
   - Displays a cleanly rendered board with validation prompts and color indicators.
3. **`TicTacToe.Tests` (xUnit Test Suite)**:
   - 23 unit tests verifying board mechanics, game transitions, and AI decision-making.

---

## Features

- **Multiple Game Modes**: Play Human vs. Human or Human vs. AI.
- **Minimax AI Opponent**: The Hard mode AI evaluates all possible future game states to make mathematically optimal, unbeatable moves.
- **Defensive Design**: Extensive bounds checking and coordinate validation.
- **Unity Compatibility**: Standard C# compilation makes it easy to integrate with Unity scripts and GameObjects.

---

## Getting Started

### Prerequisites
- [.NET SDK 10.0](https://dotnet.microsoft.com/download) (or newer)

### Run Unit Tests
To run all 23 automated tests:
```bash
dotnet test
```

### Play the Console Game
To start the interactive command-line game:
```bash
dotnet run --project Console
```

### Integrate with Unity
To use this engine in a Unity project:
1. Copy all `.cs` files inside `Core/` into your Unity `Assets/Scripts/` folder.
2. Reference the `TicTacToe.Core` namespace in your MonoBehaviour scripts to drive visual game events (like spawning 3D assets or detecting clicks).
