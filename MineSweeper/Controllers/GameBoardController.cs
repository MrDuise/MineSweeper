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


        static List<ButtonModel> buttons = new List<ButtonModel>();
        public BoardModel board = new BoardModel(8, 25);
        int playingNonBombs = 0;//counter to hold the number of cells that the user has clicked on that are not bombs
        int numNonBombs = 0;//number of cells that are not bombs



        Random random = new Random();
        const int buttons_SIZE = 64;
        int size = Convert.ToInt32(Math.Sqrt(buttons_SIZE));


        public IActionResult Index()
        {

            int index = 0;

            if (buttons.Count < buttons_SIZE)
            {
                for (int row = 0; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        buttons.Add(new ButtonModel(index));
                        buttons[index].row = row;
                        buttons[index].column = col;
                        index++;
                    }
                }

            }

            int i = 0;
            //sets up the bombs
            //sets the live neighbors of every cell
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if (board.Grid[row, col].live == true)
                    {
                        buttons[i].live = true;
                    }
                    buttons.ElementAt(i).liveNeighbors = board.Grid[row, col].liveNeighbors;
                    Console.WriteLine("board model live neighbors: " + board.Grid[row, col].liveNeighbors);
                    Console.WriteLine("buttons view live neighbors: " + buttons.ElementAt(i).liveNeighbors);



                    i++;
                }
            }


            Console.WriteLine(numNonBombs);
            return View("Index", buttons);

        }





        public IActionResult HandleButtonClick(string buttonId)
        {
            string[] values = buttonId.Split(' ');
            int row = Convert.ToInt32(values[0]);
            int col = Convert.ToInt32(values[1]);
            Console.WriteLine(row + " " + col);

            int bn = row * size + col;
            Console.WriteLine(bn);
            if (buttons.ElementAt(bn).enabled == true)
            {
                buttons.ElementAt(bn).visited = true;
            }

            board.FloodFill(row, col);

            refresh();

            playingNonBombs = board.caliculateCellsNotBombsOnBoardWhilePlaying();

            //counter to hold the number of non live cells
            numNonBombs = caliculateNonBombsOnBoard();


            //Console.WriteLine(buttonId);
            //int row = buttonId/size;
            //int col = buttonId%size;

            //if button clicked on was a bomb
            if (buttons.ElementAt(row * size + col).live == true)
            {
                //sets the trigger to disable the buttons
                buttons.ForEach(button => button.disabled = true);
                revealBombs();

                return GameLost();
            }
            else if (playingNonBombs == numNonBombs)
            {
                Console.WriteLine("checking end game contiontion " + playingNonBombs + " " + numNonBombs);
                //not real view name
                return GameWon();
            }
            return View("Index", buttons);
        }

        public IActionResult GameWon()
        {
            return View("GameWon");
        }

        public IActionResult GameLost()
        {

            return View("GameLost", buttons);
        }

        public IActionResult ShowOneButton(int buttonNumber)
        {
            //string[] values = buttonNumber.Split(' ');
            //int row = Convert.ToInt32(values[0]);
            //int col = Convert.ToInt32(values[1]);
            //Console.WriteLine(row + " " + col);

            //int bn = row * size + col;
            return PartialView("ShowOneButton", buttons.ElementAt(buttonNumber));
        }

        public IActionResult RightClickShowButton(int buttonNumber)
        {
           // string[] values = buttonNumber.Split(' ');
            //int row = Convert.ToInt32(values[0]);
            //int col = Convert.ToInt32(values[1]);
            //Console.WriteLine(row + " " + col);

            //int bn = row * size + col;
            if (buttons.ElementAt(buttonNumber).visited == false)
            {
                flagSwap(buttonNumber);

            }
            return PartialView("ShowOneButton", buttons.ElementAt(buttonNumber));
        }

        public void flagSwap(int buttonNumber)
        {
            if (buttons.ElementAt(buttonNumber).flag == false)
            {
                buttons.ElementAt(buttonNumber).flag = true;
                buttons.ElementAt(buttonNumber).enabled = false;
            }
            else
            {
                buttons.ElementAt(buttonNumber).flag = false;
                buttons.ElementAt(buttonNumber).enabled = true;
            }
        }


        public void revealBombs()
        {
            int i = 0;
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if (buttons.ElementAt(row * size + col).live == true)
                    {
                        buttons[i].visited = true;
                    }
                    i++;
                }
            }
        }

        public void refresh()
        {
            int i = 0;
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if (board.Grid[row, col].visited == true && board.Grid[row,col].live == false)
                    {
                        buttons[i].visited = true;
                    }
                    i++;
                }
            }
        }


        /// <summary>
        /// Caliculates the number of cells that are not bombs on the board
        /// </summary>
        /// <returns></returns>
        public int caliculateNonBombsOnBoard()
        {
            int numNonBombs = 0;
            for (int a = 0; a < size; a++)
            {
                for (int b = 0; b < size; b++)
                {
                    if (buttons.ElementAt(a * size + b).live == false)
                    {
                        numNonBombs++;
                    }
                }
            }
            return numNonBombs;
        }





    }
}
