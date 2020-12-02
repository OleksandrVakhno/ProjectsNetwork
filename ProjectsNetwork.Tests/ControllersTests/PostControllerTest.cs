using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjectsNetwork.Controllers;
using ProjectsNetwork.DataAccess.Repositories;
using ProjectsNetwork.Models;
using ProjectsNetwork.Services.IServices;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace ProjectsNetwork.Tests.ControllersTests
{


    public class PostsControllerTest
    {
        private readonly ControllerContext controllerContext;
        private readonly Mock<ISkillsService> mockSkillService;
        private readonly Mock<IProjectsService> mockProjectService;
        private List<Skill> skills;
        private List<UserSkill> userSkills;
        private Project project;
        private readonly ProjectRepository _projectRepository;

        public PostsControllerTest()
        {
            this.mockSkillService = new Mock<ISkillsService>();
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

            var skill1 = new Skill { Id = 1, SkillName = "Java" };
            var skill2 = new Skill { Id = 2, SkillName = "C#" };
            this.skills = new List<Skill>() { skill1, skill2 };


            var userSkill1 = new UserSkill { UserId = "1", Skill = skill1 };
            var userSkill2 = new UserSkill { UserId = "1", Skill = skill2 };
            this.userSkills = new List<UserSkill>() { userSkill1, userSkill2 };

            var project = new Project
            {
                Id = 0,
                UserId = "1",
                Name = "project1",
                Description = "new project 1",
                CreationDate = DateTime.Now,
            };

            _projectRepository.Insert(project);

        }

        [Fact]
        public void IndexTest()
        {

            mockSkillService.Setup(service => service.GetAll()).Returns(this.skills);
            _projectRepository.Insert(project);
            var projects = this._projectRepository.GetAll();
            mockProjectService.Setup(service => service.GetFiltered(It.IsAny<string>(), null)).Returns(projects);
            var controller = new PostsController(mockProjectService.Object, mockSkillService.Object);
            controller.ControllerContext = this.controllerContext;
            var result = controller.Index("Java");
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<Project>(viewResult.ViewData.Model);


            mockSkillService.Reset();
            mockProjectService.Reset();
        }

        [Fact]
        public void MyProjectsTest()
        {
            mockSkillService.Setup(service => service.GetAll()).Returns(this.skills);
            var projects = this._projectRepository.GetAll();
            mockProjectService.Setup(service => service.GetUserProjects(It.IsAny<string>())).Returns(projects);
            var controller = new PostsController(mockProjectService.Object, mockSkillService.Object);
            controller.ControllerContext = this.controllerContext;
            var result = controller.MyProjects();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<Project>(viewResult.ViewData.Model);
        }

        [Fact]
        public void AddNewSkillTest()
        {
            mockSkillService.Setup(service => service.AddSkill(It.IsAny<Skill>())).Returns(true);
            var controller = new PostsController(mockProjectService.Object, mockSkillService.Object);
            var result = controller.AddNewSkill(new Skill());
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(viewResult.ControllerName);
            Assert.Equal("Index", viewResult.ActionName);

            mockSkillService.Reset();
            mockProjectService.Reset();

            mockSkillService.Setup(service => service.AddSkill(It.IsAny<Skill>())).Returns(false);
            controller = new PostsController(mockProjectService.Object, mockSkillService.Object);
            var ex = Assert.Throws<Exception>(() => controller.AddNewSkill(new Skill()));
            Assert.Equal("Could not add skill.", ex.Message);

            mockSkillService.Reset();
            mockProjectService.Reset();
        }

        [Fact]
        public void LearnTest()
        {
            mockSkillService.Setup(service => service.GetProjectSkills(It.IsAny<int>())).Returns(this.skills);
            mockProjectService.Setup(service => service.GetProject(It.IsAny<int>())).Returns(this.project);
            var controller = new PostsController(mockProjectService.Object, mockSkillService.Object);
            var result = controller.Learn(0);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<Learn>(viewResult.ViewData.Model);

            mockSkillService.Reset();
            mockProjectService.Reset();
        }

        [Fact]
        public void SubmitInterestTest()
        {
            mockProjectService.Setup(service => service.SubmitInterest(It.IsAny<string>(), It.IsAny<int>())).Returns(true);
            var controller = new PostsController(mockProjectService.Object, mockSkillService.Object);
            var result = controller.SubmitInterest(0);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Learn", viewResult.ActionName);

            mockSkillService.Reset();
            mockProjectService.Reset();
        }

        [Fact]
        public void CancelInterestTest()
        {
            mockProjectService.Setup(service => service.CancelInterest(It.IsAny<string>(), It.IsAny<int>())).Returns(true);
            var controller = new PostsController(mockProjectService.Object, mockSkillService.Object);
            controller.SubmitInterest(0);
            var result = controller.CancelInterest(0);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Learn", viewResult.ActionName);
        }


    }
}