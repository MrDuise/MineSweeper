using Microsoft.AspNetCore.Mvc;
using MineSweeper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MineSweeper.Controllers
{
    public class Registration : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProcessRegister(UserModel user)
        {
            //SecurityService securityService = new SecurityService();
            if ("blank" == "blank")
            {
                return View("RegisterSuccess", user);
            }
            else
            {
                return View("RegisterFailure", user);
            }
        }
    }
}
