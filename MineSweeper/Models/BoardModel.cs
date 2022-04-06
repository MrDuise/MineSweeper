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

        public Cell[,] Grid { get; private set; }

        //A percentage of cells that will be set to "live" status.
        public double difficulity = 0.00;

        /// <summary>
        /// Constructor. Takes a value for the size property
        /// </summary>
        public BoardModel(int sizeVal, double difficulityVal)
        {
            size = sizeVal;

            difficulity = difficulityVal;

            Grid = new Cell[size, size];

            int index = 0;
            //initialize the array so a Button object is stored in each location

            for (int a = 0; a < size; a++)
            {
                for (int b = 0; b < size; b++)
                {
                    Grid[a, b] = new Cell(index)
                    {
                        row = a,
                        column = b
                    };
                    index++;
                }
            }

            setupBombs();
            //Grid[0, 0].live = true; //used for testing the end game contition 
            //Grid[0, 0].flag = true;
            //Grid[0, 0].visited = true;
            CalcliveNeighbors();

        }

        public BoardModel(int sizeVal, double difficultyVal, Cell[,] cells)
        {
            size = sizeVal;

            difficulity = difficultyVal;

            Grid = cells;

            setupBombs();
            //Grid[0, 0].live = true; //used for testing the end game contition 
            //Grid[0, 0].flag = true;
            //Grid[0, 0].visited = true;
            CalcliveNeighbors();
        }



        // Place bombs on some squares
        public void setupBombs()
        {
            // Random number generator for calculating bomb placement
            Random random = new Random();

            // Loop through entire grid
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    // choose a random number between 0.00 and 1.00.  If random  < difficulty then place a bomb on this square.
                    Grid[i, j].live = random.NextDouble() <= difficulity;
                }
            }

        }

        // Calculate how many neighbors are live / bombs
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

        internal bool checkForWin()
        {
            // assume victory until proven otherwise.
            bool won = true;

            // double for loop to check every cell status.
            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    if (Grid[r, c].visited == false && Grid[r, c].live == false)
                    {
                        // if a cell is not visited and does not have a bomb, then the game is not over. must continue playing.
                        won = false;
                        // break because there is no need to continue checking other cells on the board.
                        break;
                    }
                    if (Grid[r, c].live == true && Grid[r, c].flag == false)
                    {
                        // if a cell has a bomb and does not have a flag, then the game is not over. must continue.
                        won = false;
                        break;
                    }
                }
                // break because there is no need to continue checking other cells on the board.
                if (!won) break;
            }
            return won;
        }

        internal bool checkForLose()
        {
            bool lost = false;

            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    if (Grid[r, c].live && Grid[r, c].visited && !Grid[r, c].flag)
                    {
                        lost = true;
                    }
                }
            }

            return lost;
        }


        internal void leftClick(int rowGuess, int colGuess)
        {
            if (!Grid[rowGuess, colGuess].flag)
            {
                FloodFill(rowGuess, colGuess);

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


        public void revealBombs()
        {
            int i = 0;
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    if (Grid[row, col].live == true)
                    {
                        Grid[row, col].visited = true;
                    }
                    i++;
                }
            }
        }







    }
}
