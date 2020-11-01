using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectsNetwork.Data;
using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using ProjectsNetwork.Models;
using ProjectsNetwork.Services.IServices;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectsNetwork.Controllers
{
    [Authorize]
    [Route("User/Posts")]
    [Area("User")]
    public class PostsController : Controller
    {
        
       
        private readonly IProjectsService _projectsService;
        private readonly ISkillsService _skillsService;

        public PostsController(IProjectsService projectsService, ISkillsService skillsService)
        {
            this._projectsService = projectsService;
            this._skillsService = skillsService;
        }


        [Route("")]
        public IActionResult Index()
        {
            var projects = this._projectsService.GetAll();
            return View(projects);
        }

        [Route("MyProjects")]
        public IActionResult MyProjects()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var projects = this._projectsService.GetUserProjects(currentUserID);
            return View(projects);
        }

        [Route("Post")]
        public IActionResult Post()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (currentUserID == null)
            {
                return NotFound("User not found");
            }

            var project = new Project();
            project.UserId = currentUserID;
           
            var tupleModel = new Tuple<List<Skill>, Project, Skill>((List<Skill>)this._skillsService.GetAll(), project, new Skill());
            return View(tupleModel);
        }

        [HttpPost]
        [Route("Post")]
        [ValidateAntiForgeryToken]
        public IActionResult Post([Bind(Prefix = "Item2")] Project project, int[] skills)
        {

           
            if (!ModelState.IsValid)
            {
                return BadRequest("The submitted input is not valid");
            }
           

            var projectCreated = this._projectsService.PostProject(project, skills);

            if (!projectCreated)
            {
                throw new Exception("Could not create the project.");
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

            return RedirectToAction(nameof(Post));
        }

        [Route("Learn")]
        public IActionResult Learn(int id)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var project = this._projectsService.GetProject(id);
            var skills = this._skillsService.GetProjectSkills(id);
            var interest = project.UsersInterested.Find(interseted => interseted.UserId == currentUserID);
            var interested = interest != null;
            var learnModel = new Learn(project, skills, interested);
            return View(learnModel);
        }

        [Route("SubmitInterest")]
        public IActionResult SubmitInterest(int projectId)
        {

            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (currentUserID == null)
            {
                return NotFound("User not found");
            }

            if (!this._projectsService.SubmitInterest(currentUserID, projectId))
            {
                return BadRequest();
            }

            return RedirectToAction("Learn", new { id = projectId });
        }

    }
}
