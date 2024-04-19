using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace IFN563_Assessment2
{
    public abstract class Game
    {
        public bool isPlayer1Turn;
        private List<int> previousMoves = new List<int>();
        public IPlayer p1 { get; set; }
        public IPlayer p2 { get; set; }
        public Board board { get; set; }
        public string BoardType { get; set; }

        [JsonConstructor]
        public Game(Board board, IPlayer p1, IPlayer p2, bool isPlayer1Turn)
        {
            this.board = board;
            this.p1 = p1;
            this.p2 = p2;
            this.isPlayer1Turn = isPlayer1Turn;
            this.BoardType  = board.GetType().Name;
        }
        public Game() 
        {
        }  

        //check isMoveValid --> if valid setPiece()
        public abstract bool CheckMove(string move, Board board);

        public abstract (bool isGameover, bool isSomeoneWin) CheckWin(Board board);

        public bool GetMove(IPlayer player, Board board)
        {
            int move = player.MakeMove(board);

            if (CheckMove(move.ToString(), board))
            {
                previousMoves.Add(move);
                board.SetPiece(move - 1);
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("Updated gameboard");
                board.DisplayBoard();
                return true;
            }
            else
            {
                Console.WriteLine("Your move is invalid, please choose another location");
                return false;
            }
        }
        public void EndGame(IPlayer player, bool isGameover, bool isSomeoneWin)
        {
            if (isGameover && isSomeoneWin)
            {
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("-----------------------------------");
                Console.WriteLine($"!!!!!!Congratulation! {player.Name} win!!!!!!");
                Console.WriteLine("Thank you for playing this game.");
                Console.WriteLine("See you again.");
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("-----------------------------------");
            }
            else if (isGameover && !isSomeoneWin)
            {
                Console.WriteLine("Game Over!");
                Console.WriteLine("You runout of spaces");
                Console.WriteLine("Please try again next time");
            }


        }

        public bool UndoMove()
        {
            if (previousMoves.Count > 0)
            {
                int lastMove = previousMoves[previousMoves.Count - 1];
                board.ClearPiece(lastMove - 1);
                board.DisplayBoard();
                Console.Write("Confirm this move? (Y for Confirm, N for Redo to previous): ");
                string userChoice = Console.ReadLine().ToLower();
                if (userChoice == "n" || userChoice == "no" || userChoice == null)
                {
                    board.SetPiece(lastMove-1);
                    Console.WriteLine("This is your final board looks like:");
                    board.DisplayBoard();
                    return true;

                }
                else
                {
                    previousMoves.RemoveAt(previousMoves.Count - 1);
                    return false;
                }

            }
            else
            {
                Console.WriteLine("Cannot undo");
                return true;
            }
        }


        //confirm user for endturn
        public void ConfirmEndTurn(IPlayer player, Board board, bool isPlayer1Turn)
        {
            Console.WriteLine("Please choose what you want to do next");
            Console.WriteLine("1. End your turn");
            Console.WriteLine("2. Save your game progress");
            Console.WriteLine("3. Quit the game");
            Console.Write("Choose your choice: ");
            int userChoice = int.Parse(Console.ReadLine());
            switch (userChoice)
            {
                case 1:
                    break;
                case 2:
                    const string FILE = "savefile.json";
                    SaveSupportSystem.SaveGame(FILE, this, isPlayer1Turn);
                    Console.WriteLine("GoodBye");
                    Environment.Exit(0);
                    break;
                case 3:
                    Console.WriteLine("GoodBye");
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }

        public string ToJson(bool isPlayer1Turn)
        {
            var data = new
            {
                BoardType,
                isPlayer1Turn,
                p1,
                p2,
                board
            };

            return JsonConvert.SerializeObject(data); 
        }

        //factory create game object based input?? but we want user to select based on menu is that factory?
    }

    public class TreblecrossGame : Game
    {
        [JsonConstructor]
        public TreblecrossGame(Board board, IPlayer p1, IPlayer p2, bool isPlayer1Turn) : base(board, p1, p2, isPlayer1Turn)
        {
        }

        public TreblecrossGame() : base()
        {
        }

        public override bool CheckMove(string move, Board board)
        {
            string[] boardGrid = board.GetBoardGrid();
            if (boardGrid.Contains(move))
            {
                int gridIndex = int.Parse(move) - 1;

                return true;
            }
            else
            {
                return false;
            }
        }

        public override (bool isGameover, bool isSomeoneWin) CheckWin(Board board)
        {
            bool isGameover = false;
            bool isSomeoneWin = false;
            string[] boardGrid = board.GetBoardGrid();
            int countX = 0;
            for (int i = 0; i < boardGrid.Length; i++)
            {
                if (boardGrid[i] == "X")
                {
                    countX++;
                    if (countX == 3)
                    {
                        isGameover = true;
                        isSomeoneWin = true;
                    }
                }
                else
                {
                    countX = 0;
                }
            }
            //if X next to each other 3 in the row --> win
            if (countX == boardGrid.Length)
            {
                isGameover = true;
                isSomeoneWin = false;
            }

            return (isGameover, isSomeoneWin);
        }

    }

    public class ReversiGame : Game
    {
        [JsonConstructor]
        public ReversiGame(Board board, IPlayer p1, IPlayer p2, bool isPlayer1Turn) : base(board, p1, p2, isPlayer1Turn)
        {
        }

        public ReversiGame() : base()
        {
        }

        public override bool CheckMove(string move, Board board)
        {
            throw new NotImplementedException();
        }

        public override (bool isGameover, bool isSomeoneWin) CheckWin(Board board)
        {
            bool isGameover = false;
            bool isSomeoneWin = false;
            //still use same rule as Treblecross Game --> need to revise in future
            string[] boardGrid = board.GetBoardGrid();
            int countX = 0;
            for (int i = 0; i < boardGrid.Length; i++)
            {
                if (boardGrid[i] == "X")
                {
                    countX++;
                }
                else
                {
                    countX = 0;
                }
            }
            //if X next to each other 3 in the row --> win
            if (countX == 3)
            {
                isGameover = true;
                isSomeoneWin = true;
            }
            else if (countX == boardGrid.Length)
            {
                isGameover = true;
                isSomeoneWin = false;
            }

            return (isGameover, isSomeoneWin);

        }



    }
}

