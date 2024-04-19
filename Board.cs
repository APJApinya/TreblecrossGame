using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFN563_Assessment2
{
    public abstract class Board
    {
        public int BoardSizeX { get; set; }
        public int BoardSizeY { get; set; }
        public string[] boardGrid;
        public string[] GetBoardGrid()
        {
            return boardGrid;
        }

        public abstract void DisplayBoard();
        public void SetPiece(int gridIndex)
        {
            boardGrid[gridIndex] = "X";
        }
        public void ClearPiece(int gridIndex)
        {
            boardGrid[gridIndex] = (gridIndex + 1).ToString();
        }
        public void LoadGrid(string [] boardGridFromFile)
        {
            boardGrid = boardGridFromFile;
        }
    }

    public class TreblecrossBoard : Board
    {
        public TreblecrossBoard(int boardSizeX, int boardSizeY = 1)
        {
            BoardSizeX = boardSizeX;
            BoardSizeY = boardSizeY;
            boardGrid = new string[boardSizeX];
            int x = 1;
            for (int i = 0; i < boardGrid.Length; i++)
            {
                string myString = x.ToString();
                boardGrid[i] = myString;
                x++;
            }
        }
        public override void DisplayBoard()
        {
            int gridSpace = (boardGrid.Length * 4) + 1;
            string lineGrid = new string('-', gridSpace);
            Console.WriteLine(lineGrid);
            Console.Write("| ");
            for (int i = 0; i < boardGrid.Length; i++)
            {
                Console.Write(boardGrid[i] + " | ");
            }
            Console.WriteLine();
            Console.WriteLine(lineGrid);
        }
    }

    public class ReversiBoard : Board
    {
        public ReversiBoard(int boardSizeX = 8, int boardSizeY = 8)
        {
            BoardSizeX = boardSizeX;
            BoardSizeY = boardSizeY;
        }
        public override void DisplayBoard()
        {
            throw new NotImplementedException();
        }
    }

}


