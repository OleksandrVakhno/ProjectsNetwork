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


        public IActionResult Index(string filterWord)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            IEnumerable<Project> projects;
            ViewData["Title"] = "Project Posts";
            ViewData["CurrentFilter"] = filterWord;

            if (!String.IsNullOrEmpty(filterWord))
            {
                projects = this._projectsService.GetFiltered(filterWord, p => p.UserId != currentUserID);
            }
            else
            {
                projects = this._projectsService.GetAll(p => p.UserId != currentUserID);
            }

            return View(projects);

        }

        public IActionResult MyProjects()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var projects = this._projectsService.GetUserProjects(currentUserID);
            ViewData["Title"] = "My Projects";
            return View("Index", projects);
        }

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

        public IActionResult Update(int projectId, int[] skills)
        {
            var updatedProject = this._projectsService.UpdateProject(projectId, skills);

            if (!updatedProject)
            {
                throw new Exception("Could not update the project");
            }

            return RedirectToAction("Learn", new { id = projectId });
        }

        [HttpPost]
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


            return RedirectToAction(nameof(MyProjects));

        }

        [HttpPost]
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

        public IActionResult Learn(int id)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var project = this._projectsService.GetProject(id);
            var skills = this._skillsService.GetProjectSkills(id);
            var interest = project.UsersInterested.Find(interseted => interseted.UserId == currentUserID);
            var interested = interest != null;
            var accepted = false;
            if (interested)
            {
                accepted = interest.Confirmed;
            }

            var learnModel = new Learn(project, skills, accepted, interested);
            return View(learnModel);
        }

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

        public IActionResult CancelInterest(int projectId)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (currentUserID == null)
            {
                return NotFound("User not found");
            }
            if (!this._projectsService.CancelInterest(currentUserID, projectId))
            {
                return BadRequest();
            }

            return RedirectToAction("Learn", new { id = projectId });
            


        }

    }
}
