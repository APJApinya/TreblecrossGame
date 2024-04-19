using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IFN563_Assessment2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string FILE = "savefile.json";
            Game game = null;
            Board board = null;
            IPlayer p1 = null;
            IPlayer p2 = null;

            bool isGameover = false;
            bool isPlayer1Turn = true;

            //Game starter
            StartSupportSystem startSystem = new StartSupportSystem();
            int userStartChoice = startSystem.StartGame();
            if (userStartChoice == 1)
            {
                (Game gameObject, Board gameBoard, IPlayer player1, IPlayer player2) = startSystem.ConfigureGame();
                board = gameBoard;
                p1 = player1;
                p2 = player2;
                game = gameObject;
            }
            else if (userStartChoice == 2)
            {
                LoadSupportSystem loadedGame = new LoadSupportSystem();
                (Game GameObject, Board gameBoard, IPlayer player1, IPlayer player2, bool player1Turn) = loadedGame.LoadGame(FILE);
                board = gameBoard;
                p1 = player1;
                p2 = player2;
                isPlayer1Turn = player1Turn;
                game = GameObject;
            }

            while (!isGameover)
            {
                IPlayer currentPlayer = isPlayer1Turn ? p1 : p2;
                PlayTurn(currentPlayer, ref board, ref game, ref isPlayer1Turn);
                (isGameover, bool isSomeoneWin) = game.CheckWin(board);
                if (isGameover == true)
                {
                    game.EndGame(currentPlayer, isGameover, isSomeoneWin);
                }
                Console.WriteLine();
            }
        }
        static void PlayTurn(IPlayer currentPlayer, ref Board board, ref Game game, ref bool isPlayer1Turn)
        {
            bool isUserHappy = false;
            Console.WriteLine($"<<<<<<<Player {currentPlayer.Name} turn!>>>>>>");
            Console.WriteLine("This is your current boardgame looks like: ");
            board.DisplayBoard();

            while (!isUserHappy)
            {
                bool isMoveValid = false;
                while (!isMoveValid)
                {
                    isMoveValid = game.GetMove(currentPlayer, board);
                }
                if (currentPlayer is HumanPlayer)
                {
                    Console.Write("Change your mind? (Y for undo, N for confirm your move): ");
                    string userChoice = Console.ReadLine().ToLower();
                    isUserHappy = userChoice == "n" || game.UndoMove(); //if n --> true, y --> call undo()
                }
                else
                {
                    isUserHappy = true;
                }
            }

            isPlayer1Turn = !isPlayer1Turn;
            if (currentPlayer is HumanPlayer)
            {
                game.ConfirmEndTurn(currentPlayer, board, isPlayer1Turn);
            }

        }


    }
}
