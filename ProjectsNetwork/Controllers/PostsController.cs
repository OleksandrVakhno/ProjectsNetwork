using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
