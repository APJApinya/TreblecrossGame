# Overview
Treblecross is a one-dimensional board game where two players compete to form a line of three consecutive "X"s. This project is implemented in C# and offers a clean, modular design that supports gameplay, saving and loading game states, and customizable board sizes.

The game includes features for both single-player (against a computer opponent) and multiplayer modes. Additionally, it incorporates support systems for rules, user guidance, and game state management, making it a complete and fun gaming experience.

This project demonstrates:
- Clean and modular code design.
- Effective use of object-oriented principles like inheritance and interfaces.
- Practical experience with game development and state management.
- Hands-on implementation of serialization and file handling in C#.

# Key Features
- **Game Modes**:
  - Single-player mode: Play against a computer opponent.
  - Multiplayer mode: Two players compete on the same device.
- **Dynamic Board**: Customize the board size for different levels of challenge.
- **Undo Feature**: Allows players to undo their last move and rethink their strategy.
- **Save and Load Game**: Save the current game state to a file and reload it later to continue.
- **Built-In Game Rules**: A help system displays the rules and instructions when needed.
- **Extensible Design**: Includes an abstract class structure to allow future games like Reversi to be added in the future developement.

# Architecture
This project is designed with modularity and extensibility in mind:
- **Board Management**: The TreblecrossBoard class dynamically generates a 1D board and manages game piece placements.
- **Game Logic**: The TreblecrossGame class encapsulates the rules for move validation and win conditions.
- **Player Management**: Two player types: HumanPlayer (interactive input) and ComputerPlayer (random move generation).
- **Support Systems**:
  - StartSupportSystem: Guides players through game setup.
  - SaveSupportSystem: Saves the game state as JSON.
  - LoadSupportSystem: Restores game state from JSON.
  - HelpSupportSystem: Displays rules and instructions.
- **Serialization**: Uses JSON.NET (Newtonsoft.Json) for saving and loading game data.

# Tech Stack
**Language**: C#
**Framework**: .NET Core

# How It Works
- Game Setup:
  - Players select the mode (single-player or multiplayer) and the board size.
  - If single-player is chosen, the computer opponent is initialized.
- Gameplay:
  - Players take turns placing "X"s on the board.
  - The game checks for valid moves and determines the win condition after every turn.
- Winning Condition: The first player to create a line of three consecutive "X"s wins.
- Additional Features: Players can undo moves or save the game at any point.

# Project Structure
- Board.cs:
  - Abstract base class for board functionality.
  - Implements TreblecrossBoard for the game.
- Game.cs:
  - Abstract base class for game logic.
  - Implements TreblecrossGame for Treblecross-specific rules.
- Player.cs:
  - Defines the IPlayer interface and its implementations (HumanPlayer, ComputerPlayer).
- SupportSystem.cs:
  - Contains helper classes for starting, saving, loading, and displaying game rules.
- Program.cs:
  - Entry point of the application. Manages game flow and player turns.
 
# Future Enhancements
- Add support for additional games like Reversi.
- Implement a graphical user interface (GUI) for a more engaging user experience.
- Introduce AI algorithms for the computer opponent to provide more strategic gameplay.
