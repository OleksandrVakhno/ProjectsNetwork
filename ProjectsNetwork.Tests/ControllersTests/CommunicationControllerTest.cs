using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjectsNetwork.Areas.User.Controllers;
using ProjectsNetwork.Controllers;
using ProjectsNetwork.Data;
using ProjectsNetwork.DataAccess.Repositories;
using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using ProjectsNetwork.Models;
using ProjectsNetwork.Services.IApplicationService;
using ProjectsNetwork.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace ProjectsNetwork.Tests.ControllersTests
{
    public class CommunicationControllerTest
    {
        private readonly ControllerContext controllerContext;
        private readonly Mock<IProjectsService> mockProjectService;
        private Project project;
        private readonly IUserSkillRepository _skills;
        private readonly IProjectRepository _projectRepository;
        private readonly Mock<ISkillsService> mockSkillsService;
        private readonly Mock<IApplicationUserService> mockApplicationUserService;
        

        public CommunicationControllerTest()
        {
            this.mockProjectService = new Mock<IProjectsService>();

            var user = new ApplicationUser() { UserName = "AppUser", Id = "1" };

            var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim("name", user.UserName),
                };

            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            this.controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            this.Seed();
        }

        [Fact]
        public void Seed()
        {
            var project = new Project
            {
                Id = 0,
                UserId = "1",
                Name = "project1",
                Description = "new project 1",
                CreationDate = DateTime.Now,
            };

        }

        [Fact]
        public void UserInfoTest()
        {

            var user = new ApplicationUser() { UserName = "AppUser", Id = "1" };
            mockApplicationUserService.Setup(service => service.GetApplicationUser(It.IsAny<string>())).Returns(user);
            var skills = _skills.GetAll().ToList();
            mockSkillsService.Setup(service => service.GetMySkills(It.IsAny<string>())).Returns(skills);
            var projects = _projectRepository.GetAll();
            mockProjectService.Setup(service => service.GetUserProjects(It.IsAny<string>())).Returns(projects);
            var controller = new CommunicationController(mockProjectService.Object,mockApplicationUserService.Object,mockSkillsService.Object);
            controller.ControllerContext = this.controllerContext;
            var result = controller.UserInfo("1");
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<ApplicationUser>(viewResult.ViewData.Model);

            mockProjectService.Reset();
            mockApplicationUserService.Reset();
            mockSkillsService.Reset();

            user = null;
            mockApplicationUserService.Setup(service => service.GetApplicationUser(It.IsAny<string>())).Returns(user);
            skills = _skills.GetAll().ToList();
            mockSkillsService.Setup(service => service.GetMySkills(It.IsAny<string>())).Returns(skills);
            projects = _projectRepository.GetAll();
            mockProjectService.Setup(service => service.GetUserProjects(It.IsAny<string>())).Returns(projects);
            controller = new CommunicationController(mockProjectService.Object, mockApplicationUserService.Object, mockSkillsService.Object);
            controller.ControllerContext = this.controllerContext;
            result = controller.UserInfo("1");
            Assert.Equal("User not found", result.ToString());

            mockProjectService.Reset();
            mockApplicationUserService.Reset();
            mockSkillsService.Reset();

        }
    }
    
}
