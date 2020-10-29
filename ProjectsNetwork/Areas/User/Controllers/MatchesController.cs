using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectsNetwork.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class MatchesController : Controller
    {
        public IActionResult Accepted()
        {
            return View();
        }


        public IActionResult Matches(int projectId)
        {
            return View();
        }

        public IActionResult Interested(int projectId)
        {
            return View();
        }
    }
}
