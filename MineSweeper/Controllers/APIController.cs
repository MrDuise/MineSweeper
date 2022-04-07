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
                                                 new BoardDTO(b.size, b.Grid.ToString(), b.difficulity);
            return boardDTOList;
        }

        [HttpGet("showoneboard/{Id}")]
        [ResponseType(typeof(BoardDTO))]
        public ActionResult<BoardDTO> ShowOneBoard(int Id)
        {
            BoardModel board = dao.GetBoardById(Id);
            BoardDTO boardDTO = new BoardDTO(board.size, board.Grid.ToString(), board.difficulity);

            return boardDTO;
        }

    }
}
