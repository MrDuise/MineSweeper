using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MineSweeper.Models
{
    public class ButtonModel
    {
       public int id { get; set; }
        public bool visited { get; set; }
        public bool live { get; set; }
        public int liveNeighbors { get; set; }
        public bool flag { get; set; }
        public string buttonFile { get; set; }
        public bool enabled { get; set; }

        /// <summary>
        /// Constructor. Sets properities to default values
        /// </summary>
        public ButtonModel(int ID)
        {
            id = ID;
            visited = false;
            live = false;
            liveNeighbors = 0;
            flag = false;
            enabled = true;
        }
    }
}
