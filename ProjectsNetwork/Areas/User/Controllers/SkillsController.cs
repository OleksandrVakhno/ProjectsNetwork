using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectsNetwork.Services.IServices;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectsNetwork.Controllers
{
    
    [Route("User/Skills",
        Name = "skill")]
    [Area("User")]
    public class SkillsController : Controller
    {

        private readonly ISkillsService _skillsService;

        public SkillsController(ISkillsService skillsService)
        {
            this._skillsService = skillsService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var skills = this._skillsService.GetAll();
            //var mySkills = this.
            return View(skills);
        }

        [HttpPost]
        public IActionResult Post(int[] skills)
        {
            
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (currentUserID == null)
            {
                return NotFound("User not found");
            }

            var addUserSkills = this._skillsService.PostSkills(currentUserID,skills);

            if (!addUserSkills)
            {
                throw new Exception("Could not create the project.");
            }

            
            return RedirectToAction(nameof(Index));

        }
    }
}
