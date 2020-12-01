using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectsNetwork.Models;
using ProjectsNetwork.Models.ViewModels;
using ProjectsNetwork.Services.IApplicationService;
using ProjectsNetwork.Services.IServices;
using ProjectsNetwork.Utils;

namespace ProjectsNetwork.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class CommunicationController : Controller
    {
        private readonly IProjectsService _projectsService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly ISkillsService _skillsService;

        public CommunicationController(IProjectsService projectsService, IApplicationUserService applicationUserService, ISkillsService skillsService)
        {
            _projectsService = projectsService;
            _applicationUserService = applicationUserService;
            _skillsService = skillsService;
        }

      
        public IActionResult UserInfo(int id)
        {
            var project = _projectsService.GetProject(id);
            if(project == null)
            {
                return NotFound("Project not found");
            }
            ApplicationUser applicationUserToContact = _applicationUserService.GetApplicationUser(project.UserId);
            applicationUserToContact.Skills = _skillsService.GetMySkills(project.UserId);
            foreach(UserSkill userSkill in applicationUserToContact.Skills)
            {
                userSkill.Skill = _skillsService.GetASkill(userSkill.SkillId);
            }
            applicationUserToContact.Projects = (List<Project>)_projectsService.GetUserProjects(project.UserId);

            if(applicationUserToContact == null)
            {
                return NotFound("User not found");
            }
            return View(applicationUserToContact);
        }

        [ActionName("UserInfoAccepted")]
        public IActionResult UserInfo(string userId)
        {
  
            ApplicationUser applicationUserToContact = _applicationUserService.GetApplicationUser(userId);
            applicationUserToContact.Skills = _skillsService.GetMySkills(userId);
            foreach (UserSkill userSkill in applicationUserToContact.Skills)
            {
                userSkill.Skill = _skillsService.GetASkill(userSkill.SkillId);
            }
            applicationUserToContact.Projects = (List<Project>)_projectsService.GetUserProjects(userId);

            if (applicationUserToContact == null)
            {
                return NotFound("User not found");
            }
            return View("UserInfo",applicationUserToContact);
        }


    }
}
