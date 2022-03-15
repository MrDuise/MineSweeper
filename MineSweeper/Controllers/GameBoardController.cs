using Microsoft.AspNetCore.Mvc;
using MineSweeper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MineSweeper.Controllers
{
    public class GameBoardController : Controller
    {
        static List<ButtonModel> buttons = new List<ButtonModel>();
        Random random = new Random();
        const int GRID_SIZE = 25;
        public IActionResult Index()
        {
            if (buttons.Count < GRID_SIZE)
            {
                for (int i = 0; i < GRID_SIZE; i++)
                {
                    buttons.Add(new ButtonModel(i));
                }
            }
            return View(buttons);
        }

        public IActionResult HandleButtonClick(string buttonNumber)
        {
           
            int bn = int.Parse(buttonNumber);
            if (buttons.ElementAt(bn).enabled == true)
            {
                buttons.ElementAt(bn).visited = true;
            } 

            return View("Index", buttons);
        }

        public IActionResult ShowOneButton(int buttonNumber)
        {
            return PartialView(buttons.ElementAt(buttonNumber));
        }

        public IActionResult RightClickShowButton(int buttonNumber)
        {
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
            } else
            {
                buttons.ElementAt(buttonNumber).flag = false;
                buttons.ElementAt(buttonNumber).enabled = true;
            }
        }
    }
}