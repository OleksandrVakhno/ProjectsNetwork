using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ProjectsNetwork.Areas.User.Controllers
{
    [Area("User")]
    public class CommunicationController : Controller
    {

        public CommunicationController()
        {

        }


        public IActionResult UserInfo(string user)
        {


            return View();
        }
    }
}
