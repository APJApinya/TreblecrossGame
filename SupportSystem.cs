using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IFN563_Assessment2
{
    public class StartSupportSystem
    {
        //Method when user first open the program
        public int StartGame()
        {
            Console.WriteLine("Please select:");
            Console.WriteLine("1. New Game");
            Console.WriteLine("2. Load Game");
            int userChoice = GetIntegerInput("Enter your choice (1 or 2): ", 1, 2);
            return userChoice;
        }

        //Method to create new game
        public (Game, Board, IPlayer, IPlayer) ConfigureGame()
        {
            Console.WriteLine("<<<<<<Welcome to the new game>>>>>>");
            Console.WriteLine("-----------------------------------");
            //1)ask user to select mode: 2 Players or 1 Player
            Console.WriteLine("Please select the mode of play");
            Console.WriteLine("1. Single Player (Player VS Computer)");
            Console.WriteLine("2. Two Players   (Player VS Player)");
            int modeChoice = GetIntegerInput("Enter your choice (1 or 2): ", 1, 2);

            //2)Ask user to select game
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("Please select game you want to play");
            Console.WriteLine("1. Treblecross");
            Console.WriteLine("2. Reversi (coming soon)");
            int gameOfChoice = GetIntegerInput("Enter your choice (1 or 2): ", 1, 2);

            //until reversi open for play, this line of code need to implement
            if (gameOfChoice == 2)
            {
                Console.WriteLine("Reversi is not available to play for now");
                Console.WriteLine("We will automatically select Treblecross for you");
                gameOfChoice = 1;
            }

            //ask user that do they need help? --> call HelpSupportSystem if needed
            Console.WriteLine("-----------------------------------");
            Console.Write("Do you know game rule? (Y if you're ok, N to show gamerule): ");
            string helpCall = Console.ReadLine().ToLower();
            if (helpCall == "n" || helpCall == "no")
            {
                HelpSupportSystem help = new HelpSupportSystem();
                help.ShowRule(gameOfChoice);
            }
            else if (helpCall == null)
            { 
            }


            //create board object from Board class
            Board board = null;
            //if user pick Treblecross --> ask user to input size of Board
            if (gameOfChoice == 1)
            {
                Console.WriteLine("-----------------------------------");
                int boardSize = GetIntegerInput("Please select the size of board (min is 5): ", 5, 999);
                //create board from Board
                board = new TreblecrossBoard(boardSize);
            }
            //if user pick Reversi --> system should set the size of Board
            //this is not open for user to pick yet, need to revise later
            else if (gameOfChoice == 2)
            {
                board = new ReversiBoard();
            }

            //create player from IPlayer
            IPlayer p1 = null, p2 = null;

            if (modeChoice == 1)
            {
                //single player : human vs computer
                Console.WriteLine("-----------------------------------");
                Console.Write("Please input name for Player1: ");
                string p1Name = Console.ReadLine();
                p1 = new HumanPlayer(p1Name);
                p2 = new ComputerPlayer();
            }
            else if (modeChoice == 2)
            {
                //two players: human vs human
                Console.WriteLine("-----------------------------------");
                Console.Write("Please input name for Player1: ");
                string p1Name = Console.ReadLine();
                p1 = new HumanPlayer(p1Name);
                Console.WriteLine("-----------------------------------");
                Console.Write("Please input name for Player2: ");
                string p2Name = Console.ReadLine();
                p2 = new HumanPlayer(p2Name);
            }

            //create game
            Game game = null;
            if (gameOfChoice == 1)
            {
                game = new TreblecrossGame(board, p1, p2, true);
            } else if (gameOfChoice == 2)
            {
                game = new ReversiGame(board, p1, p2, true);
            }
            return (game, board, p1, p2);
        }


            private int GetIntegerInput(string prompt, int min, int max)
            {
            int input;
            do
            {
                Console.Write(prompt);
                if(!int.TryParse(Console.ReadLine(), out input) || input < min || input > max)
                {
                    Console.WriteLine("Invalid value, please try again.");
                }

            } while (input < min || input > max);

            return input;
        }
    }



    public class SaveSupportSystem
    {
        public static void SaveGame(string filePath, Game game, bool isPLayer1Turn)
        {
            string jsonData = game.ToJson(isPLayer1Turn);
            File.WriteAllText(filePath, jsonData);
        }

    }

    public class LoadSupportSystem
    {
        public (Game game,Board board, IPlayer p1, IPlayer p2, bool isPlayer1Turn) LoadGame(string filePath)
        {
            string jsonData = File.ReadAllText(filePath);
            JObject jsonObject = JObject.Parse(jsonData); //put data from json file into jason object

            //extract objects from json
            string boardType = jsonObject["BoardType"].ToString();
            bool isPlayer1Turn = (bool)jsonObject["isPlayer1Turn"];
            JObject p1Object = (JObject)jsonObject["p1"];
            JObject p2Object = (JObject)jsonObject["p2"];
            JObject boardObject = (JObject)jsonObject["board"];

            //ReCreate Board
            Board board = null;
            JArray boardGridArray = (JArray)boardObject["boardGrid"];
            switch (boardType)
            {
                case "TreblecrossBoard":
                    board = new TreblecrossBoard((int)jsonObject["board"]["BoardSizeX"]);
                    string[] boardGrid = boardGridArray.Select(x => x.ToString()).ToArray();
                    board.LoadGrid(boardGrid);
                    break;
                case "ReversiBoard":
                    board = new ReversiBoard();
                    boardGrid = boardGridArray.Select(x => x.ToString()).ToArray();
                    board.LoadGrid(boardGrid);
                    break;
                default:
                    throw new InvalidOperationException($"Unknown board type: {boardType}");
            }

            //Recreate Players
            IPlayer p1 = null;
            IPlayer p2 = null;

            string p1Name = p1Object["Name"].ToString();
            string p2Name = p2Object["Name"].ToString();

            p1 = new HumanPlayer(p1Name);
            if (p2Name != "Computer")
            {
                p2 = new HumanPlayer(p2Name);
            } else
            {
                p2 = new ComputerPlayer(p2Name);
            }

            //Recreate Game
            Game game = null;
            if (board is TreblecrossBoard)
            {
                game = new TreblecrossGame(board, p1, p2, true);
            }
            else if (board is ReversiBoard)
            {
                game = new ReversiGame(board, p1, p2, true);
            }

            return (game, board, p1, p2, isPlayer1Turn);
        }

    }

    public class HelpSupportSystem
    {
        public void ShowRule(int userGameChoice)
        {
            string gameName;
            //Treblecross Game
            if (userGameChoice == 1) 
            {
                gameName = "Treblecross";
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("This game is {0}", gameName);
                Console.WriteLine("***********");
                Console.WriteLine("What is it?");
                Console.WriteLine("***********");
                Console.WriteLine("The game is an octal game played on a one-dimensional board and both players play using the same piece (an X)");
                Console.WriteLine("Each player on their turn plays a piece in an unoccupied space.");
                Console.WriteLine("***********");
                Console.WriteLine("How to win?");
                Console.WriteLine("***********");
                Console.WriteLine("The game is won if a player on their turn makes a line of 3 pieces (Xs) in a row");
            } else if (userGameChoice == 2)
            {
                Console.WriteLine("Now still under constructioin");
            }
        }
    }



}

