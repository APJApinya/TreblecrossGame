using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFN563_Assessment2
{
    public interface IPlayer
    {
        string Name { get; set; }

        int MakeMove(Board board);
    }

    public class HumanPlayer : IPlayer
    {
        public string Name { get; set; }
        public HumanPlayer(string playerName)
        {
            Name = playerName;
        }
        public int MakeMove(Board board)
        {
            //Prompt user to input where to place piece
            Console.Write("Input where do you want to place: ");
            int move = int.Parse(Console.ReadLine());
            if (move == 0 || move > board.BoardSizeX)
            {
                Console.WriteLine("Your input is invalid, please input the correct number from the board");

            }

            return move;
        }
    }

    public class ComputerPlayer : IPlayer
    {
        public string Name { get; set; }

        public ComputerPlayer(string playerName = "Computer")
        {
            Name = playerName;
        }

        public int MakeMove(Board board)
        {
            string[] boardGrid = board.GetBoardGrid();
            int boardGridLength = 0;
            for (int i = 0; i < boardGrid.Length; i++)
            {
                if (boardGrid[i] != "X")
                {
                    boardGridLength++;
                }
            }
            int[] randomChoices = new int[boardGridLength];
            int x = 0;
            for (int i = 0; i < boardGrid.Length; i++)
            {
                int myInteger;
                if (boardGrid[i] != "X" && int.TryParse(boardGrid[i], out myInteger))
                {
                    randomChoices[x] = myInteger;
                    x++;
                }
            }

            Random random = new Random();
            int moveIndex = random.Next(1, boardGridLength);
            int move = randomChoices[moveIndex - 1];
            //how to generate if we have reversi game in out platform

            return move;
        }
    }

}

