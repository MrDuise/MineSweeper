using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MineSweeper.Models
{
    public class Cell
    {
        public int id { get; set; }
        public int row { get; set; }
        public int column { get; set; }
        public bool visited { get; set; }
        public bool live { get; set; }
        public int liveNeighbors { get; set; }
        public bool flag { get; set; }

        public string buttonFile { get; set; }
        public bool enabled { get; set; }

        /// <summary>
        /// Constructor. Sets properities to default values
        /// </summary>
        public Cell(int newID)
        {
            id = newID;
            row = 0;
            column = 0;
            visited = false;
            live = false;
            liveNeighbors = 0;
            flag = false;
            enabled = true;
        }
    }
}
