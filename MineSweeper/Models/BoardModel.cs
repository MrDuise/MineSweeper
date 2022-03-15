using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MineSweeper.Models
{
    public class BoardModel
    {
        //board is square, so size is both the length and width of the board
        public int size { get; private set; }

         public ButtonModel[,] Grid { get; private set; }

        //A percentage of cells that will be set to "live" status.
        public double difficulity = 0.00;

        /// <summary>
        /// Constructor. Takes a value for the size property
        /// </summary>
        public BoardModel(int sizeVal, double difficulityVal)
        {
            size = sizeVal;

            difficulity = difficulityVal;

            Grid = new ButtonModel[size, size];

            int index = 0;
            //initialize the array so a Button object is stored in each location

            for (int a = 0; a < size; a++)
            {
                for (int b = 0; b < size; b++)
                {
                    Grid[a, b] = new ButtonModel(index);
                    Grid[a, b].row = a;
                    Grid[a, b].column = b;
                    index++;
                }
            }

            setupliveNeighbors();
            //Grid[0, 0].live = true; //used for testing the end game contition 
            CalcliveNeighbors();

        }



        /// <summary>
        /// Calculate a cells live neighbors. Cells can have between 0 and 8 live neighbors.
        /// </summary>
        /// <returns></returns>
        public int calculateliveNeighborsForOneCell(int row, int col)
        {
            int liveNeighbors = 0;

            //sets the start and end row and column for the search. Search is in a 3*3 square around the inputted cell location
            int row_start = row - 1; int row_end = row + 1; int col_start = col - 1; int col_end = col + 1;

            //setup for if row or column is at the far ends of the grid
            if (row_start <= 0)
                row_start = 0;

            if (row_end == size)
                row_end = size - 1;


            if (col_start <= 0)
                col_start = 0;

            if (col_end == size)
                col_end = size - 1;


            //this section does the checking for the live bombs
            for (int i = row_start; i <= row_end; i++)
            {
                for (int j = col_start; j <= col_end; j++)
                {
                    if (Grid[i, j].live == true)
                        liveNeighbors++;
                }
            }

            return liveNeighbors;
        }

        /// <summary>
        /// Calcualtes the number of live bombs for the entire board and stores the numbers in each cells liveNeighbors property
        /// </summary>
        public void CalcliveNeighbors()
        {
            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {


                    // NW of current square
                    if (r - 1 >= 0 && c - 1 >= 0 && Grid[r - 1, c - 1].live) Grid[r, c].liveNeighbors++;

                    // N
                    if (r - 1 >= 0 && Grid[r - 1, c].live) Grid[r, c].liveNeighbors++;

                    // NE
                    if (r - 1 >= 0 && c + 1 < size && Grid[r - 1, c + 1].live) Grid[r, c].liveNeighbors++;

                    // W
                    if (c - 1 >= 0 && Grid[r, c - 1].live) Grid[r, c].liveNeighbors++;

                    // E
                    if (c + 1 < size && Grid[r, c + 1].live) Grid[r, c].liveNeighbors++;

                    // SW
                    if (r + 1 < size && c - 1 >= 0 && Grid[r + 1, c - 1].live) Grid[r, c].liveNeighbors++;

                    // S
                    if (r + 1 < size && Grid[r + 1, c].live) Grid[r, c].liveNeighbors++;

                    // SE
                    if (r + 1 < size && c + 1 < size && Grid[r + 1, c + 1].live) Grid[r, c].liveNeighbors++;

                }
            }
        }

        /// <summary>
        /// randomly inatilize the grid with live bombs. based off the difficulity property
        /// </summary>
        public void setupliveNeighbors()
        {
            int howHard = Convert.ToInt32(this.difficulity);
            Random rand = new Random();



            for (int a = 0; a < size; a++)
            {

                for (int b = 0; b < size; b++)
                {
                    int randNum = rand.Next(1, 100);
                    if (randNum <= howHard)
                    {
                        Grid[a, b].live = true;
                    }

                }
            }
        }

        /// <summary>
        /// Recursive floodfill function. keeps on calling itself until a live neighbor has been reached
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public void FloodFill(int r, int c)
        // use recursion to clear adjacent cells that are empty.
        {
            Console.WriteLine(Grid[r, c].live);
            //set current cell visited to true
            Grid[r, c].visited = true;

            // if current cell has a live neighbor, then stop.
            if (Grid[r, c].liveNeighbors > 0) return;

            // N.  Call flood fill on the cell north of here if it has not yet been visited.
            if (r - 1 >= 0 && Grid[r - 1, c].visited == false)
                FloodFill(r - 1, c);

            // S
            if (r + 1 < size && Grid[r + 1, c].visited == false)
                FloodFill(r + 1, c);

            // W
            if (c - 1 >= 0 && Grid[r, c - 1].visited == false)
                FloodFill(r, c - 1);

            // E
            if (c + 1 < size && Grid[r, c + 1].visited == false)
                FloodFill(r, c + 1);

            // NW
            if (r - 1 >= 0 && c - 1 >= 0 && Grid[r - 1, c - 1].visited == false)
                FloodFill(r - 1, c - 1);

            // NE
            if (r - 1 >= 0 && c + 1 < size && Grid[r - 1, c + 1].visited == false)
                FloodFill(r - 1, c + 1);

            // SW
            if (r + 1 < size && c - 1 >= 0 && Grid[r + 1, c - 1].visited == false)
                FloodFill(r + 1, c - 1);

            // SE
            if (r + 1 < size && c + 1 < size && Grid[r + 1, c + 1].visited == false && Grid[r, c].liveNeighbors == 0)
                FloodFill(r + 1, c + 1);

        }

        public bool isValid(int r, int c)
        {
            return (r >= 0 && r < size && c >= 0 && c < size);
        }

    

        /// <summary>
        /// calculates the number of cells that are not bombs that have been visited while playing the game
        /// </summary>
        /// <returns></returns>
        public int caliculateCellsNotBombsOnBoardWhilePlaying()
        {
            int numNonBombsInGame = 0;
            for (int a = 0; a < size; a++)
            {
                for (int b = 0; b < size; b++)
                {
                    if (Grid[a, b].live == false && Grid[a, b].visited == true && Grid[a, b].flag == false)
                    {
                        numNonBombsInGame++;
                    }
                }
            }
            return numNonBombsInGame;
        }

    }
}
