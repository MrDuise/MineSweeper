using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MineSweeper.Controllers
{
    public class BoardSetUpController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
