using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MineSweeper.Models
{
    public class BoardDTO
    {
        public int size { get; set; }
        public string grid { get; set; }
        public double difficulty { get; set; }

        public BoardDTO(int sizeVal, string gridVal, double difficultyVal)
        {
            size = sizeVal;
            difficulty = difficultyVal;
            grid = gridVal;
        }
    }
}
