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
        
        // GET: /<controller>/
        public IActionResult Index()
        {
            var projects = this._projectsService.GetAll();
            return View(projects);
        }

        public IActionResult Post()
        {
            var tupleModel = new Tuple<List<Skill>, Project >((List<Skill>)this._skillsService.GetAll(), new Project());
            return View(tupleModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Post([Bind(Prefix = "Item2")] Project project, int[] skills)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The submitted input is not valid");
            }
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (currentUserID == null)
            {
                return NotFound("User not found");
            }

            var projectCreated = this._projectsService.PostProject(currentUserID, project, skills);

            if (!projectCreated)
            {
                throw new Exception("Could not create the project.");
            }


            return RedirectToAction(nameof(Index));

        }

        public IActionResult Learn(int id)
        {
            var project = this._projectsService.GetProject(id);
            return View(project);
        }

    }
}
