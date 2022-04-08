using Microsoft.AspNetCore.Mvc;
using MineSweeper.Models;
using MineSweeper.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace MineSweeper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class APIController : Controller
    {
        SaveDAO dao = new SaveDAO();

        [HttpGet]
        [ResponseType(typeof(List<BoardDTO>))]
        public IEnumerable<BoardDTO> Index()
        {
            List<BoardModel> savedBoards = dao.AllBoards();


            IEnumerable<BoardDTO> boardDTOList = from b in savedBoards
                                                 select
                                                 new BoardDTO(b.size, cellString(b), b.difficulity);
            return boardDTOList;
        }

        public string cellString(BoardModel board)
        {
            string cellString = "";
            for (int x = 0; x < board.size; x++)
            {
                for (int y = 0; y < board.size; y++)
                {
                    cellString = cellString + "{";
                    cellString += board.Grid[x, y].id.ToString() + ",";
                    cellString += board.Grid[x, y].row.ToString() + ",";
                    cellString += board.Grid[x, y].column.ToString() + ",";
                    cellString += board.Grid[x, y].visited.ToString() + ",";
                    cellString += board.Grid[x, y].live.ToString() + ",";
                    cellString += board.Grid[x, y].liveNeighbors.ToString() + ",";
                    cellString += board.Grid[x, y].flag.ToString() + ",";
                    cellString += board.Grid[x, y].enabled.ToString();
                    cellString = cellString + "}";
                }
            }
            return cellString;
        }


        [HttpGet("showoneboard/{Id}")]
        [ResponseType(typeof(BoardDTO))]
        public ActionResult<BoardDTO> ShowOneBoard(int Id)
        {
            BoardModel board = dao.GetBoardById(Id);
            BoardDTO boardDTO = new BoardDTO(board.size, board.Grid.ToString(), board.difficulity);

            return boardDTO;
        }


        [HttpDelete("delete/{Id}")]
        public bool deleteOneBoard(int Id)
        {
            return dao.deleteSave(Id);
        }


    }
}
