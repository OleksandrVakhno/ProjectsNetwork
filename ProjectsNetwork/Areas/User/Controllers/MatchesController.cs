using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectsNetwork.Models.ViewModels;
using ProjectsNetwork.Services.IServices;

namespace ProjectsNetwork.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class MatchesController : Controller
    {
        private readonly IProjectsService _projectsService;
        public MatchesController(IProjectsService projectsService)
        {
            this._projectsService = projectsService;
        }

        public IActionResult Accepted()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var projects = this._projectsService.GetAcceptedProjects(currentUserID);
            return View(projects);
        }

        public IActionResult Matches()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var projects = this._projectsService.GetMatches(currentUserID);
            return View(projects);
        }


        public IActionResult Interested(int projectId)
        {
            var interestedUsers = this._projectsService.GetInterested(projectId);
            var interestedView = new InterestedViewModel { ProjectId = projectId, InterestedUsers = interestedUsers };

            return View(interestedView);
        }


        public IActionResult AcceptInterest(int projectId, string userId)
        {
            var result = this._projectsService.AcceptInterest(userId, projectId);
            if (!result)
            {
                return BadRequest("Failed to accept interest");
            }

            return RedirectToAction(nameof(Accepted));

        }

    }
}
