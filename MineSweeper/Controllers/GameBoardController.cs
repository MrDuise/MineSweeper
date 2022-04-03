using Microsoft.AspNetCore.Mvc;
using MineSweeper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;



/**
 * NOTES
 * Formula to turn row and col into an ID number
 * row * size + col
 * ex. (2 * 8) + 3
 * to turn an ID into a row and col 
 * use ID/ size to get the row
 * use ID % size to get the column number
 */

namespace MineSweeper.Controllers
{
    public class GameBoardController : Controller
    {

        static BoardModel board;

        double difficulity = 0.00;
        int size;


        public IActionResult PageSetup(string boardSize, string gameDiff)
        {
            size = Convert.ToInt32(boardSize);
            difficulity = Convert.ToDouble(gameDiff);
            board = new BoardModel(size, difficulity);
            return Index();
        }

        public IActionResult Index()
        {
            return View("Index", board);
        }





        public IActionResult HandleButtonClick(int row, int col)
        {
           
            //calls board.leftClick and checks for flag
            PlaceTurn(row, col);

            //check for end-game contitions
            if (board.checkForLose())
            {
                board.revealBombs();

                return PartialView("GameLost", board);
            }
            else if (board.checkForWin())
            {
                //not real view name
                return GameWon();
            }
            return PartialView("ShowOneButton", board);
        }


        public IActionResult ShowOneButton()
        {

            return PartialView("ShowOneButton", board);
        }

        public IActionResult RightClickShowButton(int row, int col)
        {
            flagSwap(row, col);

            return PartialView("ShowOneButton", board);
        }

        public void flagSwap(int row, int col)
        {

            if (board.Grid[row, col].flag == false)
            {
                board.Grid[row, col].flag = true;
                board.Grid[row, col].enabled = false;
                board.Grid[row, col].visited = true;
            }
            else
            {
                board.Grid[row, col].flag = false;
                board.Grid[row, col].enabled = true;
                board.Grid[row, col].visited = false;
            }
        }



        public void PlaceTurn(int rowNumber, int columnNumber)
        {
            //used for testing
            Console.WriteLine(rowNumber + " " + columnNumber);


            if (board.Grid[rowNumber, columnNumber].enabled == true)
            {
                board.Grid[rowNumber, columnNumber].visited = true;
            }
            // update board.  false = update only one square.
            board.leftClick(rowNumber, columnNumber);

        }







        public IActionResult GameWon()
        {
            return View("GameWon");
        }

        public IActionResult GameLost()
        {

            return PartialView("GameLost", board);
        }






    }
}
