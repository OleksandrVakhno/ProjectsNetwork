using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectsNetwork.Models;
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
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (currentUserID == null)
            {
                return NotFound("User not found");
            }
            var mySkills = this._skillsService.GetMySkills(currentUserID);
            var tupleModel = new Tuple<List<Skill>, List<Skill>, Skill>((List<Skill>)skills, mySkills, new Skill());
            return View(tupleModel);
        }

        [HttpPost]
        [Route("PostUserSkills")]
        public IActionResult PostUserSkills(int[] skills)
        {

            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (currentUserID == null)
            {
                return NotFound("User not found");
            }

            var addUserSkills = this._skillsService.PostUserSkills(currentUserID, skills);

            if (!addUserSkills)
            {
                throw new Exception("Could not add skills.");
            }


            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        [Route("AddNewSkill")]
        public IActionResult AddNewSkill(Skill skill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The submitted input is not valid");
            }

            var addedSkill = this._skillsService.AddSkill(skill);

            if (!addedSkill)
            {
                throw new Exception("Could not add skill.");
            }

            return RedirectToAction(nameof(Index));
        }
    }

    
}
