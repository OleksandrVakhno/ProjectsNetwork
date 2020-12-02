using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjectsNetwork.Areas.User.Controllers;
using ProjectsNetwork.Controllers;
using ProjectsNetwork.Data;
using ProjectsNetwork.DataAccess.Repositories;
using ProjectsNetwork.Models;
using ProjectsNetwork.Models.ViewModels;
using ProjectsNetwork.Services.IServices;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Xunit;
namespace ProjectsNetwork.Tests.ControllersTests
{
    public class MatchesControllerTest
    {
        private readonly ControllerContext controllerContext;
        private readonly Mock<IProjectsService> mockProjectService;
        private List<ApplicationUser> users;
        private List<Project> projects;
        private List<InterestedInProject> interests;

        public MatchesControllerTest()
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
            var user = new ApplicationUser() { UserName = "AppUser", Id = "1" };
            this.users = new List<ApplicationUser>() { user };

            var project = new Project
            {
                Id = 0,
                UserId = "1",
                Name = "project1",
                Description = "new project 1",
                CreationDate = DateTime.Now,
            };
            this.projects = new List<Project>() { project };

            var interested = new InterestedInProject()
            {
                UserId = "1",
                ProjectId = 1,
                User = user,
                Project = project,
                Confirmed = true
            };
            this.interests = new List<InterestedInProject>() { interested };




        }

        [Fact]
        public void AcceptedTest()
        {
            mockProjectService.Setup(service => service.GetAcceptedProjects(It.IsAny<string>())).Returns(this.interests);
            var controller = new MatchesController(mockProjectService.Object);
            controller.ControllerContext = this.controllerContext;
            var result = controller.Accepted();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<List<InterestedInProject>>(viewResult.ViewData.Model);

            mockProjectService.Reset();

        }

        [Fact]
        public void MatchesTest()
        {
            mockProjectService.Setup(service => service.GetMatches(It.IsAny<string>())).Returns(this.interests);
            var controller = new MatchesController(mockProjectService.Object);
            controller.ControllerContext = this.controllerContext;
            var result = controller.Matches();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<List<InterestedInProject>>(viewResult.ViewData.Model);

            mockProjectService.Reset();
        }

        [Fact]
        public void InterestedTest()
        {
            mockProjectService.Setup(service => service.GetInterested(It.IsAny<int>())).Returns(this.users);
            var controller = new MatchesController(mockProjectService.Object);
            controller.ControllerContext = this.controllerContext;
            var result = controller.Interested(0);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<InterestedViewModel>(viewResult.ViewData.Model);

            mockProjectService.Reset();
        }

        [Fact]
        public void AcceptInterestTest()
        {
            mockProjectService.Setup(service => service.AcceptInterest(It.IsAny<string>(), It.IsAny<int>())).Returns(true);
            var controller = new MatchesController(mockProjectService.Object);
            controller.ControllerContext = this.controllerContext;
            var result = controller.AcceptInterest(0, "1");
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(viewResult.ControllerName);
            Assert.Equal("Accepted", viewResult.ActionName);

            mockProjectService.Reset();

            mockProjectService.Setup(service => service.AcceptInterest(It.IsAny<string>(), It.IsAny<int>())).Returns(false);
            controller = new MatchesController(mockProjectService.Object);
            controller.ControllerContext = this.controllerContext;
            result = controller.AcceptInterest(0, "1");
            Assert.IsType<BadRequestObjectResult>(result);

            mockProjectService.Reset();
        }
    }

}
