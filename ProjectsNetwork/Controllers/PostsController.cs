using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectsNetwork.Data;
using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using ProjectsNetwork.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectsNetwork.Controllers
{
    public class PostsController : Controller
    {
        private readonly IProjectRepository _projectRepository;

        public PostsController(IProjectRepository projectRepository)
        {
            this._projectRepository = projectRepository;
        }
        
        // GET: /<controller>/
        public IActionResult Index()
        {
            var projects = this._projectRepository.GetAll();
            return View(projects);
        }

        public IActionResult Post()
        {
            return View("PostForm");
        }

        [HttpPost]
        public IActionResult Post(Project project)
        {
            project.CreationDate = DateTime.Now;
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            project.UserId = currentUserID;

            //return Json(project);

            this._projectRepository.Insert(project);
            this._projectRepository.Save();
            var projects = this._projectRepository.GetAll();
            return View("Index", projects);
        }

        public IActionResult Learn(int id)
        {
            var project = this._projectRepository.Get(id);
            return View("Learn",project);
        }

    }
}
