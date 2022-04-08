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

    //[ApiController]
    //[Route("api/[controller]")]

    public class SaveController : Controller
    {
        SaveDAO saveGame = new SaveDAO();

        //[HttpPut("processSave")]
        public IActionResult ProcessSave()
        {
            if(saveGame.createSave(GameBoardController.board, "ThatGhostToast"))
            {
                return PartialView("SaveCreated");

            } else
            {
                return PartialView("SaveFailed");
            }
        }

        //[HttpGet("getSavedBoard")]
        //[ResponseType(typeof(BoardModel))]

        public ActionResult<List<BoardModel>> GetSavedBoard(string username)
        {
            List<BoardModel> boards = saveGame.getSavedBoard(username);

            return boards;
       
        }

    }
}
