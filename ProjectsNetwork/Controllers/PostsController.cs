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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectsNetwork.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ISkillRepository _skillRepository;

        public PostsController(IProjectRepository projectRepository, ISkillRepository skillRepository)
        {
            this._projectRepository = projectRepository;
            this._skillRepository = skillRepository;
        }
        
        // GET: /<controller>/
        public IActionResult Index()
        {
            var projects = this._projectRepository.GetAll();
            return View(projects);
        }

        public IActionResult Post()
        {
            var tupleModel = new Tuple<List<Skill>, Project >((List<Skill>)this._skillRepository.GetAll(), new Project());
            return View("PostForm",tupleModel);
        }

        [HttpPost]
        public IActionResult Post([Bind(Prefix = "Item2")] Project project, int[] skills)
        {
            project.CreationDate = DateTime.Now;
            try
            {
                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                project.UserId = currentUserID;
            }catch(Exception e)
            {
                return View("../Shared/Error",e);
            }

            if (skills != null)
            {
                List<ProjectSkill> prefferedSkills = new List<ProjectSkill>();
                foreach(int x in skills)
                {
                    var cur = new ProjectSkill();
                    cur.ProjectId = project.Id;
                    cur.SkillId = x;
                    prefferedSkills.Add(cur);
                }
                project.PrefferedSkills = prefferedSkills;
            }
            
            //return Json(project);

            try
            {
                this._projectRepository.Insert(project);
            }catch(Exception e)
            {
                return View("../Shared/Error",e);
            }
            if (this._projectRepository.Save() >= 1)
            {
                var projects = this._projectRepository.GetAll();
                return View("Index", projects);
            }
            return View("../Shared/Error");

        }

        public IActionResult Learn(int id)
        {
            var project = this._projectRepository.Get(id);
            return View("Learn",project);
        }

    }
}
